.data

BlueGreenMask dd 0FFFF00FFh,0FFFF00FFh, 0, 0FFFF00FFh,   ; Bitmaska dla kolorów Zielony-Niebieski
                 0FFFF00FFh, 0FFFF00FFh, 0, 0FFFF00FFh

RedMask dd       0, 0, 0FFFF00FFh, 0FFFF00FFh,           ;Bitmaska dla kolorów Czerwony
                 0, 0, 0FFFF00FFh, 0FFFF00FFh

BytesRemaining dq ?	; Pozosta³a wartoœc bajtów
BytesPerIter dq 32  

.code
MyProc1 Proc 
    sub r8, rdx                               ;ile bajtów jest potrzebnych do zmodyfikowania ca³ej tablicy
	cmp r8, 32                                ;sprawdzamy czy mamy 32 bajty
	jb TailLoop                               ;skok do drugiej pêtli
	add rcx, rdx                              ;przesuniêcie indexu na wartoœæ start
	mov BytesRemaining, r8                    ;zapisanie wartoœci pozosta³ych bitów
	sub rsi, rsi                              ;wyzerowanie rsi
	vmovups ymm0, ymmword ptr [BlueGreenMask] ;zapisanie wartoœci maski na ymm0

Loop1:
	cmp BytesRemaining, 0				      ;Koniec programu gdy zostanie 0 bajtów
	je Finished                               ;skok do konca programu
	cmp BytesRemaining, 32				      ;Koniec pêtli gdy mamy mniej ni¿ 32 bajty
	jb TailLoop					           
	vmovups ymm1, ymmword ptr[rcx + rsi]      ;kopjowanie 32 bajtów do ymm1
	vandps ymm2, ymm1, ymm0				      ;modyfikacja ymm1 bitmask¹ i zapisanie wartoœci do ymm2
	vmovups ymmword ptr[rcx + rsi], ymm2      ;zapisanie zmodyfikowaniej wartoœci
	
	add rsi, 32							      ;zwiêkszenie rsi o 32
	sub BytesRemaining, 32				      ;zmniejszenie liczy pozosta³ych bajtów o 32
	ja Loop1							      ;je¿eli s¹ jeszcze 32 pozosta³e bajty to wracamy do pêtli
	jb TailLoop     					      ;je¿eli nie ma to powtarzamy iteracje
	jz Finished							      ;je¿eli jest 0 pozosta³ych bajtów nast¹pi koniec

TailLoop:	
	mov eax, dword ptr[rcx + rsi]          
	and eax, dword ptr[BlueGreenMask]
	sub BytesRemaining, 4
	ja TailLoop

Finished:
  ret

MyProc1 ENDP

MyProc2 Proc 

    sub r8, rdx
	cmp r8, 32
	jb TailLoop
	add rcx, rdx
	mov BytesRemaining, r8
	sub rsi, rsi
	vmovups ymm0 ,ymmword ptr [RedMask]

Loop1:
	cmp BytesRemaining, 0				  
	je Finished
	cmp BytesRemaining, 32				  
	jb TailLoop					          
	vmovups ymm1, ymmword ptr[rcx + rsi]  
	vandps ymm2, ymm1, ymm0				  
	vmovups ymmword ptr[rcx + rsi], ymm2  
	
	add rsi, 32							  
	sub BytesRemaining, 32				  
	ja Loop1							 
	jb TailLoop     					  
	jz Finished							  

TailLoop:	
	mov eax, dword ptr[rcx + rsi]
	and eax, dword ptr[RedMask]
	sub BytesRemaining, 4
	ja TailLoop

Finished:
  ret

MyProc2 ENDP

END