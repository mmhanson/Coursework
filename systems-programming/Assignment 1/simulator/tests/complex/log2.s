main:
	subl	$8, %esp
	readr	%edi
	call	log2
	printr	%eax
	addl	$8, %esp
	ret
log2:
	movl	$2, %r8d
	cmpl	%r8d, %edi
	jbe	.L3
	subl	$8, %esp
	shrl	%edi
	call	log2
	addl	$1, %eax
	jmp	.L2
.L3:
	movl	$1, %eax
	ret
.L2:
	addl	$8, %esp
	ret
