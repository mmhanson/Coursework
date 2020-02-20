#lang racket/base
(require racket/cmdline
         racket/string
         racket/set
         racket/format
         net/url
         net/head
         racket/port)

(define introduce? #f)
(define multi? #f)

(define-values (host port)
  (command-line
   #:once-each
   [("--multi") "Try more than one friend at a time"
    (set! multi? #t)]
   [("--introduce") "Test the \"introduce\" service"
    (set! introduce? #t)]
   #:args (host port)
   (values host port)))

(define user1 "alice")
(define user2 "bob")
(define user3 "carol")
(define user4 "dan")
(define user5 "eve")
(define user6 "frank")
(define user7 "grace")

(define root-url
  (string->url
   (format "http://~a:~a/" host port)))

(define fail? #f)

(define (message what user1 user2
                 #:friends-tag [friends-tag 'friends]
                 #:extras [extras null])
  (printf "~a: ~a <- ~a\n" what user1 (regexp-replace* #rx"\n" user2 " "))
  (call/input-url (struct-copy url (combine-url/relative root-url what)
                               [query (append
                                       extras
                                       (list
                                        (cons 'user user1)
                                        (cons friends-tag user2)))])
                  get-pure-port
                  port->string))

(define (string->friends str)
  (list->set (string-split str #rx"\n")))

(define (friends->string . fs)
  (string-join fs "\n"))
  
(define (check-included str user1 user2 expected?)
  (define friends (string->friends str))
  (for ([user2 (in-set (string->friends user2))])
    (unless (equal? (set-member? friends user2) expected?)
      (set! fail? #t)
      (eprintf (string-append (if expected?
                                  "new friend missing in result\n"
                                  "newly unfriended still in result\n")
                              "  user: ~a\n"
                              "  friend in result: ~a\n")
               user1
               user2))))

(define (befriend user1 user2)
  (check-included (message "befriend" user1 user2)
                  user1 user2
                  #t))

(define (unfriend user1 user2)
  (check-included (message "unfriend" user1 user2)
                  user1 user2
                  #f))

(define (get-friends user)
  (string->friends
   (call/input-url (struct-copy url (combine-url/relative root-url
                                                          "friends")
                                [query (list
                                        (cons 'user user))])
                   get-pure-port
                   port->string)))

(define (check-friends user new expected #:should-change? [should-change? #t])
  (unless (equal? new expected)
    (set! fail? #t)
    (eprintf (string-append (if should-change?
                                "friends didn't change in the expected way\n"
                                "friends changed unexpectedly\n")
                            "  user: ~a\n"
                            "  expected: ~a\n"
                            "  got: ~a\n")
             user
             (set->string expected)
             (set->string new))))

(define (set->string s)
  (apply ~a #:separator " " (set->list s)))

;; ----------------------------------------

;; Get initial friends
(define orig-f1 (get-friends user1))
(define orig-f2 (get-friends user2))

;; Add to friends
(befriend user1 user2)

(define new-f1 (get-friends user1))
(define new-f2 (get-friends user2))

(define expected-f1 (set-add orig-f1 user2))
(define expected-f2 (set-add orig-f2 user1))

(check-friends user1 new-f1 expected-f1)
(check-friends user2 new-f2 expected-f2)

;; Re-friending should have no effect
(befriend user2 user1)

(check-friends user1 (get-friends user1) new-f1)
(check-friends user2 (get-friends user2) new-f2)

;; Add other friends
(befriend user3 user4)

(define new-c3 (get-friends user3))

;; Check old friends
(check-friends user1 (get-friends user1) new-f1 #:should-change? #f)
(check-friends user2 (get-friends user2) new-f2 #:should-change? #f)

;; Another friend
(befriend user1 user3)

(check-friends user1 (get-friends user1) (set-add new-f1 user3))

;; Unfriend
(unfriend user1 user2)

(check-friends user1 (get-friends user1) (set-remove (set-add new-f1 user3) user2))
(check-friends user2 (get-friends user2) (set-remove new-f2 user1))

;; Multiple friends at once
(when multi?
  (define orig-f5 (get-friends user5))
  (define orig-f6 (get-friends user6))
  (define orig-f7 (get-friends user7))
  
  (befriend user5 (friends->string user6 user7))
  (check-friends user5 (get-friends user5) (set-union orig-f5 (set user6 user7)))
  (check-friends user6 (get-friends user6) (set-add orig-f6 user5))
  (check-friends user7 (get-friends user6) (set-add orig-f7 user5))

  (unfriend user5 (friends->string user7 user6))
  (check-friends user5 (get-friends user5) (set-subtract orig-f5 (set user6 user7)))
  (check-friends user6 (get-friends user6) (set-remove orig-f6 user5))
  (check-friends user7 (get-friends user6) (set-remove orig-f7 user5)))

;; Introduce
(when introduce?
  (message "introduce" user1 user3
           #:friends-tag 'friend
           #:extras `([host . ,host]
                      [port . ,port]))
  (check-friends user1 (get-friends user1) (set-union (set-remove (set-add new-f1 user3) user2)
                                                      (set-remove new-c3 user1))))

;; Conclusion
(if fail?
    (exit 1)
    (printf "Simple tests passed\n"))
