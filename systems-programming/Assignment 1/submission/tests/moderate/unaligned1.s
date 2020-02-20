main:
	movl	$4660, %ebx
	printr	%ebx
	movl	%ebx, 0(%eax)
	movl	1(%eax), %eax
	printr	%eax
