main:
	movl	$10, %eax
	movl	$11, %ebx
	movl	$12, %ecx
	movl	$15, %esi
	jmp	first
	printr	%esi
second:
	printr	%ebx
	jmp	done
first:
	printr	%eax
	jmp	second
done:
	printr	%ecx
