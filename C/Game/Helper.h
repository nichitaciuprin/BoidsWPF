#include <string.h>
#include <stdio.h>

void Helper_PrintEmptyLine() { printf("\n"); }
void Helper_PrintFloatHex(float f)
{
    unsigned int ui;
    memcpy(&ui, &f, sizeof (ui));
    printf("%X", ui);
}
void Helper_MaybeWarn(const char* msg, long actualTime, long maxTime)
{
    if (actualTime > maxTime)
        printf("%s IS SLOW. %ld > %ld ms\n",msg,actualTime,maxTime);
}