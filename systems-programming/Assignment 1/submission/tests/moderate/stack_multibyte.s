main:
	subl	$4, %esp
	movl	$-21555, %ebx
	movl	%ebx, 0(%esp)
	movl	0(%esp), %ecx
	printr	%ecx
	pushl	%ebx
	popl	%edx
	printr	%edx
