main:
	movl	$1012, %eax
	movl	$1012, %ebx
	movl	$555, %ecx
	movl	%ecx, 0(%eax)
	movl	0(%ebx), %edx
	printr	%edx
	ret
