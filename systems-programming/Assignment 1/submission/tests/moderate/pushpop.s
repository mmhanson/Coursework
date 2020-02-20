main:
	movl	$1, %eax
	movl	$2, %ebx
	movl	$3, %ecx
	pushl	%eax
	printr	%esp
	popl	%r8d
	printr	%r8d
	pushl	%ebx
	popl	%r8d
	printr	%r8d
	pushl	%eax
	pushl	%ebx
	pushl	%ecx
	popl	%r8d
	printr	%r8d
	popl	%r8d
	printr	%r8d
	popl	%r8d
	printr	%r8d
