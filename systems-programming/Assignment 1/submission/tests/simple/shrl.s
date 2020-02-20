main:
	movl	$21, %eax
	shrl	%eax
	printr	%eax
	shrl	%eax
	printr	%eax
	shrl	%eax
	printr	%eax
	shrl	%eax
	printr	%eax
	shrl	%eax
	printr	%eax
	movl	$-1, %ebx
	shrl	%ebx
	printr	%ebx
	movl	$-32768, %ecx
	shrl	%ecx
	printr	%ecx
