/*
 * friendlist.c - [Starting code for] a web-based friend-graph manager.
 *
 * Based on:
 *  tiny.c - A simple, iterative HTTP/1.0 Web server that uses the 
 *      GET method to serve static and dynamic content.
 *   Tiny Web server
 *   Dave O'Hallaron
 *   Carnegie Mellon University
 */
#include "csapp.h"
#include "dictionary.h"
#include "more_string.h"
#include <string.h>

typedef struct FriendListLinkTag FriendListLink;
struct FriendListLinkTag
{
    char* friend;
    FriendListLink* prev;
    FriendListLink* next;
};

static void doit(int fd);
static dictionary_t *read_requesthdrs(rio_t *rp);
static void read_postquery(rio_t *rp, dictionary_t *headers, dictionary_t *d);
static void clienterror(int fd, char *cause, char *errnum, 
                        char *shortmsg, char *longmsg);
static void print_stringdictionary(dictionary_t *d);
static void serve_request(int fd, dictionary_t *query);
static void serve_friends(int fd, dictionary_t* query);
static void serve_befriend(int fd, dictionary_t* query);
static void serve_unfriend(int fd, dictionary_t* query);
static void serve_introduce(int fd, dictionary_t* query);
/* custom helpers */
static void free_friendlist(void* friendlist);
static void free_friendlist_link(void* friendlist_link);
static void make_friends(const char* user0, const char* user1);
static void unmake_friends(const char* user0, const char* user1);
static void add_friend(char* user, char* friend);
static void del_friend(const char* user, const char* friend);
static char* get_friends(char* user);
static char** get_external_friends(const char* friend, const char* hostname, const char* port);
static int connect_to(const char* hostname, const char* port);
static void send_request(int conn, const char* header);
static void send_ok_response(int conn, const char* body);
static dictionary_t* parse_response(rio_t* rio);
void sigint_handler(int arg);

/* map a string to a string of their friend */
dictionary_t* friends;

#define FOR_EACH_FRIEND(cursor, user) \
    for (cursor = dictionary_get(friends, user); \
         cursor != NULL; \
         cursor = cursor->next)
#define FOR_EACH_CHAR(cursor, string) \
    for (cursor = string; \
         *cursor != '\0'; \
         cursor += 1)
void sigint_handler(int arg)
{
    free_dictionary(friends);
    exit(0);
}

int main(int argc, char **argv)
{
  int listenfd, connfd;
  char hostname[MAXLINE], port[MAXLINE];
  socklen_t clientlen;
  struct sockaddr_storage clientaddr;

  /* Check command line args */
  if (argc != 2) {
    fprintf(stderr, "usage: %s <port>\n", argv[0]);
    exit(1);
  }

  listenfd = Open_listenfd(argv[1]);

  /* Don't kill the server if there's an error, because
     we want to survive errors due to a client. But we
     do want to report errors. */
  exit_on_error(0);

  /* Also, don't stop on broken connections: */
  Signal(SIGPIPE, SIG_IGN); // dont crash on broken connections
  Signal(SIGINT, sigint_handler); // to free resources before closing down

  /* init friends dictionary */
  friends = make_dictionary(COMPARE_CASE_SENS, free_friendlist);

  while (1) {
    clientlen = sizeof(clientaddr);
    connfd = Accept(listenfd, (SA *)&clientaddr, &clientlen);
    if (connfd >= 0) {
      Getnameinfo((SA *) &clientaddr, clientlen, hostname, MAXLINE, 
                  port, MAXLINE, 0);
      printf("Accepted connection from (%s, %s)\n", hostname, port);
      doit(connfd);
      Close(connfd);
    }
  }
}

/*
 * doit - handle one HTTP request/response transaction
 */
void doit(int fd)
{
    char buf[MAXLINE];
    char* method;
    char* uri;
    char* version;
    rio_t rio;
    dictionary_t *headers, *query;
    
    /* Read request line and headers */
    Rio_readinitb(&rio, fd);
    if (Rio_readlineb(&rio, buf, MAXLINE) <= 0)
    {
        return;
    }
    printf("%s", buf);
    
    if (!parse_request_line(buf, &method, &uri, &version))
    {
        clienterror(fd, method, "400", "Bad Request",
                    "Friendlist did not recognize the request");
    }
    else
    {
        if (strcasecmp(version, "HTTP/1.0")
            && strcasecmp(version, "HTTP/1.1"))
        {
            /* if version != 1.0 && version != 1.1 */
            clienterror(fd, version, "501", "Not Implemented",
                        "Friendlist does not implement that version");
        }
        else if (strcasecmp(method, "GET")
                 && strcasecmp(method, "POST"))
        {
            /* if method not equal to GET or POST */
            clienterror(fd, method, "501", "Not Implemented",
                        "Friendlist does not implement that method");
        }
        else
        {
            headers = read_requesthdrs(&rio);
            
            /* Parse all query arguments into a dictionary */
            query = make_dictionary(COMPARE_CASE_SENS, free);
            parse_uriquery(uri, query);
            if (!strcasecmp(method, "POST"))
                read_postquery(&rio, headers, query);
            
            /* For debugging, print the dictionary */
            print_stringdictionary(query);
            
            /* You'll want to handle different queries here,
               but the intial implementation always returns
               nothing: */
            if (starts_with("/friends", uri))
            {
                serve_friends(fd, query);
            }
            else if (starts_with("/befriend", uri))
            {
                serve_befriend(fd, query);
            }
            else if (starts_with("/unfriend", uri))
            {
                serve_unfriend(fd, query);
            }
            else if (starts_with("/introduce", uri))
            {
                serve_introduce(fd, query);
            }
            else
            {
                serve_request(fd, query);
            }
            
            /* Clean up */
            free_dictionary(query);
            free_dictionary(headers);
        }
        
        /* Clean up status line */
        free(method);
        free(uri);
        free(version);
    }
}

/*
 * read_requesthdrs - read HTTP request headers
 */
dictionary_t *read_requesthdrs(rio_t *rp) {
  char buf[MAXLINE];
  dictionary_t *d = make_dictionary(COMPARE_CASE_INSENS, free);

  Rio_readlineb(rp, buf, MAXLINE);
  printf("%s", buf);
  while(strcmp(buf, "\r\n")) {
    Rio_readlineb(rp, buf, MAXLINE);
    printf("%s", buf);
    parse_header_line(buf, d);
  }
  
  return d;
}

void read_postquery(rio_t *rp, dictionary_t *headers, dictionary_t *dest) {
  char *len_str, *type, *buffer;
  int len;
  
  len_str = dictionary_get(headers, "Content-Length");
  len = (len_str ? atoi(len_str) : 0);

  type = dictionary_get(headers, "Content-Type");
  
  buffer = malloc(len+1);
  Rio_readnb(rp, buffer, len);
  buffer[len] = 0;

  if (!strcasecmp(type, "application/x-www-form-urlencoded")) {
    parse_query(buffer, dest);
  }

  free(buffer);
}

static char *ok_header(size_t len, const char *content_type)
{
  char *len_str, *header;
  
  header = append_strings("HTTP/1.0 200 OK\r\n",
                          "Server: Friendlist Web Server\r\n",
                          "Connection: close\r\n",
                          "Content-length: ", len_str = to_string(len), "\r\n",
                          "Content-type: ", content_type, "\r\n\r\n",
                          NULL);
  free(len_str);

  return header;
}

/*
 * serve_request - example request handler
 */
static void serve_request(int fd, dictionary_t *query)
{
  send_ok_response(fd, "alice\nbob");
}

/*
 * serve_friends
 * For URIs starting with '/friends'.
 */
static void serve_friends(int fd, dictionary_t* query)
{
    char* body;
    char* user;
    FriendListLink* friendlist;

    // GET QUERIES //
    user = dictionary_get(query, "user");
    if (user == NULL)
    {
        clienterror(fd, user, "400", "Bad Request",
                    "Must include a 'user' query.");
        return;
    }

    // MAKE RESPONSE //
    friendlist = dictionary_get(friends, user);
    if (friendlist == NULL)
    {
        body = strdup("");
    }
    else
    {
        body = get_friends(user);
    }

    send_ok_response(fd, body);
    free(body);
}

/*
 * serve_unfriend
 * For URIs starting with '/unfriend'.
 */
static void serve_unfriend(int fd, dictionary_t* query)
{
    char* body;
    char* user;
    char* friends_query;
    char** friends_arr;
    int idx;

    // GET QUERIES //
    user = dictionary_get(query, "user");
    friends_query = dictionary_get(query, "friends");
    if (user == NULL)
    {
        clienterror(fd, user, "400", "Bad Request",
                    "Must include a 'user' query.");
        return;
    }
    if (friends_query == NULL)
    {
        clienterror(fd, friends_query, "400", "Bad Request",
                    "Must include a 'friends' query.");
        return;
    }

    // REMOVE FRIENDS //
    friends_arr = split_string(friends_query, '\n');
    for (idx = 0; friends_arr[idx] != NULL; idx++)
    {
        unmake_friends(user, friends_arr[idx]); // TODO implement
        free(friends_arr[idx]);
    }
    free(friends_arr);

    // MAKE RESPONSE //
    body = get_friends(user);
    send_ok_response(fd, body);
    free(body);
}

/*
 * serve_befriend
 * For URIs starting with '/befriend'.
 */
static void serve_befriend(int fd, dictionary_t* query)
{
    char* body;
    char* user;
    char* friends_query;
    char** friends_arr;
    int idx;

    // GET QUERIES //
    user = dictionary_get(query, "user");
    friends_query = dictionary_get(query, "friends");
    if (user == NULL)
    {
        clienterror(fd, user, "400", "Bad Request",
                    "Must include a 'user' query.");
        return;
    }
    if (friends_query == NULL)
    {
        clienterror(fd, friends_query, "400", "Bad Request",
                    "Must include a 'friends' query.");
        return;
    }

    // ADD FRIENDS //
    friends_arr = split_string(friends_query, '\n');
    for (idx = 0; friends_arr[idx] != NULL; idx++)
    {
        make_friends(user, friends_arr[idx]);
        free(friends_arr[idx]);
    }
    free(friends_arr);

    // MAKE RESPONSE //
    body = get_friends(user);
    send_ok_response(fd, body);
    free(body);
}

/*
 * serve_introduce
 * For URIs starting with '/introduce'.
 */
static void serve_introduce(int fd, dictionary_t* query)
{
    char* body;
    char* user;
    char* friend_query;
    char* host_query;
    char* port_query;
    char** friends_arr;
    char* cursor;
    int idx;

    // GET QUERIES //
    user = dictionary_get(query, "user");
    friend_query = dictionary_get(query, "friend");
    host_query = dictionary_get(query, "host");
    port_query = dictionary_get(query, "port");
    if (user == NULL)
    {
        clienterror(fd, user, "400", "Bad Request",
                    "Must include a 'user' query.");
        return;
    }
    if (friend_query == NULL)
    {
        clienterror(fd, friend_query, "400", "Bad Request",
                    "Must include a 'friend' query.");
        return;
    }
    if (host_query == NULL)
    {
        clienterror(fd, host_query, "400", "Bad Request",
                    "Must include a 'host' query.");
        return;
    }
    if (port_query == NULL)
    {
        clienterror(fd, port_query, "400", "Bad Request",
                    "Must include a 'port' query.");
        return;
    }

    friends_arr = get_external_friends(friend_query, host_query, port_query);
    for (idx = 0; friends_arr[idx] != NULL; idx++)
    {
        cursor = friends_arr[idx];
        printf("Friend! %s \n", cursor);

        if (strcmp(user, cursor) != 0)
        {
            make_friends(user, cursor);
        }
        free(cursor);
    }
    free(friends_arr);

    // MAKE RESPONSE //
    body = get_friends(user);
    send_ok_response(fd, body);
    free(body);
}

/*
 * clienterror - returns an error message to the client
 */
void clienterror(int fd, char *cause, char *errnum, 
		 char *shortmsg, char *longmsg) {
  size_t len;
  char *header, *body, *len_str;

  body = append_strings("<html><title>Friendlist Error</title>",
                        "<body bgcolor=""ffffff"">\r\n",
                        errnum, " ", shortmsg,
                        "<p>", longmsg, ": ", cause,
                        "<hr><em>Friendlist Server</em>\r\n",
                        NULL);
  len = strlen(body);

  /* Print the HTTP response */
  header = append_strings("HTTP/1.0 ", errnum, " ", shortmsg, "\r\n",
                          "Content-type: text/html; charset=utf-8\r\n",
                          "Content-length: ", len_str = to_string(len), "\r\n\r\n",
                          NULL);
  free(len_str);
  
  Rio_writen(fd, header, strlen(header));
  Rio_writen(fd, body, len);

  free(header);
  free(body);
}

static void print_stringdictionary(dictionary_t *d) {
  int i, count;

  count = dictionary_count(d);
  for (i = 0; i < count; i++) {
    printf("%s=%s\n",
           dictionary_key(d, i),
           (const char *)dictionary_value(d, i));
  }
  printf("\n");
}

/*
 * Make two users friends with each other.
 * @user0 and @user1 are copied into fresh strings, these fresh strings are
 * used in the friendlists of these users.
 */
static void make_friends(const char* user0, const char* user1)
{
    char* user0_fresh;
    char* user1_fresh;

    user0_fresh = strdup(user0);
    user1_fresh = strdup(user1);

    add_friend(user0_fresh, user1_fresh);
    add_friend(user1_fresh, user0_fresh);
}

/*
 * Make two people not friends anymore.
 * @user0 and @user1 will be ready to free on return.
 */
static void unmake_friends(const char* user0, const char* user1)
{
    del_friend(user0, user1);
    del_friend(user1, user0);
}

/*
 * Add a friend to a users friendlist.
 *
 * The list of FriendListLinks pointed to by friends[user] will contain @friend.
 * @friend will be pointed to by @user's friendlist. Do not free it. It will be
 * freed in del_friend().
 */
static void add_friend(char* user, char* friend)
{
    FriendListLink* friendlist;
    FriendListLink* new_link;
    FriendListLink* cursor;

    // MAKE LINK //
    friendlist = dictionary_get(friends, user);
    new_link = malloc(sizeof(FriendListLink));
    new_link->friend = friend;

    // APPEND //
    if (friendlist == NULL)
    {
        new_link->prev = NULL;
        new_link->next = NULL;
        dictionary_set(friends, user, new_link);
    }
    else
    {
        cursor = friendlist;
        if (strcmp(cursor->friend, friend) == 0)
        {
            free(new_link);
            free(friend);
            return; // trying to add a friend already in the friendlist
        }
        while (cursor->next != NULL)
        {
            if (strcmp(cursor->friend, friend) == 0)
            {
                free(new_link);
                free(friend);
                return; // trying to add a friend already in the friendlist
            }
            cursor = cursor->next;
        }
        new_link->prev = cursor;
        new_link->next = NULL;
        cursor->next = new_link;
    }
}

/*
 * Remove a friend from a user's friendlist.
 *
 * Will free all memory associated with @friend's link in @user's friendlist.
 * If @user is not friends with @friend, then nothing happens.
 */
static void del_friend(const char* user, const char* friend)
{
    FriendListLink* cursor;
    FriendListLink* next_head;
    int found_friend_flag;

    // FIND FRIEND //
    found_friend_flag = 0;
    FOR_EACH_FRIEND(cursor, user)
    {
        if (strcmp(cursor->friend, friend) == 0)
        {
            found_friend_flag = 1;
            break;
        }
    }
    if (!found_friend_flag)
    {
        return; // @user is not friends with @friend
    }

    // UNLINK THE FRIENDS LINK //
    if (cursor->prev != NULL)
    {
        cursor->prev->next = cursor->next;
    }
    if (cursor->next != NULL)
    {
        cursor->next->prev = cursor->prev;
    }

    // FREE THE LINK //
    if (cursor == dictionary_get(friends, user))
    {
        if (cursor->next == NULL)
        {
            dictionary_remove(friends, user); // was the only link
        }
        else
        {
            next_head = cursor->next;
            cursor->next = NULL; // to avoid freeing entire friendlist
            dictionary_set(friends, user, next_head); // link new chain
        }
    }
    else
    {
        free_friendlist_link(cursor);
    }
}

/*
 * Get a string containing a user's friends.
 * List is separated by newline characters and terminated by null.
 * Returns empty string if no friends.
 */
static char* get_friends(char* user)
{
    char* friendstring;
    unsigned int friendstring_len;
    char* friendstring_cursor;
    char* friend_cursor;
    FriendListLink* friendlist_cursor;

    // GET NEEDED LENGTH OF FRIENDLIST //
    friendstring_len = 0;
    FOR_EACH_FRIEND(friendlist_cursor, user)
    {
        friendstring_len += strlen(friendlist_cursor->friend);
        friendstring_len += 1; // newline
    }
    friendstring_len += 1; // null terminator

    // COMPILE FRIENDLIST //
    friendstring = malloc(friendstring_len * sizeof(char));
    friendstring_cursor = friendstring;
    FOR_EACH_FRIEND(friendlist_cursor, user)
    {
        FOR_EACH_CHAR(friend_cursor, friendlist_cursor->friend)
        {
            *friendstring_cursor = *friend_cursor;
            friendstring_cursor += 1;
        }
        *friendstring_cursor = '\n';
        friendstring_cursor += 1;
    }
    *friendstring_cursor = '\0';

    return friendstring;
}

/*
 * Free the resources associated with a friendlist.
 *
 * @friendlist: A FriendListLink ptr. all FriendListLinks that are after it are
 * freed (ie they can be reached through @friendlist->next).
 */
static void free_friendlist(void* friendlist)
{
    FriendListLink* cursor;
    FriendListLink* next;

    cursor = (FriendListLink*)friendlist;
    while (cursor != NULL)
    {
        next = cursor->next;
        free_friendlist_link(cursor);
        cursor = next;
    }
}

/*
 * Free the resources associated with a friendlist_link.
 *
 * @friendlist_link. A FriendListLink ptr.
 */
static void free_friendlist_link(void* friendlist_link)
{
    FriendListLink* p = (FriendListLink*)friendlist_link;

    free(p->friend);
    free(p);
}

/*
 * Get a list of friends of a user from an external server.
 * All args will be ready to free after return.
 *
 * @return: An null-terminated array of strings containing:
 *   1. Each friend of @friend on the server specified by '@host:@port'.
 *   2. A (fresh copy) of @friend.
 */
static char** get_external_friends(const char* friend, const char* hostname, const char* port)
{
    int conn;
    char* request_header;
    dictionary_t* response_headers;
    rio_t rio;
    char** friendlist;

    conn = connect_to(hostname, port);
    request_header = append_strings("GET /friends?user=", friend, " HTTP/1.1\r\n",
                                    "Host: ", hostname, ":", port, "\r\n",
                                    "Accept: text/html\r\n\r\n",
                                    NULL);
    send_request(conn, request_header);

    Rio_readinitb(&rio, conn);
    response_headers = parse_response(&rio);

    if (response_headers == NULL)
    {
        return NULL; // invalid response
    }
    print_stringdictionary(response_headers);
    friendlist = split_string(dictionary_get(response_headers, "body"), '\n');

    free(request_header);
    free_dictionary(response_headers);
    printf("done!\n");
    return friendlist;
}

/*
 * Parse an HTTP response, put everything into a dictionary.
 * Maps the header (ie "version", "status", and "desc").
 * Maps each field.
 *
 * @return null if error in response.
 */
static dictionary_t* parse_response(rio_t* rio)
{
    dictionary_t* dict;
    char buf[MAXLINE];
    char* status;
    char* version;
    char* desc;
    char* body;
    char* tmp;
    int res;

    dict = make_dictionary(COMPARE_CASE_INSENS, free);

    // PARSE & STORE & PRINT STATUS LINE //
    Rio_readlineb(rio, buf, MAXLINE);
    res = parse_status_line(buf, &version, &status, &desc);
    if (res == 0)
    {
        return NULL; // parsing failed
    }
    dictionary_set(dict, "version", version);
    dictionary_set(dict, "status", status);
    dictionary_set(dict, "desc", desc);
    printf("%s", buf);

    // PARSE & STORE & PRINT EACH FIELD-VALUE PAIR //
    while(strcmp(buf, "\r\n") != 0)
    {
        Rio_readlineb(rio, buf, MAXLINE);
        printf("%s", buf);
        parse_header_line(buf, dict);
    }

    // READ & STORE & PRINT THE BODY //
    Rio_readlineb(rio, buf, MAXLINE); // assumes at least one line of body
    body = strdup(buf);
    while ( (res = Rio_readlineb(rio, buf, MAXLINE)) != 0 )
    {
        tmp = append_strings(body, buf, NULL);
        free(body);
        body = tmp;
    }
    dictionary_set(dict, "body", body);

    return dict;
}

/*
 * Make an IPv4, TCP connection to a server.
 */
static int connect_to(const char* hostname, const char* port)
{
    int conn;
    struct addrinfo hints;
    struct addrinfo* addrinfo;
    struct addrinfo* cursor;

    // PREPARE TO CONNECT //
    conn = 0;
    memset(&hints, 0, sizeof(hints));
    hints.ai_family = AF_INET;
    hints.ai_socktype = SOCK_STREAM;

    // CONNECT //
    Getaddrinfo(hostname, port, &hints, &addrinfo);
    for (cursor = addrinfo; cursor != NULL; cursor = cursor->ai_next)
    {
        // TODO server crashes if connection fails
        conn = Socket(cursor->ai_family, cursor->ai_socktype, cursor->ai_protocol);
        Connect(conn, cursor->ai_addr, cursor->ai_addrlen);
    }

    Freeaddrinfo(addrinfo);

    return conn;
}

/*
 * Send an HTTP request through a connection.
 */
static void send_request(int conn, const char* header)
{
    char* header_cpy;

    header_cpy = strdup(header); // since Rio_writen isnt const
    Rio_writen(conn, header_cpy, strlen(header_cpy));

    free(header_cpy);
}

/*
 * Send an 'OK' HTTP response through a connection.
 */
static void send_ok_response(int conn, const char* body)
{
    int len;
    char* header;
    char* body_cpy;

    body_cpy = strdup(body); // since Rio_writen isnt const
    len = strlen(body_cpy);
    header = ok_header(len, "text/html; charset=utf-8");

    Rio_writen(conn, header, strlen(header));
    Rio_writen(conn, body_cpy, len);

    printf("Response headers:\n");
    printf("%s", header);

    free(header);
    free(body_cpy);
}
