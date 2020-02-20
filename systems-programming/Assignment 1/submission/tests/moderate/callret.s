main:
	movl	$1, %eax
	printr	%eax
	call	two
	movl	$1, %eax
	printr	%eax
	ret
two:
	movl	$2, %eax
	printr	%eax
	call	three
	movl	$2, %eax
	printr	%eax
	ret
three:
	movl	$3, %eax
	printr	%eax
	ret
