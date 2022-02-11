// Asm.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "Asm.h"


// This is an example of an exported variable
ASM_API int nAsm=0;

// This is an example of an exported function.
ASM_API int fnAsm(void)
{
    return 0;
}

// This is the constructor of a class that has been exported.
CAsm::CAsm()
{
    return;
}
