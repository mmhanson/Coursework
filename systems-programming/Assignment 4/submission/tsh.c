/* 
 * tsh - A tiny shell program with job control
 * 
 * Max Hanson - u0985911
 */
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <ctype.h>
#include <signal.h>
#include <sys/types.h>
#include <sys/wait.h>
#include <errno.h>

/* 
 * csapp.c - Functions for the CS:APP3e book
 *
 * Updated 10/2016 reb:
 *   - Fixed bug in sio_ltoa that didn't cover negative numbers
 *
 * Updated 2/2016 droh:
 *   - Updated open_clientfd and open_listenfd to fail more gracefully
 *
 * Updated 8/2014 droh: 
 *   - New versions of open_clientfd and open_listenfd are reentrant and
 *     protocol independent.
 *
 *   - Added protocol-independent inet_ntop and inet_pton functions. The
 *     inet_ntoa and inet_aton functions are obsolete.
 *
 * Updated 7/2014 droh:
 *   - Aded reentrant sio (signal-safe I/O) routines
 * 
 * Updated 4/2013 droh: 
 *   - rio_readlineb: fixed edge case bug
 *   - rio_readnb: removed redundant EINTR check
 */

ssize_t sio_puts(char s[]);
ssize_t sio_putl(long v);
static size_t sio_strlen(char s[]);
static void sio_ltoa(long v, char s[], int b);
static void sio_reverse(char s[]);

/* Misc manifest constants */
#define MAXLINE    1024   /* max line size */
#define MAXARGS     128   /* max args on a command line */
#define MAXJOBS      16   /* max jobs at any point in time */
#define MAXJID    1<<16   /* max job ID */

/* Job states */
#define UNDEF 0 /* undefined */
#define FG 1    /* running in foreground */
#define BG 2    /* running in background */
#define ST 3    /* stopped */

/* 
 * Jobs states: FG (foreground), BG (background), ST (stopped)
 * Job state transitions and enabling actions:
 *     FG -> ST  : ctrl-z
 *     ST -> FG  : fg command
 *     ST -> BG  : bg command
 *     BG -> FG  : fg command
 * At most 1 job can be in the FG state.
 */

/* Global variables */
extern char **environ;      /* defined in libc */
char prompt[] = "tsh> ";    /* command line prompt (DO NOT CHANGE) */
int verbose = 0;            /* if true, print additional output */
int nextjid = 1;            /* next job ID to allocate */
char sbuf[MAXLINE];         /* for composing sprintf messages */

struct job_t {              /* The job struct */
  pid_t pid;              /* job PID */
  int jid;                /* job ID [1, 2, ...] */
  int state;              /* UNDEF, BG, FG, or ST */
  char cmdline[MAXLINE];  /* command line */
};
struct job_t jobs[MAXJOBS]; /* The job list. Note: is modified from some signal handlers */
/* End global variables */


/* Function prototypes */

/* Here are the functions that you will implement */
void eval(char *cmdline);
int builtin_cmd(char **argv);
void do_bg(int jid);
void do_fg(int jid);
void waitfg(pid_t pid);

void sigchld_handler(int sig);
void sigtstp_handler(int sig);
void sigint_handler(int sig);

/* Here are helper routines that we've provided for you */
int parseline(const char *cmdline, char **argv, int cmdnum); 
void sigquit_handler(int sig);

void clearjob(struct job_t *job);
void initjobs(struct job_t *jobs);
int maxjid(struct job_t *jobs); 
int addjob(struct job_t *jobs, pid_t pid, int state, char *cmdline);
int deletejob(struct job_t *jobs, pid_t pid); 
struct job_t *getjobpid(struct job_t *jobs, pid_t pid);
struct job_t *getjobjid(struct job_t *jobs, int jid); 
int pid2jid(pid_t pid); 
void listjobs(struct job_t *jobs);

void usage(void);
void unix_error(char *msg);
void app_error(char *msg);
typedef void handler_t(int);
handler_t *Signal(int signum, handler_t *handler);

/* Helper functions that I added. */
struct job_t *find_foreground_job(struct job_t *jobs);

/*
 * main - The shell's main routine 
 */
int main(int argc, char **argv) 
{
  char c;
  char cmdline[MAXLINE];
  int emit_prompt = 1; /* emit prompt (default) */

  /* Redirect stderr to stdout (so that driver will get all output
   * on the pipe connected to stdout) */
  dup2(1, 2);

  /* Parse the command line */
  while ((c = getopt(argc, argv, "hvp")) != EOF) {
    switch (c) {
    case 'h':             /* print help message */
      usage();
      break;
    case 'v':             /* emit additional diagnostic info */
      verbose = 1;
      break;
    case 'p':             /* don't print a prompt */
      emit_prompt = 0;  /* handy for automatic testing */
      break;
    default:
      usage();
    }
  }

  /* Install the signal handlers */

  /* These are the ones you will need to implement */
  Signal(SIGINT,  sigint_handler);   /* ctrl-c */
  Signal(SIGTSTP, sigtstp_handler);  /* ctrl-z */
  Signal(SIGCHLD, sigchld_handler);  /* Terminated or stopped child */

  /* This one provides a clean way to kill the shell */
  Signal(SIGQUIT, sigquit_handler); 

  /* Initialize the job list */
  initjobs(jobs);

  /* Execute the shell's read/eval loop */
  while (1) {

    /* Read command line */
    if (emit_prompt) {
      printf("%s", prompt);
      fflush(stdout);
    }
    if ((fgets(cmdline, MAXLINE, stdin) == NULL) && ferror(stdin))
      app_error("fgets error");
    if (feof(stdin)) { /* End of file (ctrl-d) */
      fflush(stdout);
      exit(0);
    }

    /* Evaluate the command line */
    eval(cmdline);

    fflush(stdout);
    fflush(stdout);
  } 

  exit(0); /* control never reaches here */
}
  
/* 
 * eval - Evaluate the command line that the user has just typed in
 * 
 * If the user has requested a built-in command (quit, jobs, bg or fg)
 * then execute it immediately. Otherwise, fork a child process and
 * run the job in the context of the child. If the job is running in
 * the foreground, wait for it to terminate and then return.  Note:
 * each child process must have a unique process group ID so that our
 * background children don't receive SIGINT (SIGTSTP) from the kernel
 * when we type ctrl-c (ctrl-z) at the keyboard.  
 */
void eval(char *cmdline) 
{
    char *argv1[MAXARGS]; /* argv for execve() */
    char *argv2[MAXARGS]; /* argv for second command execve() */
    char *cmd2;
    pid_t child_pid;
    sigset_t signal_set;
    int bg;               /* should the job run in bg or fg? */
    int pipeline[2];
    
    /* If the line contains two commands, split into two strings */
    cmd2 = strchr(cmdline, '|');
    
    if (cmd2 != NULL && strlen(cmd2) >= 3 && (cmd2 - cmdline) >= 2)
    {
        // Terminate the first command with newline and null character
        cmd2--;
        cmd2[0] = '\n';
        cmd2[1] = '\0';
        // Set the second command to start after the next space
        cmd2 += 3;
    }
    
    /* Parse command line */
    bg = parseline(cmdline, argv1, 1); // note: bg is true if cmd is background
    if (argv1[0] == NULL)  
    {
        return;   /* ignore empty lines */
    }
    
    // pipe cmd1 to cmd2 if cmd2 is non-null
    if(cmd2 != NULL)
    {
        parseline(cmd2, argv2, 2);

        if (pipe(pipeline) == -1)
        {
            app_error("Error creating a pipeline.\n");
        }

        if (fork() == 0)
        {
            // first child, writes to the pipe
            close(pipeline[0]);
            dup2(pipeline[1], 1); // redirect stdout to write-end of new pipe

            if (execve(argv1[0], argv1, environ) == -1)
            {
                exit(1);
            }
        }
        if (fork() == 0)
        {
            // second child, reads from the pipe
            close(pipeline[1]);
            dup2(pipeline[0], 0); // redirect stdin to read-end of new pipe
            
            if (execve(argv2[0], argv2, environ) == -1)
            {
                exit(1);
            }
        }

        close(pipeline[0]);
        close(pipeline[1]);
        wait(NULL);
        wait(NULL);

        return;
    }
    
    // if command is built-in, then execute it and return
    if (builtin_cmd(argv1) != 0)
    {
        return;
    }

    // if command is an executable path, then create a job for it
    // note: blocking/unblocking of SIGCHLD, SIGINT, SIGTSTP is to avoid race
    //       condition with 'jobs' with those signal's handlers
    sigemptyset(&signal_set);
    sigaddset(&signal_set, SIGCHLD);
    sigaddset(&signal_set, SIGINT);
    sigaddset(&signal_set, SIGTSTP);
    sigprocmask(SIG_BLOCK, &signal_set, NULL); // block SIGCHLD, SIGINT, SIGTSTP

    child_pid = fork();
    if (child_pid == 0)
    {
        setpgid(0, 0); // put self in a new group
        sigprocmask(SIG_UNBLOCK, &signal_set, NULL); // unblock SIGCHLD, SIGINT, SIGTSTP
        if (execve(argv1[0], argv1, environ) == -1)
        {
            // app_error is hard to use with formatted output
            printf("%s: Command not found\n", argv1[0]);
            exit(1);
        }
    }

    if (bg)
    {
        addjob(jobs, child_pid, BG, cmdline);
        printf("[%d] (%d) %s", pid2jid(child_pid), child_pid, cmdline);
        fflush(stdout);
        sigprocmask(SIG_UNBLOCK, &signal_set, NULL); // unblock SIGCHLD, SIGINT, SIGTSTP

        return;
    }
    else
    {
        addjob(jobs, child_pid, FG, cmdline);
        sigprocmask(SIG_UNBLOCK, &signal_set, NULL); // unblock SIGCHLD, SIGINT, SIGTSTP
        waitfg(child_pid);
    }
        
    return;
}

/* 
 * parseline - Parse the command line and build the argv array.
 * 
 * Characters enclosed in single quotes are treated as a single
 * argument.  Return true if the user has requested a BG job, false if
 * the user has requested a FG job.  
 */
int parseline(const char *cmdline, char **argv, int cmdnum) 
{
  static char array1[MAXLINE]; /* holds local copy of command line */
  static char array2[MAXLINE]; /* holds local copy of 2nd command line */
  char *buf;                   /* ptr that traverses command line */
  char *delim;                 /* points to first space delimiter */
  int argc;                    /* number of args */
  int bg;                      /* background job? */
  
  if(cmdnum == 1)
    buf = array1;
  else
    buf = array2;
  
  strcpy(buf, cmdline);
  
  buf[strlen(buf)-1] = ' ';  /* replace trailing '\n' with space */
  while (*buf && (*buf == ' ')) /* ignore leading spaces */
    buf++;
  
  /* Build the argv list */
  argc = 0;
  if (*buf == '\'') {
    buf++;
    delim = strchr(buf, '\'');
  }
  else {
    delim = strchr(buf, ' ');
  }
  
  while (delim) {
    argv[argc++] = buf;
    *delim = '\0';
    buf = delim + 1;
    while (*buf && (*buf == ' ')) /* ignore spaces */
      buf++;
    
    if (*buf == '\'') {
      buf++;
      delim = strchr(buf, '\'');
    }
    else {
      delim = strchr(buf, ' ');
    }
  }
  argv[argc] = NULL;
  
  if (argc == 0)  /* ignore blank line */
    return 1;
  
  /* should the job run in the background? */
  if ((bg = (*argv[argc-1] == '&')) != 0) {
    argv[--argc] = NULL;
  }
  
  
  return bg;
}

/* 
 * builtin_cmd - If the user has typed a built-in command then execute
 *    it immediately.  
 */
int builtin_cmd(char **argv) 
{
  char *cmd = argv[0];

  if (!strcmp(cmd, "quit"))
  {
      exit(0);
  }
  
  if (!strcmp(cmd, "jobs"))
  {
      listjobs(jobs);
      fflush(stdout);

      return 1;
  }

  if (!strcmp(cmd, "bg") || !strcmp(cmd, "fg")) { /* bg and fg commands */
      
    int jid;

    /* Ignore command if no argument */
    if (argv[1] == NULL) {
      printf("%s command requires a %%jobid argument\n", argv[0]);
      return 1;
    }

    if (argv[1][0] == '%') {
      jid = atoi(&argv[1][1]);
    }
    else {
      printf("%s: argument must be a %%jobid\n", argv[0]);
      return 1;
    }
      
    if(!strcmp(cmd, "bg"))
      do_bg(jid);
    else
      do_fg(jid);
    return 1;
  }

  if (!strcmp(cmd, "&")) { /* Ignore singleton & */
    return 1;
  }

  return 0;     /* not a builtin command */
}

/* 
 * do_bg - Execute the builtin bg command
 */
void do_bg(int jid) 
{
    struct job_t *job;

    // send SIGCONT to the job's group and mark process as 'BG'
    job = getjobjid(jobs, jid);
    if (job == NULL)
    {
        sio_puts("%");
        sio_putl(jid);
        sio_puts(": No such job\n");
        return;
    }

    sio_puts("[");
    sio_putl(job->jid);
    sio_puts("] (");
    sio_putl(job->pid);
    sio_puts(") ");
    sio_puts(job->cmdline);
    
    kill(-1 * job->pid, SIGCONT);
    job->state = BG;

    return;
}

/* 
 * do_fg - Execute the builtin fg command
 */
void do_fg(int jid) 
{
    struct job_t *job;

    job = getjobjid(jobs, jid);
    if (job == NULL)
    {
        sio_puts("%");
        sio_putl(jid);
        sio_puts(": No such job\n");
        return;
    }

    if (job->state == ST)
    {
        kill(-1 * job->pid, SIGCONT);
    }
    job->state = FG;
    waitfg(job->pid);

    return;
}

/* 
 * waitfg - Block until process pid is no longer the foreground process
 */
void waitfg(pid_t pid)
{
    struct job_t *job;

    // wait until the foreground job is not in the foreground anymore
    job = find_foreground_job(jobs);
    while (job->state == FG)
    {
        sleep(1);
    }

    return;
}

/*****************
 * Signal handlers
 *****************/

/* 
 * sigchld_handler - The kernel sends a SIGCHLD to the shell whenever
 *     a child job terminates (becomes a zombie), or stops because it
 *     received a SIGSTOP or SIGTSTP signal. The handler reaps all
 *     available zombie children, but doesn't wait for any other
 *     currently running children to terminate.  
 */
void sigchld_handler(int sig) 
{
    pid_t reaped_pid;
    int old_errno;
    sigset_t signal_set;
    int wstatus;
    struct job_t *job;

    old_errno = errno;
    sigemptyset(&signal_set);
    sigaddset(&signal_set, SIGCHLD);
    sigaddset(&signal_set, SIGINT);
    sigaddset(&signal_set, SIGTSTP);
    sigprocmask(SIG_BLOCK, &signal_set, NULL); // block SIGCHLD, SIGINT, SIGTSTP

    // reap all available zombies
    while ((reaped_pid = waitpid(-1, &wstatus, WNOHANG | WUNTRACED)) > 0)
    {
        if (WIFSTOPPED(wstatus))
        {
            // mark stopped jobs as 'stopped' so shell can continue
            job = getjobpid(jobs, reaped_pid);
            sio_puts("Job [");
            sio_putl(job->jid);
            sio_puts("] (");
            sio_putl(job->pid);
            sio_puts(") stopped by signal ");
            sio_putl(WSTOPSIG(wstatus));
            sio_puts("\n");

            job->state = ST;
        }
        if (WIFSIGNALED(wstatus))
        {
            // delete jobs terminated by signal from the job list
            job = getjobpid(jobs, reaped_pid);
            sio_puts("Job [");
            sio_putl(job->jid);
            sio_puts("] (");
            sio_putl(job->pid);
            sio_puts(") terminated by signal ");
            sio_putl(WTERMSIG(wstatus));
            sio_puts("\n");

            deletejob(jobs, reaped_pid);
        }
        if (WIFEXITED(wstatus))
        {
            // delete jobs terminated normally from the job list
            deletejob(jobs, reaped_pid);
        }
    }

    errno = old_errno;
    sigprocmask(SIG_UNBLOCK, &signal_set, NULL); // unblock SIGCHLD, SIGINT, SIGTSTP

    return;
}

/* 
 * sigint_handler - The kernel sends a SIGINT to the shell whenver the
 *    user types ctrl-c at the keyboard.  Catch it and send it along
 *    to the foreground job.  
 */
void sigint_handler(int sig) 
{
    sigset_t signal_set;
    struct job_t *job;

    sigemptyset(&signal_set);
    sigaddset(&signal_set, SIGCHLD);
    sigaddset(&signal_set, SIGINT);
    sigaddset(&signal_set, SIGTSTP);
    sigprocmask(SIG_BLOCK, &signal_set, NULL); // block SIGCHLD, SIGINT, SIGTSTP

    // find first (and therefore only) foreground job in the jobs list
    job = find_foreground_job(jobs);
    kill(-1 * job->pid, sig); // forward sig to jobs entire group
    
    sigprocmask(SIG_UNBLOCK, &signal_set, NULL); // unblock SIGCHLD, SIGINT, SIGTSTP

    return;
}

/*
 * sigtstp_handler - The kernel sends a SIGTSTP to the shell whenever
 *     the user types ctrl-z at the keyboard. Catch it and suspend the
 *     foreground job by sending a SIGTSTP.  
 */
void sigtstp_handler(int sig) 
{
    sigset_t signal_set;
    struct job_t *job;

    sigemptyset(&signal_set);
    sigaddset(&signal_set, SIGCHLD);
    sigaddset(&signal_set, SIGINT);
    sigaddset(&signal_set, SIGTSTP);
    sigprocmask(SIG_BLOCK, &signal_set, NULL); // block SIGCHLD, SIGINT, SIGTSTP

    // find first (and therefore only) foreground job in the jobs list
    job = find_foreground_job(jobs);
    kill(-1 * job->pid, sig); // forward sig to jobs entire group

    sigprocmask(SIG_UNBLOCK, &signal_set, NULL); // unblock SIGCHLD, SIGINT, SIGTSTP

    return;
}

/*********************
 * End signal handlers
 *********************/

/***********************************************
 * Helper routines that manipulate the job list
 **********************************************/

/* clearjob - Clear the entries in a job struct */
void clearjob(struct job_t *job) {
  job->pid = 0;
  job->jid = 0;
  job->state = UNDEF;
  job->cmdline[0] = '\0';
}

/* initjobs - Initialize the job list */
void initjobs(struct job_t *jobs) {
  int i;

  for (i = 0; i < MAXJOBS; i++)
    clearjob(&jobs[i]);
}

/* maxjid - Returns largest allocated job ID */
int maxjid(struct job_t *jobs) 
{
  int i, max=0;

  for (i = 0; i < MAXJOBS; i++)
    if (jobs[i].jid > max)
      max = jobs[i].jid;
  return max;
}

/* addjob - Add a job to the job list */
int addjob(struct job_t *jobs, pid_t pid, int state, char *cmdline) 
{
    int i;
    
    if (pid < 1)
    {
        return 0;
    }
    
    for (i = 0; i < MAXJOBS; i++)
    {
        if (jobs[i].pid == 0)
        {
            jobs[i].pid = pid;
            jobs[i].state = state;
            jobs[i].jid = nextjid++;
            if (nextjid > MAXJOBS)
            {
                nextjid = 1;
            }
            strcpy(jobs[i].cmdline, cmdline);
            if(verbose)
            {
                printf("Added job [%d] %d %s\n", jobs[i].jid, jobs[i].pid, jobs[i].cmdline);
            }
            return 1;
        }
    }
    printf("Tried to create too many jobs\n");
    return 0;
}

/* deletejob - Delete a job whose PID=pid from the job list */
int deletejob(struct job_t *jobs, pid_t pid) 
{
  int i;

  if (pid < 1)
    return 0;

  for (i = 0; i < MAXJOBS; i++) {
    if (jobs[i].pid == pid) {
      clearjob(&jobs[i]);
      nextjid = maxjid(jobs)+1;
      return 1;
    }
  }
  return 0;
}


/* getjobpid  - Find a job (by PID) on the job list */
struct job_t *getjobpid(struct job_t *jobs, pid_t pid) {
  int i;

  if (pid < 1)
    return NULL;
  for (i = 0; i < MAXJOBS; i++)
    if (jobs[i].pid == pid)
      return &jobs[i];
  return NULL;
}

/* getjobjid  - Find a job (by JID) on the job list */
struct job_t *getjobjid(struct job_t *jobs, int jid) 
{
  int i;

  if (jid < 1)
    return NULL;
  for (i = 0; i < MAXJOBS; i++)
    if (jobs[i].jid == jid)
      return &jobs[i];
  return NULL;
}

/* pid2jid - Map process ID to job ID */
int pid2jid(pid_t pid) 
{
  int i;

  if (pid < 1)
    return 0;
  for (i = 0; i < MAXJOBS; i++)
    if (jobs[i].pid == pid) {
      return jobs[i].jid;
    }
  return 0;
}

/* listjobs - Print the job list */
void listjobs(struct job_t *jobs) 
{
  int i;

  for (i = 0; i < MAXJOBS; i++) {
    if (jobs[i].pid != 0) {
      printf("[%d] (%d) ", jobs[i].jid, jobs[i].pid);
      switch (jobs[i].state) {
      case BG: 
	printf("Running ");
	break;
      case FG: 
	printf("Foreground ");
	break;
      case ST: 
	printf("Stopped ");
	break;
      default:
	printf("listjobs: Internal error: job[%d].state=%d ", 
	       i, jobs[i].state);
      }
      printf("%s", jobs[i].cmdline);
    }
  }
}

/*
 * Find the first [and hopefully only] foreground job.
 * Returns NULL if no foreground job is found.
 */
struct job_t *find_foreground_job(struct job_t *jobs)
{
    int i;

    for (i = 0; i < MAXJOBS; i++)
    {
        if (jobs[i].pid != 0)
        {
            if (jobs[i].state == FG)
            {
                return &jobs[i];
            }
        }
    }

    return NULL;
}
/******************************
 * end job list helper routines
 ******************************/


/***********************
 * Other helper routines
 ***********************/

/*
 * usage - print a help message
 */
void usage(void) 
{
  printf("Usage: shell [-hvp]\n");
  printf("   -h   print this message\n");
  printf("   -v   print additional diagnostic information\n");
  printf("   -p   do not emit a command prompt\n");
  exit(1);
}

/*
 * unix_error - unix-style error routine
 */
void unix_error(char *msg)
{
  fprintf(stdout, "%s: %s\n", msg, strerror(errno));
  exit(1);
}

/*
 * app_error - application-style error routine
 */
void app_error(char *msg)
{
  fprintf(stdout, "%s\n", msg);
  exit(1);
}

/*
 * Signal - wrapper for the sigaction function
 */
handler_t *Signal(int signum, handler_t *handler) 
{
  struct sigaction action, old_action;

  action.sa_handler = handler;  
  sigemptyset(&action.sa_mask); /* block sigs of type being handled */
  action.sa_flags = SA_RESTART; /* restart syscalls if possible */

  if (sigaction(signum, &action, &old_action) < 0)
    unix_error("Signal error");
  return (old_action.sa_handler);
}

/*
 * sigquit_handler - The driver program can gracefully terminate the
 *    child shell by sending it a SIGQUIT signal.
 */
void sigquit_handler(int sig) 
{
  sio_puts("Terminating after receipt of SIGQUIT signal\n");
  exit(1);
}


/* Put string */
ssize_t sio_puts(char s[])
{
  return write(STDOUT_FILENO, s, sio_strlen(s));
}


/* Put long */
ssize_t sio_putl(long v)
{
  char s[128];
  sio_ltoa(v, s, 10); /* Based on K&R itoa() */
  return sio_puts(s);
}


/* sio_strlen - Return length of string (from K&R) */
static size_t sio_strlen(char s[])
{
  int i = 0;
  while (s[i] != '\0')
    ++i;
  return i;
}


/* sio_ltoa - Convert long to base b string (from K&R) */
static void sio_ltoa(long v, char s[], int b)
{
  int c, i = 0;
  int neg = v < 0;

  if (neg)
    v = -v;

  do {
    s[i++] = ((c = (v % b)) < 10)  ?  c + '0' : c - 10 + 'a';
  } while ((v /= b) > 0);

  if (neg)
    s[i++] = '-';

  s[i] = '\0';
  sio_reverse(s);
}

/* sio_reverse - Reverse a string (from K&R) */
static void sio_reverse(char s[])
{
  int c, i, j;

  for (i = 0, j = strlen(s)-1; i < j; i++, j--) {
    c = s[i];
    s[i] = s[j];
    s[j] = c;
  }
}
