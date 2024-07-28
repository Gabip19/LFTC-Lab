%include 'io.inc'
global main
section .text
main:
MOV eax, 5

MOV [b], eax
CALL io_readint
MOV [a], eax

MOV eax, 1

MOV ebx, [a]
IMUL ebx

MOV ecx, eax
MOV eax, 3
MOV ebx, 5
IDIV ebx
PUSH eax
MOV eax, ecx

MOV ecx, eax
POP eax
MOV ebx, 4
IMUL ebx
PUSH eax
MOV eax, ecx

POP ebx
ADD eax, ebx

MOV ebx, [b]
SUB eax, ebx

MOV ebx, 2
ADD eax, ebx

MOV [a], eax
MOV eax, 3

MOV ebx, 4
ADD eax, ebx

MOV ebx, [a]
ADD eax, ebx

MOV [b], eax
MOV eax, [a]
CALL io_writeint
CALL io_writeln

MOV eax, [b]
CALL io_writeint
CALL io_writeln

ret

section .data
	a dd 0
	b dd 0
	c dd 0
