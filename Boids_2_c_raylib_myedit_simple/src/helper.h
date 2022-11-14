// #include <stdlib.h>
// #include <raylib.h>
// #include <stdlib.h>
// #include "subgen.h"
#include <string.h>
// #include "Vector2.h"

void Helper_PrintEmptyLine() { printf("\n"); }
void Helper_PrintFloatHex(float f)
{
    unsigned int ui;
    memcpy(&ui, &f, sizeof (ui));
    printf("%X", ui);
    // printf("%X", *(unsigned int *)&f);
}
// void Helper_PrintVector2(Vector2 v1) { printf("<%f,%f>\n",v1.x,v1.y); }
// void Helper_PrintVector2Hex(Vector2 v1) { Helper_PrintFloatHex(v1.x), Helper_PrintFloatHex(v1.y); }
// Vector2 Vector2_RandNormDir()
// {
//     float x = Subgen_FractionSigned();
//     float y = Subgen_FractionSigned();
//     Vector2 result = Vector2Normalize( (Vector2) { x,y } );
//     return result;
// }