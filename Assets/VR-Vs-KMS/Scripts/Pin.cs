using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pin
{
    public float x;
    public float y;
    public float z;

    public Pin(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

}
