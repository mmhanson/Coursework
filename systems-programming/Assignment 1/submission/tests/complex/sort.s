main:
	subl	$32, %esp
	readr	%r8d
	movl	%r8d, 0(%esp)
	readr	%r8d
	movl	%r8d, 4(%esp)
	readr	%r8d
	movl	%r8d, 8(%esp)
	readr	%r8d
	movl	%r8d, 12(%esp)
	readr	%r8d
	movl	%r8d, 16(%esp)
	readr	%r8d
	movl	%r8d, 20(%esp)
	movl	$6, %esi
	movl	%esp, %edi
	call	sort
	movl	0(%esp), %r8d
	printr	%r8d
	movl	4(%esp), %r8d
	printr	%r8d
	movl	8(%esp), %r8d
	printr	%r8d
	movl	12(%esp), %r8d
	printr	%r8d
	movl	16(%esp), %r8d
	printr	%r8d
	movl	20(%esp), %r8d
	printr	%r8d
	addl	$32, %esp
	ret
swap:
	movl	$4, %ecx
	imull	%esi, %ecx
	addl	%edi, %ecx
	movl	0(%ecx), %esi
	movl	$4, %eax
	imull	%edx, %eax
	addl	%edi, %eax
	movl	0(%eax), %edx
	movl	%edx, 0(%ecx)
	movl	%esi, 0(%eax)
	ret
sort:
	pushl	%r12d
	pushl	%ebp
	pushl	%ebx
	movl	%esi, %ebp
	movl	$1, %r8d
	cmpl	%r8d, %esi
	jle	.L2
	movl	%edi, %ebx
	movl	$0, %esi
	jmp	.L4
.L8:
	movl	$1, %r12d
	addl	%esi, %r12d
	movl	%esi, %edx
	movl	%r12d, %eax
	jmp	.L5
.L7:
	movl	%eax, %edi
	movl	%edx, %ecx
	movl	$4, %r8d
	imull	%ecx, %r8d
	addl	%ebx, %r8d
	movl	0(%r8d), %ecx
	movl	$4, %r8d
	imull	%edi, %r8d
	addl	%ebx, %r8d
	movl	0(%r8d), %r8d
	cmpl	%ecx, %r8d
	jge	.L6
	movl	%eax, %edx
.L6:
	addl	$1, %eax
.L5:
	cmpl	%ebp, %eax
	jl	.L7
	movl	%ebx, %edi
	call	swap
	movl	%r12d, %esi
.L4:
	cmpl	%ebp, %esi
	jl	.L8
.L2:
	popl	%ebx
	popl	%ebp
	popl	%r12d
	ret
