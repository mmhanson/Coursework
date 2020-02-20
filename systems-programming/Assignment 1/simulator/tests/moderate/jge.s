main:
	movl	$1, %eax
	movl	$2, %ebx
	movl	$-1, %ecx
	movl	$-2, %edx
	movl	$15, %esi
	cmpl	%eax, %eax
	jge	pass1
	printr	%esi
pass1:
	cmpl	%ebx, %eax
	jge	fail1
	movl	$11, %edi
	printr	%edi
fail1:
	cmpl	%ecx, %edx
	jge	fail2
	movl	$12, %edi
	printr	%edi
fail2:
	cmpl	%eax, %ecx
	jge	fail3
	movl	$13, %edi
	printr	%edi
fail3:	
	cmpl	%eax, %ebx
	jge	pass2
	printr	%esi
pass2:
	cmpl	%edx, %ecx
	jge	pass3
	printr	%esi
pass3:
	cmpl	%ecx, %eax
	jge	pass4
	printr	%esi
pass4:	
	ret
