main:
	pushl	%ebx
	call	two
	printr	%esp
	popl	%ebx
	ret
two:
	printr	%esp
	ret
