using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Puzzle : MonoBehaviour
{
    [Header("Is this part open?")]
    public bool shortest;
    public bool longA;
    public bool longB;
    public bool longC;
    public bool sideA;
    public bool sideB;
    public bool insideA;
    public bool insideB;
    public bool insideC;
    public bool insideD;

    public int GetLong()
    {
        int res = 0;
        if (longA) res += 1;
        if (longB) res += 2;
        if (longC) res += 4;
        return res;
    }

    public int GetShort()
    {
        int res = 0;
        if (shortest) res += 8;
        return res;
    }

    public int GetSide()
    {
        int res = 0;
        if (sideA) res += 1;
        if (sideB) res += 2;
        return res;
    }

    public int GetInside()
    {
        int res = 0;
        if (insideA) res += 1;
        if (insideB) res += 2;
        if (insideC) res += 4;
        if (insideD) res += 8;
        return res;
    }
}
