#include <stdlib.h>
#include <string.h>
#include <strings.h>
#include <ctype.h>
#include "dictionary.h"

static int same_key(const char *key1, const char *key2, int compare_mode);

struct dictionary_t {
  int compare_mode;
  free_proc_t free_value;
  size_t count, alloc;
  const char **keys;
  void **values;
};

static void no_free(void *p) { }

dictionary_t *make_dictionary(int compare_mode, free_proc_t free_value) {
  dictionary_t *d = calloc(1, sizeof(dictionary_t));

  d->compare_mode = compare_mode;
  d->free_value = (free_value ? free_value : no_free);
  
  return d;
}

void free_dictionary(dictionary_t *d) {
  int i;
  
  for (i = 0; i < d->count; i++) {
    free((void *)d->keys[i]);
    d->free_value(d->values[i]);
  }

  if (d->keys)
    free(d->keys);
  if (d->values)
    free(d->values);
  free(d);
}

void dictionary_set(dictionary_t *d, const char *key, void *value) {
  int i;

  for (i = 0; i < d->count; i++) {
    if (same_key(key, d->keys[i], d->compare_mode)) {
      d->free_value(d->values[i]);
      d->values[i] = value;
      return;
    }
  }

  if (d->count == d->alloc) {
    d->alloc = 2 * (d->alloc + 1);
    d->keys = realloc(d->keys, d->alloc*sizeof(const char*));
    d->values = realloc(d->values, d->alloc*sizeof(void*));
  }

  d->keys[d->count] = strdup(key);
  d->values[d->count] = value;
  d->count++;
}

void dictionary_remove(dictionary_t *d, const char *key) {
  int i, j;

  for (i = 0; i < d->count; i++) {
    if (same_key(key, d->keys[i], d->compare_mode)) {
      free((void *)d->keys[i]);
      d->free_value(d->values[i]);
      for (j = i + 1; j < d->count; j++) {
        d->keys[j-1] = d->keys[j];
        d->values[j-1] = d->values[j];
      }
      --d->count;
      return;
    }
  }
}

void *dictionary_get(dictionary_t *d, const char *key) {
  int i;

  for (i = 0; i < d->count; i++) {
    if (same_key(key, d->keys[i], d->compare_mode))
      return d->values[i];
  }

  return NULL;
}

size_t dictionary_count(dictionary_t *d) {
  return d->count;
}

const char *dictionary_key(dictionary_t *d, size_t i) {
  return d->keys[i];
}

const char **dictionary_keys(dictionary_t *d) {
  int i;
  const char **strs = malloc(sizeof(char *) * (d->count + 1));
  for (i = 0; i < d->count; i++) {
    strs[i] = d->keys[i];
  }
  strs[i] = NULL;
  return strs;
}

void *dictionary_value(dictionary_t *d, size_t i) {
  return d->values[i];
}

static int same_key(const char *key1, const char *key2, int compare_mode) {
  if (compare_mode == COMPARE_CASE_INSENS)
    return !strcasecmp(key1, key2);
  else
    return !strcmp(key1, key2);
}
