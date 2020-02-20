main:
	subl	$8, %esp
	readr	%edi
	call	fact
	printr	%eax
	addl	$8, %esp
	ret
fact:
	pushl	%ebx
	movl	%edi, %ebx
	movl	$2, %r8d
	cmpl	%r8d, %edi
	je	.L3
	subl	$1, %edi
	call	fact
	imull	%ebx, %eax
	jmp	.L2
.L3:
	movl	$2, %eax
.L2:
	popl	%ebx
	ret
