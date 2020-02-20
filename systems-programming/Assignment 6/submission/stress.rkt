#lang racket/base
(require racket/cmdline
         net/url
         net/uri-codec
         racket/port
         racket/string
         racket/list
         racket/place)

;; Number of entries will be the product of these:
(define NUM-PLACES 4)       ; base parallelism
(define NUM-THREADS 10)     ; more concurrency

(define NUM-USERS 4)
(define NUM-FRIENDS 100)

(define NUM-REPEATS 1)

(define FRIEND-LENGTH 10) ; size of each entry

;; Sending lots of headers sometimes will
;; make some connections sluggish, maybe worth
;; a try:
(define MAX-JUNK-HEADERS 0)

(define-values (host port)
  (command-line
   #:once-each
   [("--lo") "Run a short, low-stress variant"
    (set! NUM-USERS 2)
    (set! NUM-THREADS 2)
    (set! NUM-FRIENDS 2)]
   #:args (host port)
   (values host port)))

(module+ main
  (define results
    (map place-wait
         (for/list ([p NUM-PLACES])
           (define pl (run-group))
           (place-channel-put pl p)
           pl)))
  (unless (andmap zero? results)
    (raise-user-error 'stress "one or more places failed")))
  
(define user-1 "%%one%%")
(define user-2 "\">two?")

(define (run-group)
  (place
   pch
   (define p (sync pch))

   (define root-url
     (string->url
      (format "http://~a:~a/" host port)))

   (define friend-url (combine-url/relative root-url "befriend"))
   (define unfriend-url (combine-url/relative root-url "unfriend"))

   ;; maps a user to a known subset of its friends:
   (define friendss (make-hash))

   ;; all new friends created in this place:
   (define all-friends (make-hash))

   (define (check-expected-friends user [expected-friends #f])
     (define friends (hash-ref friendss user))
     (call/input-url (struct-copy url (combine-url/relative root-url
                                                            "friends")
                                  [query (list
                                          (cons 'user user))])
                     get-pure-port
                     (lambda (i)
                       (define reported-friends (make-hash))
                       (for ([l (in-lines i 'linefeed)])
                         (when (not (hash-ref friends l 'unknown))
                           (raise-user-error 'stress "~a: unexpected friend for ~s: ~s" p user l))
                         (when (hash-ref reported-friends l #f)
                           (raise-user-error 'stress "duplicate friend for ~s: ~s" user l))
                         (hash-set! reported-friends l #t))
                       (for ([expected-friend (in-hash-keys (or expected-friends friends))]
                             #:when (hash-ref friends expected-friend))
                         (unless (hash-ref reported-friends expected-friend #f)
                           (raise-user-error 'stress "missing friend for ~s: ~s" user expected-friend))))))

   (define (encode-post-query alist)
     (string->bytes/utf-8 (alist->form-urlencoded alist)))
   
   (define (befriend #:user user #:friends friends)
     (call/input-url (struct-copy url friend-url
                                  [query (list
                                          (cons 'user user))])
                     (lambda (url)
                       (post-pure-port url
                                       (encode-post-query (list (cons 'friends friends)))
                                       (cons
                                        "Content-Type: application/x-www-form-urlencoded"
                                        (for/list ([i (random (add1 MAX-JUNK-HEADERS))])
                                          (format "x-junk-~a: garbage" i)))))
                     port->string))

   (define (unfriend #:user user #:friends friends)
     (call/input-url (struct-copy url unfriend-url
                                  [query (list
                                          (cons 'user user))])
                     (lambda (url)
                       (post-pure-port url
                                       (encode-post-query (list (cons 'friends friends)))
                                       (cons
                                        "Content-Type: application/x-www-form-urlencoded"
                                        (for/list ([i (random (add1 MAX-JUNK-HEADERS))])
                                          (format "x-junk-~a: garbage" i)))))
                     port->string))

   (define (id->user j)
     (case j
       [(0) user-1]
       [(1) user-2]
       [else (format "u~a" j)]))

   (define place-user (format "p~a" p))

   (log-error "friending")
   (for-each
    sync
    (for/list ([k NUM-THREADS])
      (thread
       (lambda ()
         (for ([j NUM-USERS])
           (define user (id->user j))
           (define friends (hash-ref! friendss user make-hash))
           (define local-friends (make-hash))
           (for ([r NUM-REPEATS])
             (define new-friends
               (for/list ([i NUM-FRIENDS])
                 (format "f[~a,~a,~a,~a,~a]~a" p k j r i (make-string FRIEND-LENGTH #\*))))
             (define all-new-friends (string-join new-friends "\n"))
             (befriend #:user user #:friends all-new-friends)
             (befriend #:user place-user #:friends all-new-friends)
             (unfriend #:user user #:friends all-new-friends)
             (befriend #:user user #:friends all-new-friends)
             (for ([new-friend (in-list new-friends)])
               (hash-set! friends new-friend #t)
               (hash-set! local-friends new-friend #t)
               (hash-set! all-friends new-friend #t)))
           (check-expected-friends user local-friends))))))

   (log-error "checking friends")
   (for ([j NUM-USERS])
     (check-expected-friends (id->user j)))

   (log-error "unfriending")
   (for ([j NUM-USERS])
     (define user (id->user j))
     (define friends (hash-ref friendss user))
     (define all
       (string-join
        (for/list ([friend (in-hash-keys all-friends)])
          (hash-set! friends friend #f)
          friend)
        "\n"))
     (unfriend #:user user #:friends all))

   (log-error "checking friends")
   (for ([j NUM-USERS])
     (check-expected-friends (id->user j)))
   
   (log-error "introducing")
   (for ([j NUM-USERS])
     (define user (id->user j))
     (define friends (hash-ref! friendss user make-hash))
     (void (call/input-url (struct-copy url (combine-url/relative root-url
                                                                  "introduce")
                                        [query (list
                                                (cons 'user user)
                                                (cons 'friend place-user)
                                                (cons 'host host)
                                                (cons 'port port))])
                           get-pure-port
                           port->string))
     (for ([friend (in-hash-keys all-friends)])
       (hash-set! friends friend #t)))

   (log-error "checking friends")
   (for ([j NUM-USERS])
     (check-expected-friends (id->user j)))))
