#pragma once

typedef struct Subgen
{
    int state[55];
    int si;
    int sj;
    int seed;
} Subgen;

Subgen Subgen_Init(int seed);
float Subgen_FractionUnsigned(Subgen* subgen);
float Subgen_FractionSigned(Subgen* subgen);
float Subgen_Range(Subgen* subgen, float min, float max);