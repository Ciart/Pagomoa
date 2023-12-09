using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Period", menuName = "New Period")]
[Serializable]
public class SkyLight : ScriptableObject
{
    public string Name;
    public float length;
    public Gradient skyColorSpectrum;
    public float floorIntensityStart;
    public float floorIntensityEnd;
    public float floorChangeStartTime;
    public float floorChangeEndTime;
}
