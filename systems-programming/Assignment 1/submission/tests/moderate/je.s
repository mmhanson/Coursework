main:
	movl	$0, %eax
	movl	$0, %ebx
	movl	$1, %ecx
	cmpl	%eax, %ebx
	je	pass1
	movl	$15, %edi
	printr	%edi
pass1:
	movl	$10, %edi
	printr	%edi
	cmpl	%eax, %ecx
	je	fail2
	movl	$11, %edi
	printr	%edi
fail2:
	ret
