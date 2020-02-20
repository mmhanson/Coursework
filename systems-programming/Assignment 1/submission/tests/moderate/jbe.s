main:
	movl	$1, %eax
	movl	$2, %ebx
	movl	$-1, %ecx
	movl	$-2, %edx
	movl	$15, %esi
	cmpl	%eax, %eax
	jbe	pass1
	printr	%esi
pass1:
	cmpl	%eax, %ebx
	jbe	fail1
	movl	$11, %edi
	printr	%edi
fail1:
	cmpl	%edx, %ecx
	jbe	fail2
	movl	$12, %edi
	printr	%edi
fail2:
	cmpl	%eax, %ecx
	jbe	fail3
	movl	$13, %edi
	printr	%edi
fail3:	
	cmpl	%ebx, %eax
	jbe	pass2
	printr	%esi
pass2:
	cmpl	%ecx, %edx
	jbe	pass3
	printr	%esi
pass3:
	cmpl	%ecx, %eax
	jbe	pass4
	printr	%esi
pass4:	
	ret
