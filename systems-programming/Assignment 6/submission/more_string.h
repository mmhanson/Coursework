/* Allocates a new string to append all of the given strings. Any
   number of strings can be provided, and NULL must be provided after
   the last string.

   For example,
      append_strings("apple", "banana", "coconut", NULL)
   returns a fresh string
      "applebananacoconut"
*/
char *append_strings(const char *s, ...);

/* Allocates a string with the decimal representation of the number
   `v`: */
char *to_string(long v);

/* Returns 1 if `s` starts with `starts` --- that is, if `s` is at
   least `strlen(starts)` characters long and if the first
   `strlen(starts)` characters match `starts`: */
int starts_with(char *starts, char *s);

/* Takes a string and splits it into an array of strings that were
   separated/terminated by `sep` in the original string, where `sep`
   can be any byte value except 0. If the string ends in `sep`, then
   an ending empty string is *not* included in the result array (i.e.,
   `sep` is treated as a terminator). The result array is
   NULL-terminated. The array is freshly allocated, and each string in
   the array is freshly allocated. */
char **split_string(const char *str, char sep);

/* Combines every string in a NULL-terminated array of strings into
   one string with `sep` added as a terminator after each string. */
char *join_strings(const char * const *strs, char sep);

/* Parses an HTTP request line, returning 0 if parsing fails
   and 1 otherwise. If parsing succeeds, `method_p`, `uri_p`,
   and `version_p` are set to `malloc`ed strings for the
   three parts of the request. Any of the `..._p` arguments
   can be NULL if the corresponding part is not wanted. */
int parse_request_line(const char *buf,
                       char **method_p, char **uri_p, char **version_p);

/* Parses an HTTP response status line, returning 0 if parsing fails
   and 1 otherwise. If parsing succeeds, `version_p`, `status_p`,
   and `desc_p` are set to `malloc`ed strings for the
   three parts of the response. Any of the `..._p` arguments
   can be NULL if the corresponding part is not wanted. */
int parse_status_line(const char *buf,
                      char **version_p, char **status_p, char **desc_p);

/* Parses a single HTTP header line, adding a mapping from the field
   name to the field value (as a `malloc`ed string) to `d`: */
void parse_header_line(char *buf, dictionary_t *d);

/* Parses a query string (as in the query part of a URL), adding to
   `d` to map each field name to each value (as a `malloc`ed
   string), recognizing both "&" and ";" as query separators: */
void parse_query(const char *buf, dictionary_t *d);

/* Parses the query part, if any, of a URL (i.e., the part after the
   first "?") into the dictionary `d`: */
void parse_uriquery(const char *buf, dictionary_t *d);

/* Returns a freshly allocated string that is like the given one,
   except that every non-ASCII, non-alphabetic, or non-numeric
   character is encoded in "%" form: */
char *query_encode(const char *);

/* Returns a freshly allocated string that is like the given one,
   except that "%" encodings are converted to the encoded character
   and "+" is converted to a space: */
char *query_decode(const char *);

/* Returns a freshly allocated string that is like the given one,
   except that each `<`, `>`, `&`, and `"` character is converted to
   its `&lt;`, `&gt;`, `&amp;`, and `&quot;` encoding, respectively: */
char *entity_encode(const char *);
