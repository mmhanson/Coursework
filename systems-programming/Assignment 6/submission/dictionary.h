/* A dictionary maps a string to a pointer. The pointer can be
   anything, such as another string. */

/* Opaque type for a dictionary instance: */
typedef struct dictionary_t dictionary_t;

/* A function provided to make_dictionary() that is used to destroy
   each value in the dictionary when the dictionary itself is
   destroyed or when the value is replaced with another value: */
typedef void (*free_proc_t)(void*);

/* Select between case-sensitive and case-insensitive
   string comparison for dictionary keys: */
#define COMPARE_CASE_SENS   0
#define COMPARE_CASE_INSENS 1

/* Creates a dictionary with the given comparison mode and
   value-destruction function, where the value-destruction function
   can be NULL: */
dictionary_t *make_dictionary(int compare_mode, free_proc_t free_value);

/* Destroys a dictionary, which frees all key strings -- and also
   destroys all values using the function provided to
   make_dictionary() if that function is not NULL: */
void free_dictionary(dictionary_t *d);

/* Sets the dictionary's mapping for `key` to `value`, destroying the
   value (if any) that `key` is currently mapped to. The dictionary
   makes its own copy of `key` and does not refer to that pointer on
   return. It keeps the `value` pointer as-is and effectively takes
   ownership of the value. */
void dictionary_set(dictionary_t *d, const char *key, void *value);

/* Removes the dictionary's mapping, if any, for `key`. */
void dictionary_remove(dictionary_t *d, const char *key);

/* Returns the dictionary's value for `key`, or NULL if the dictionary
   has no value for `key`. The dictionary retains ownership of the
   result value, so beware that the result can be destroyed if the
   mapping for `key` is changed. */
void *dictionary_get(dictionary_t *d, const char *key);

/* Returns the number of keys/values that are mapped in the
   dictionary: */
size_t dictionary_count(dictionary_t *d);

/* Returns one key in the dictionary, where `i` is between 0
   (inclusive) and dictionary_count(d) (exclusive). The dictionary
   retains ownership of the key, and it is valid only as long as the
   key is not removed from the dictionary. */
const char *dictionary_key(dictionary_t *d, size_t i);

/* Returns a NULL-terminated array of all keys in `d`. The dictionary
   retains ownership of each key, and each key is valid only as long as it
   is not removed from the dictionary. The caller takes ownership of the
   freshly allocated array. */
const char **dictionary_keys(dictionary_t *d);

/* Returns one value in the dictionary, where `i` is between 0
   (inclusive) and dictionary_count(d) (exclusive). */
void *dictionary_value(dictionary_t *d, size_t i);
