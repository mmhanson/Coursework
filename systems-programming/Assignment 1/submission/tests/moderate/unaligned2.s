main:
	movl	$4660, %ebx
	printr	%ebx
	movl	%ebx, 1(%eax)
	movl	0(%eax), %eax
	printr	%eax
