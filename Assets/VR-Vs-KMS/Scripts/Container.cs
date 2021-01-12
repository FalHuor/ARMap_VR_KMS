using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Container
{
    public List<Pin> ContaminationArea = new List<Pin>();
    public List<Pin> ThrowableObject = new List<Pin>();
    public List<Pin> SpawnPoint = new List<Pin>();
}
