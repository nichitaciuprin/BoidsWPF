#ifndef MOVEMENT_H
#define MOVEMENT_H

#include "Math.h"

void Movement_Catchup(float* AposP, float* AvelP, float Aacc, float Bpos, float Bvel, float deltaTime)
{
    float Apos = *AposP;
    float Avel = *AvelP;

    float velDiff = Bvel - Avel;
    float matchTime = abs(velDiff)/Aacc;
    float Apos2 = Apos + Avel * matchTime + Math_Sign(velDiff)*Aacc*matchTime*matchTime/2;
    float Bpos2 = Bpos + Bvel*matchTime;
    float posDiff = Bpos2 - Apos2;
    int initDirection = Math_Sign(posDiff);
    float distance = Math_Abs(posDiff);
    float offsetTime = Math_Sqrt(distance/Aacc);
    int direction;
    float time;
    float cachedValue;
    if (offsetTime > 0)
    {
        time = offsetTime;

        direction = initDirection;

        time = Math_Clamp(deltaTime,0,time);
        deltaTime -= time;
        cachedValue = direction*Aacc*time;
        Apos += Avel*time;
        Apos += cachedValue*time/2;
        Avel += cachedValue;
        if (deltaTime == 0) return;

        direction = -initDirection;

        time = Math_Clamp(deltaTime,0,time);
        deltaTime -= time;
        cachedValue = direction*Aacc*time;
        Apos += Avel*time;
        Apos += cachedValue*time/2;
        Avel += cachedValue;
        if (deltaTime == 0) return;
    }
    if (matchTime > 0)
    {
        time = matchTime;

        direction = -initDirection;

        time = Math_Clamp(deltaTime,0,time);
        deltaTime -= time;
        cachedValue = direction*Aacc*time;
        Apos += Avel*time;
        Apos += cachedValue*time/2;
        Avel += cachedValue;
        if (deltaTime == 0) return;
    }
    Apos += Avel*deltaTime;

    *AposP = Apos;
    *AvelP = Avel;
}

#endif