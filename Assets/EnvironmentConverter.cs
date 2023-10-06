using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class EnvironmentConverter : MonoBehaviour
{
    [SerializeField] List<Period> periods;

    Camera camera;
    public Light2D light;

    private void Awake()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    public void Convert(float nowTime, float maxTime)
    {
        SkyColorConvert();
        FloorLightConvert();
    }
    private Period GetNowPeriod()
    {
        float time = TimeManagerTemp.Instance._time / 60000f;
        float max = 0;
        for(int i = 0; i < periods.Count; i++)
        {
            max += periods[i].length;
            if(time <= max)
                return periods[i];
        }
        return null;
    }
    private float GetMaxMinTimeofPeriod(Period whatPeriod, bool isMax = true)
    {
        float max = 0;
        foreach(Period period in periods)
        {
            max += period.length;
            if (period.Equals(whatPeriod))
                if (isMax) return max * 60000;
                else return (max - period.length) * 60000;
        }
        return -1;
    }
    private float GetFlood()
    {
        Period nowPeriod = GetNowPeriod();
        float flood = (TimeManagerTemp.Instance._time - GetMaxMinTimeofPeriod(nowPeriod, false)) / (GetMaxMinTimeofPeriod(nowPeriod) - GetMaxMinTimeofPeriod(nowPeriod, false));
        return flood;
        //Debug.Log(nowPeriod + " : " + (TimeManagerTemp.Instance._time - GetMaxMinTimeofPeriod(nowPeriod, false)) + " / " + (GetMaxMinTimeofPeriod(nowPeriod) - GetMaxMinTimeofPeriod(nowPeriod, false)));
    }
    private void SkyColorConvert()
    {
        Color c;

        foreach (Period period in periods)
            if (period.Equals(GetNowPeriod()))
            {
                c = period.skyColorSpectrum.Evaluate(GetFlood());
                camera.backgroundColor = new Color(c.r, c.g, c.b);
                return;
            }
    }
    private void FloorLightConvert()
    {
        foreach (Period period in periods)
            if (period.Equals(GetNowPeriod()))
            {
                float flood = GetFlood();
                if (period.floorChangeStartTime <= flood && flood <= period.floorChangeEndTime)
                    light.intensity = (flood - period.floorChangeStartTime) / (period.floorChangeEndTime - period.floorChangeStartTime);
                return;
                //Debug.Log($"{flood - period.floorChangeStartTime} / {period.floorChangeEndTime - period.floorChangeStartTime}");
            }
    }

}