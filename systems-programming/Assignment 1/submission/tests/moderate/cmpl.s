main:
	movl	$1, %eax
	movl	$2, %ebx
	movl	$-1, %ecx
	movl	$-2, %edx
	cmpl	%eax, %eax
	printr	%eflags
	cmpl	%ecx, %ecx
	printr	%eflags
	cmpl	%eax, %ebx
	printr	%eflags
	cmpl	%ebx, %eax
	printr	%eflags
	cmpl	%eax, %ecx
	printr	%eflags
	cmpl	%ecx, %eax
	printr	%eflags
	cmpl	%ecx, %edx
	printr	%eflags
	cmpl	%edx, %ecx
	printr	%eflags
	movl	$-1, %esi
	shrl	%esi
	cmpl	%edx, %esi
	printr	%eflags
	subl	$-1, %esi
	cmpl	%eax, %esi
	printr	%eflags
	ret
