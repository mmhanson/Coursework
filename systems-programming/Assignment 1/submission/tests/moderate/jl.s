main:
	movl	$1, %eax
	movl	$2, %ebx
	movl	$-1, %ecx
	movl	$-2, %edx
	movl	$15, %esi
	cmpl	%eax, %eax
	jl	fail1
	movl	$10, %edi
	printr	%edi
fail1:
	cmpl	%eax, %ebx
	jl	fail2
	movl	$11, %edi
	printr	%edi
fail2:
	cmpl	%edx, %ecx
	jl	fail3
	movl	$12, %edi
	printr	%edi
fail3:
	cmpl	%ecx, %eax
	jl	fail4
	movl	$13, %edi
	printr	%edi
fail4:	
	cmpl	%ebx, %eax
	jl	pass1
	printr	%esi
pass1:
	cmpl	%ecx, %edx
	jl	pass2
	printr	%esi
pass2:
	cmpl	%eax, %ecx
	jl	pass3
	printr	%esi
pass3:	
	ret
