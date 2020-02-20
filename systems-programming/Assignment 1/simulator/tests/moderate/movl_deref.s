main:
	movl	$1012, %eax
	movl	$1, %ebx
	movl	$2, %ecx
	movl	$3, %edx
	movl	%ebx, 0(%eax)
	movl	%ecx, 4(%eax)
	movl	%edx, 8(%eax)
	printr	%eax
	movl	0(%eax), %r8d
	movl	4(%eax), %r9d
	movl	8(%eax), %r10d
	printr	%r8d
	printr	%r9d
	printr	%r10d
	movl	%ebx, -4(%eax)
	movl	%ecx, -8(%eax)
	movl	-4(%eax), %r9d
	movl	-8(%eax), %r10d
	printr	%r9d
	printr	%r10d
	ret
