main:
	movl	$1, %eax
	movl	$2, %ebx
	movl	$-1, %ecx
	movl	$-2, %edx
	movl	$15, %esi
	cmpl	%eax, %eax
	jle	pass1
	printr	%esi
pass1:
	cmpl	%eax, %ebx
	jle	fail1
	movl	$11, %edi
	printr	%edi
fail1:
	cmpl	%edx, %ecx
	jle	fail2
	movl	$12, %edi
	printr	%edi
fail2:
	cmpl	%ecx, %eax
	jle	fail3
	movl	$13, %edi
	printr	%edi
fail3:	
	cmpl	%ebx, %eax
	jle	pass2
	printr	%esi
pass2:
	cmpl	%ecx, %edx
	jle	pass3
	printr	%esi
pass3:
	cmpl	%eax, %ecx
	jle	pass4
	printr	%esi
pass4:	
	ret
