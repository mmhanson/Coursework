main:
	movl	$2, %eax
	movl	$3, %ebx
	imull	%eax, %ebx
	printr	%ebx
