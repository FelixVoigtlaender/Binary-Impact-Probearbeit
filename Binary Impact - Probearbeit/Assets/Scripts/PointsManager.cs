using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static PointsManager instance;

    public IntHolder points;

    private void Awake()
    {
        instance = this;

        points.value = 0;
    }


    public void DeltaPoints(int delta)
    {
        int value = points.value + delta;
        value = Mathf.Max(0, value);
        points.value = value;
    }
    
}
