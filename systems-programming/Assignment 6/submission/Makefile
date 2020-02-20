
FRIENDLIST_C = friendlist.c
CFLAGS = -O2 -g -Wall -I.
#CFLAGS = -g -Wall -I.

friendlist: $(FRIENDLIST_C) dictionary.c dictionary.h csapp.c csapp.h more_string.c more_string.h
	$(CC) $(CFLAGS) -o friendlist $(FRIENDLIST_C) dictionary.c more_string.c csapp.c -pthread

clean:
	rm friendlist
