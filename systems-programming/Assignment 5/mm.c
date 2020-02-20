/*
 * mm-naive.c - The least memory-efficient malloc package.
 * 
 * In this naive approach, a block is allocated by allocating a
 * new page as needed.  A block is pure payload. There are no headers or
 * footers.  Blocks are never coalesced or reused.
 *
 * NOTE TO STUDENTS: Replace this header comment with your own header
 * comment that gives a high level description of your solution.
 *
 * Written by Max Hanson (u0985911) for CS 4400, Fall 2019.
 */
#include <stdio.h>
#include <stdlib.h>
#include <assert.h>
#include <unistd.h>
#include <string.h>

#include "mm.h"
#include "memlib.h"

/* === Constant macros === */
#define PTR_SIZE (sizeof(void*))
/* All payload pointers are a multiple of this */
#define ALIGNMENT (16)
/* size of block headers/footers (note header_t may be shorter) */
#define HEADER_SIZE (ALIGNMENT)
/* size of header and footer */
#define BLOCK_OVERHEAD (2 * HEADER_SIZE)
/* size of the links at the start of each page, including any padding */
#define PAGE_LINKS_SIZE (ALIGN((2) * (PTR_SIZE)))
/* smallest page size allocatable (will allocate bigger if needed) */
//#define MIN_PAGE_SIZE ((10) * (mem_pagesize()))
#define INIT_PAGE_SIZE ((4) * (mem_pagesize()))
/* how large the largest page can be */
#define MAX_PAGE_SIZE (100 * INIT_PAGE_SIZE)
/* how many bytes needed for page links, prologue, and epilogue */
#define PAGE_OVERHEAD (PAGE_LINKS_SIZE + (3 * HEADER_SIZE))
/* blocks must be this big to include freelist links */
#define MIN_BLOCK_SIZE (BLOCK_OVERHEAD + (2 * PTR_SIZE))

/* === Functional macros === */
#define MAX(a, b) ((a) > (b) ? (a) : (b))
#define MIN(a, b) ((a) < (b) ? (a) : (b))
/* rounds up to the nearest multiple of ALIGNMENT */
#define ALIGN(size) (((size) + ((ALIGNMENT)-1)) & ~((ALIGNMENT)-1))
/* rounds up to the nearest multiple of mem_pagesize() */
#define PAGE_ALIGN(size) (((size) + (mem_pagesize()-1)) & ~(mem_pagesize()-1))
/* iterate the pagelist, head to tail */
#define FOR_EACH_PAGE(page_cursor) \
    for (page_cursor = head_page; \
         page_cursor != NULL; \
         page_cursor = get_next_page(page_cursor))
/* iterate on the blocklist of a page, prologue (including) to epilogue (excluding) */
#define FOR_EACH_BLOCK_IN_PAGE(page, block_cursor) \
    for (block_cursor = page + PAGE_LINKS_SIZE; \
         get_block_size((header_t*)block_cursor) != HEADER_SIZE; \
         block_cursor = ((void*)block_cursor) + get_block_size((header_t*)block_cursor))
/* iterate on freelist, starting at freelist_head */
#define FOR_EACH_BLOCK_IN_FREELIST(block_cursor) \
    for (block_cursor = freelist_head; \
         block_cursor != NULL && get_next_freeblock(block_cursor) != NULL; \
         block_cursor = (void*)get_next_freeblock(block_cursor))

typedef unsigned int header_t;

void* head_page; /* points to first page chunk in list of pages */
void* tail_page; // TODO remove
header_t* freelist_head;
size_t min_page_size;

/* Helper functions */
void* make_page(size_t min_freeblock_size);
void* attempt_reallocation(void* head_page, size_t new_block_size);
void* reallocate_block(header_t* free_block_pointer, size_t new_block_size);
void pack_header(header_t* header_ptr, size_t block_size, int alloc_flag);
void* put_free_block(header_t* start, size_t total_block_size);
void* put_block(header_t* header, size_t blocksize);
void link_freeblock(header_t* freeblock);
void unlink_freeblock(header_t* freeblock);
void unlink_freepage(void* page);
void link_page(void* page);
int is_page_free(void* page);
void unmap_containing_page_if_free(header_t* block);

/* Pointer arithmetic and bitwise operations functions */
void* get_prev_page(void* page);
void* get_next_page(void* page);
void set_prev_page(void* page, void* prev_page);
void set_next_page(void* page, void* next_page);
header_t* payload_ptr_to_header_ptr(void* payload_ptr);
header_t* header_ptr_to_footer_ptr(header_t* header_ptr);
size_t get_block_size(header_t* header_ptr);
int is_block_allocd(header_t* header_ptr);
header_t* get_next_freeblock(header_t* freeblock_hdrp);
header_t* get_prev_freeblock(header_t* freeblock_hdrp);
void set_next_freeblock(header_t* freeblock, header_t* next_freeblock);
void set_prev_freeblock(header_t* freeblock, header_t* prev_freeblock);
header_t* get_next_block(header_t* block);
header_t* get_prev_block(header_t* block);

/* Debugging heler functions */
void check_heap();
void print_heap_error(char* msg);

/* 
 * mm_init - initialize the malloc package.
 */
int mm_init(void)
{
    /* do some basic checks on the system */
    if (sizeof(header_t ) > HEADER_SIZE)
    {
        printf("SYSTEM ERROR! sizeof(header_t) > HEADER_SIZE. \n");
        return 1;
    }

    head_page = NULL;
    tail_page = NULL;
    freelist_head = NULL;
    min_page_size = INIT_PAGE_SIZE;

    make_page(min_page_size - PAGE_OVERHEAD); /* auto-inits head/tail page */
    if (head_page == NULL)
    {
        return 1;
    }

    //check_heap();
    return 0;
}

/* 
 * mm_malloc - Allocate a block by using bytes from current_page,
 *     grabbing a new page if necessary.
 */
void* mm_malloc(size_t size)
{
    size_t new_block_size;
    void* block_pointer;
    void* new_page;

    new_block_size = MAX(MIN_BLOCK_SIZE, ALIGN(size) + BLOCK_OVERHEAD);
    
    /* attempt to put block in the free spots of the heap */
    block_pointer = attempt_reallocation(head_page, new_block_size);
    if (block_pointer == NULL)
    {
        /* no free spot found, make new page (link at the end of pagelist) */
        new_page = make_page(new_block_size);
        if (new_page == NULL)
        {
            return NULL; /* page could not be made */
        }

        /* put the block in the new page */
        block_pointer = attempt_reallocation(new_page, new_block_size);
        if (block_pointer == NULL)
        {
            printf("MALLOC ERROR! New page not big enough for its block. \n");
            return NULL;
        }
    }
    
    //check_heap();
    return block_pointer;
}

/*
 * mm_free - Freeing a block does nothing.
 */
void mm_free(void* payload_ptr)
{
    header_t* header_ptr;
    header_t* footer_ptr;
    header_t* next_block;
    header_t* prev_block;
    size_t block_size;
    //void* containing_page;

    header_ptr = payload_ptr_to_header_ptr(payload_ptr);
    block_size = get_block_size(header_ptr);

    /* coalesce adjacent free blocks */
    next_block = get_next_block(header_ptr);
    prev_block = get_prev_block(header_ptr);
    if (!is_block_allocd(next_block))
    {
        unlink_freeblock(next_block);
        block_size += get_block_size(next_block);
    }
    if (!is_block_allocd(prev_block))
    {
        unlink_freeblock(prev_block);
        header_ptr = prev_block;
        block_size += get_block_size(prev_block);
    }

    /* add header/footer for new, coalesced block */
    pack_header(header_ptr, block_size, 0);
    footer_ptr = header_ptr_to_footer_ptr(header_ptr);
    pack_header(footer_ptr, block_size, 0);

    /* add to freelist */
    link_freeblock(header_ptr);

    /* unmap the containing page if it no longer has any allocated blocks */
    unmap_containing_page_if_free(header_ptr);

    //check_heap();
    return;
}

/*
 * =============================================================================
 * === Helper Functions                                                      ===
 * =============================================================================
 */

/*
 * Get a page, initialize it, and calculate metadata.
 * Page is linked after the tailpage (appended).
 *
 * @min_freeblock_size: the minimum size the pages freeblock must be.
 *   Automatically aligned.
 * @return: pointer to the first byte of the page, or null if the page could not
 *   be mapped.
 */
void* make_page(size_t min_freeblock_size)
{
    void* new_page;
    void* placeholder;
    size_t page_total_size;
    size_t free_block_size;

    page_total_size = MAX(ALIGN(min_freeblock_size) + PAGE_OVERHEAD, min_page_size);
    free_block_size = page_total_size - PAGE_OVERHEAD;
    min_page_size = MIN(2 * min_page_size, MAX_PAGE_SIZE);

    /* create a blank page */
    new_page = mem_map(page_total_size);
    if (new_page == NULL)
    {
        return NULL;
    }

    link_page(new_page);

    /* add a prologue */
    placeholder = new_page + PAGE_LINKS_SIZE;
    pack_header((header_t*)placeholder, 2 * HEADER_SIZE, 1);
    placeholder += HEADER_SIZE;
    pack_header((header_t*)placeholder, 2 * HEADER_SIZE, 1);
    placeholder += HEADER_SIZE;

    /* add a giant free block for all the allocatable space */
    placeholder = put_free_block((header_t*)placeholder, free_block_size);

    /* add an epilogue */
    pack_header((header_t*)placeholder, HEADER_SIZE, 1);

    return new_page;
}

/*
 * Attempt to allocate a block within the free blocks of the heap.
 *
 * @new_block_size: size in bytes of new block (including header/footer)
 * @return: pointer to the allocated block, or null if there was no space.
 */
void* attempt_reallocation(void* head_page, size_t new_block_size)
{
    void* block_cursor;
    //void* smallest_block;
    //size_t blocksize;

    block_cursor = NULL;
    FOR_EACH_BLOCK_IN_FREELIST(block_cursor)
    {
        if (get_block_size(block_cursor) >= new_block_size)
        {
            return reallocate_block(block_cursor, new_block_size);
        }
    }
    return NULL;

    /* put the block in the smallest, yet still large enough block */
    /* this strategy isnt used because it destroys throughput with tiny gains in utilization */
    /*
    smallest_block = NULL;
    FOR_EACH_BLOCK_IN_FREELIST(block_cursor)
    {
        blocksize = get_block_size((header_t*)block_cursor);

        // if there's no smallest block, or this block is smaller than it
        if ( (blocksize >= new_block_size) &&
             (smallest_block == NULL || blocksize < get_block_size(smallest_block)) )
        {
            smallest_block = block_cursor;
        }
    }

    if (smallest_block == NULL)
    {
        return NULL; // no space found
    }
    return reallocate_block(smallest_block, new_block_size);
    */
}

/*
 * Allocate a block using a free block.
 * CAUTION! Assumes that the size of the free block pointed to by
 * @free_block_pointer is >= to @new_block_size
 *
 * @free_block_pointer: pointer to start (header) of the free block.
 * @new_block_size: size in bytes of entire new block (including header/footer)
 * @return: pointer to new block payload
 */
void* reallocate_block(header_t* freeblock, size_t new_block_size)
{
    size_t freeblock_size;
    size_t remaining_blocksize;
    void* new_payload_ptr;
    header_t* remaining_freeblock;

    freeblock_size = get_block_size(freeblock);
    remaining_blocksize = freeblock_size - new_block_size;
    if (remaining_blocksize < MIN_BLOCK_SIZE)
    {
        /* only make a new block at the end if theres enough space */
        new_block_size = freeblock_size;
        remaining_blocksize = 0;
    }


    unlink_freeblock(freeblock);

    /* put the new block at the start of the free block */
    new_payload_ptr = put_block(freeblock, new_block_size);

    /* re-make the remainder of the free block if needed */
    if (remaining_blocksize > 0)
    {
        remaining_freeblock = ((void*)freeblock) + new_block_size;
        put_free_block(remaining_freeblock, remaining_blocksize);
    }


    return new_payload_ptr;
}

/*
 * Place an allocated block.
 *
 * @return: a pointer to the new payload.
 */
void* put_block(header_t* header, size_t blocksize)
{
    pack_header(header, blocksize, 1);
    pack_header(((void*)header) + blocksize - HEADER_SIZE, blocksize, 1);

    return (((void*)header) + HEADER_SIZE);
}

/*
 * Place a free block in a page.
 *
 * @header_ptr: the first byte of the free block.
 * @total_block_size: size in bytes of block including header/footer.
 * @return: byte just after the free block.
 */
void* put_free_block(header_t* header_ptr, size_t total_block_size)
{
    header_t* footer_ptr;

    pack_header(header_ptr, total_block_size, 0);
    /* must get footer_ptr after packing header */
    footer_ptr = header_ptr_to_footer_ptr(header_ptr);
    pack_header(footer_ptr, total_block_size, 0);

    link_freeblock(header_ptr);

    return ((void*)header_ptr + total_block_size);
}

/*
 * Prepend a freeblock to the freelist.
 */
void link_freeblock(header_t* header_ptr)
{
    /* prepend to freelist */
    if (freelist_head == NULL)
    {
        set_next_freeblock(header_ptr, NULL);
        set_prev_freeblock(header_ptr, NULL);

        freelist_head = header_ptr;
    }
    else
    {
        set_prev_freeblock(freelist_head, header_ptr);
        set_prev_freeblock(header_ptr, NULL);
        set_next_freeblock(header_ptr, freelist_head);

        freelist_head = header_ptr;
    }
}

/*
 * Unlink a freeblock from the freelist.
 */
void unlink_freeblock(header_t* freeblock)
{
    if (get_prev_freeblock(freeblock) != NULL)
    {
        set_next_freeblock(get_prev_freeblock(freeblock),
                           get_next_freeblock(freeblock));
    }
    if (get_next_freeblock(freeblock) != NULL)
    {
        set_prev_freeblock(get_next_freeblock(freeblock),
                           get_prev_freeblock(freeblock));
    }
    if (freeblock == freelist_head)
    {
        freelist_head = get_next_freeblock(freeblock);
    }
}

/*
 * Unlink a (presumably free) page from the pagelist.
 */
void unlink_freepage(void* page)
{
    if (get_prev_page(page) != NULL)
    {
        set_next_page(get_prev_page(page),
                      get_next_page(page));
    }
    if (get_next_page(page) != NULL)
    {
        set_prev_page(get_next_page(page),
                      get_prev_page(page));
    }
    if (page == head_page)
    {
        head_page = get_next_page(page);
    }
    if (page == tail_page)
    {
        tail_page = get_prev_page(page);
    }
}

/*
 * Append a page to the pagelist.
 */
void link_page(void* page)
{
    /* append the page the pagelist */
    if (tail_page == NULL)
    {
        /* this is the first page, init the pagelist */
        set_prev_page(page, NULL);
        set_next_page(page, NULL);
        head_page = page;
        tail_page = page;
    }
    else
    {
        set_next_page(tail_page, page);
        set_prev_page(page, tail_page);
        set_next_page(page, NULL);
        tail_page = page;
    }
}

/*
 * Determine if there are any allocated blocks in a page.
 *
 * @return: 0 if there are allocated blocks, 1 if there are not.
 */
int is_page_free(void* page)
{
    header_t* block_cursor;
    size_t blocksize;

    FOR_EACH_BLOCK_IN_PAGE(page, block_cursor)
    {
        blocksize = get_block_size(block_cursor);
        /* if block is allocd, and it is not the prologue or epilogue */
        if (blocksize != HEADER_SIZE &&
            blocksize != (2 * HEADER_SIZE) &&
            is_block_allocd(block_cursor))
        {
            return 0;
        }
    }
    return 1;
}

/*
 * If the page has no allocated blocks in it, then unlink the freeblock(s) in a
 * page, unlink the page, and unmap the page.
 *
 * This is in its own helper so it can shortcircuit the process and just return
 * if it finds an allocated block in the process of iterating backwards to the
 * start of the page from the given block. It'd be more readable to split the
 * logic into their own helpers, but this would not allow the shortcircuits.
 */
void unmap_containing_page_if_free(header_t* block)
{
    void* placeholder;
    header_t* block_cursor;
    size_t blocksize;
    size_t size_so_far;

    /* put placeholder at the page prologue by iterating backwards */
    placeholder = (void*)block;
    while (get_block_size(placeholder) != (2 * HEADER_SIZE))
    {
        if (is_block_allocd(placeholder))
        {
            /* shortcicruit the whole thing if an alloc'd block is seen */
            return;
        }

        placeholder = get_prev_block(placeholder);
    }

    /* move from prologue to start of page */
    placeholder = placeholder - PAGE_LINKS_SIZE;

    /* look through whole page for free ptrs and calculate page size */
    size_so_far = PAGE_LINKS_SIZE;
    FOR_EACH_BLOCK_IN_PAGE(placeholder, block_cursor)
    {
        blocksize = get_block_size(block_cursor);
        size_so_far += blocksize;
        /* if block is allocd, and it is not the prologue or epilogue */
        if (blocksize != HEADER_SIZE &&
            blocksize != (2 * HEADER_SIZE) &&
            is_block_allocd(block_cursor))
        {
            /* shortcicruit, found allocd block */
            return;
        }
    }
    /* macro above doesnt include epilogue */
    size_so_far += HEADER_SIZE;

    /* unlink and unmap the page, by this point the page is entirely free */
    unlink_freeblock(block);
    unlink_freepage(placeholder);
    mem_unmap(placeholder, size_so_far);
}

/*
 * Pack metadata into a header.
 *
 * @block_size: the size of the entire block in bytes.
 * @alloc_flag: 0 if this block is free, 1 if it is allocated.
 */
void pack_header(header_t* header_ptr, size_t block_size, int alloc_flag)
{
    *header_ptr = (block_size) | (alloc_flag);
}

/*
 * =============================================================================
 * === Pointer Arithmetic and Bitwise Operation Helper Functions             ===
 * =============================================================================
 */

/*
 * Get a pointer to the previous page.
 */
void* get_prev_page(void* page)
{
    return (*((void**)page));
}

/*
 * Get a pointer to the next page.
 */
void* get_next_page(void* page)
{
    return (*((void**)(page + PTR_SIZE)));
}

/*
 * Set the previous page of a page.
 */
void set_prev_page(void* page, void* prev_page)
{
    *((void**)page) = prev_page;
}

/*
 * Set the next page of a page.
 */
void set_next_page(void* page, void* next_page)
{
    *((void**)(page + PTR_SIZE)) = next_page;
}

/*
 * Get the size of a header.
 */
size_t get_block_size(header_t* header_ptr)
{
    return ((*header_ptr) & ~0x7);
}

/*
 * Get the alloc flag of a header.
 */
int is_block_allocd(header_t* header_ptr)
{
    return ((*header_ptr) & 0x1);
}

/*
 * Convert a payload pointer to a pointer to that payload's header.
 */
header_t* payload_ptr_to_header_ptr(void* payload_ptr)
{
    return (header_t*)(payload_ptr - HEADER_SIZE);
}

/*
 * Convert a pointer to a header to a pointer to a footer.
 */
header_t* header_ptr_to_footer_ptr(header_t* header_ptr)
{
    void* placeholder;
    size_t block_size;

    block_size = get_block_size(header_ptr);
    placeholder = (void*)header_ptr;
    placeholder = placeholder + block_size - HEADER_SIZE;

    return (header_t*)placeholder;
}

/*
 * Get the next freeblock in the freelist.
 *
 * @freeblock_hdrp: header of the current freeblock.
 * @return: header of the next freeblock, or null.
 */
header_t* get_next_freeblock(header_t* freeblock)
{
    /* 'next' ptr is put right after header, 'prev' ptr is put right after it */
    return *((void**)((void*)freeblock + PTR_SIZE));
}

/*
 * Get the previous freeblock in the freelist.
 *
 * @freeblock: header of the current freeblock.
 * @return: header of the previous freeblock, or null.
 */
header_t* get_prev_freeblock(header_t* freeblock)
{
    /* 'prev' ptr is put right after the 'next' ptr*/
    return *((void**)((void*)freeblock + (2 * PTR_SIZE)));
}

/*
 * Set the next freeblock of a freeblock.
 *
 * @next_freeblock: NEXT(@freeblock) will point to this.
 */
void set_next_freeblock(header_t* freeblock, header_t* next_freeblock)
{
    *((header_t**)((void*)freeblock + PTR_SIZE)) = next_freeblock;
}

/*
 * Set the prev freeblock of a freeblock.
 *
 * @prev_freeblock: PREV(@freeblock) will point to this.
 */
void set_prev_freeblock(header_t* freeblock, header_t* prev_freeblock)
{
    *((header_t**)((void*)freeblock + (2 * PTR_SIZE))) = prev_freeblock;
}

/*
 * Get a pointer to the header of the next block.
 */
header_t* get_next_block(header_t* block)
{
    return (((void*)block) + get_block_size(block));
}

/*
 * Get a pointer to the header of the previous block.
 */
header_t* get_prev_block(header_t* block)
{
    size_t prev_blocksize;

    prev_blocksize = get_block_size(((void*)block) - HEADER_SIZE);
    return (((void*)block) - prev_blocksize);
}

/*
 * =============================================================================
 * === Debugging Helper Functions                                            ===
 * =============================================================================
 */

/*
 * Check the validity of the heap.
 * Checks:
 *  - for each page: the prologue leads to the epilogue
 *  - for each page: NEXT(PREV(p)) = p, and PREV(NEXT(p)) = p
 *  - the head page leads to the tail page
 *  - PREV(head page) = null, and NEXT(tail page) = null
 *  - that there are no adjacent free blocks
 */
void check_heap()
{
    void* page_cursor;
    void* placeholder;
    void* block_cursor;
    size_t block_size;
    int is_last_block_allocd;
    header_t* prev_freeblock;
    header_t* next_freeblock;

    if (get_prev_page(head_page) != NULL)
    {
        print_heap_error("PREV(head) != null");
    }
    if (get_next_page(tail_page) != NULL)
    {
        print_heap_error("NEXT(tail) != null");
    }

    FOR_EACH_PAGE(page_cursor)
    {
        /* check the prologue */
        block_cursor = page_cursor + PAGE_LINKS_SIZE;
        block_size = get_block_size((header_t*)block_cursor);
        if (block_size != (2 * HEADER_SIZE))
        {
            print_heap_error("Prologue is incorrect size");
        }

        /* check the blocklist */
        is_last_block_allocd = 1;
        FOR_EACH_BLOCK_IN_PAGE(page_cursor, block_cursor)
        {
            if (get_block_size(block_cursor) == 0)
            {
                print_heap_error("Blocksize of zero.");
            }

            if (!is_last_block_allocd && !is_block_allocd(block_cursor))
            {
                print_heap_error("Two adjacent free blocks found.");
            }
            is_last_block_allocd = is_block_allocd(block_cursor);
        }

        /* check the pagelinks (PREV(NEXT(p)) = p and NEXT(PREV(p)) = p) */
        if (page_cursor != head_page)
        {
            placeholder = get_prev_page(page_cursor);
            placeholder = get_next_page(placeholder);
            if (placeholder != page_cursor)
            {
                print_heap_error("NEXT(PREV(p)) != p");
            }
        }
        if (page_cursor != tail_page)
        {
            placeholder = get_next_page(page_cursor);
            placeholder = get_prev_page(placeholder);
            if (placeholder != page_cursor)
            {
                print_heap_error("PREV(NEXT(p)) != p");
            }
        }

        /* check that the page isnt free, freepages should be unmapped */
        /* note this discludes initial pages */
        if (is_page_free(page_cursor) && page_cursor != head_page && page_cursor != tail_page)
        {
            print_heap_error("Found freepage in the pagelist");
        }
    }

    FOR_EACH_BLOCK_IN_FREELIST(block_cursor)
    {
        if (block_cursor == freelist_head &&
            get_prev_freeblock(freelist_head) != NULL)
        {
            print_heap_error("PREV(freelist_head) != null.");
        }

        if (is_block_allocd(block_cursor))
        {
            print_heap_error("Allocated block in freelist");
        }

        prev_freeblock = get_prev_freeblock(block_cursor);
        if (prev_freeblock != NULL &&
            get_next_freeblock(prev_freeblock) != block_cursor)
        {
            print_heap_error("NEXT(PREV(freeblock)) != freeblock");
        }
        next_freeblock = get_next_freeblock(block_cursor);
        if (next_freeblock != NULL &&
            get_prev_freeblock(next_freeblock) != block_cursor)
        {
            print_heap_error("PREV(NEXT(freeblock)) != freeblock");
        }
    }
}

/*
 * Print an error and terminate the program.
 */
void print_heap_error(char* msg)
{
    printf("HEAP ERROR! %s. \n", msg);
    exit(1);
}
