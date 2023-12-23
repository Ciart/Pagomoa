using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Environments
{
    public class SkyLightConverter : MonoBehaviour
    {
        public SkyLight[] skyLights;

        public Light2D light;

        public SpriteRenderer skyShadow;

        private TimeManager _timeManager;
        
        private Camera _camera;

        private void Awake()
        {
            _timeManager = TimeManager.Instance;
            _camera = Camera.main;
        }

        private void Update()
        {
            SkyColorConvert();
            FloorLightConvert();
        }

        private SkyLight GetNowSkyLight()
        {
            var time = TimeManager.Instance.time / 60000f;
            var max = 0f;

            foreach (var t in skyLights)
            {
                max += t.length;

                if (time <= max)
                {
                    return t;
                }
            }

            return null;
        }

        private float GetMaxMinTimeOfPeriod(SkyLight whatSkyLight, bool isMax = true)
        {
            var max = 0f;

            foreach (var period in skyLights)
            {
                max += period.length;

                if (period.Equals(whatSkyLight))
                    if (isMax) return max * 60000;
                    else return (max - period.length) * 60000;
            }

            return -1;
        }

        private float GetFlood()
        {
            var nowSkyLight = GetNowSkyLight();
            var flood = (TimeManager.Instance.time - GetMaxMinTimeOfPeriod(nowSkyLight, false)) /
                        (GetMaxMinTimeOfPeriod(nowSkyLight) - GetMaxMinTimeOfPeriod(nowSkyLight, false));

            return flood;
        }

        private void SkyColorConvert()
        {
            foreach (SkyLight period in skyLights)
                if (period.Equals(GetNowSkyLight()))
                {
                    var color = period.skyColorSpectrum.Evaluate(GetFlood());
                    Color.RGBToHSV(color, out var h, out var s, out var v);

                    _camera.backgroundColor = color;
                    skyShadow.color = Color.HSVToRGB(h, Mathf.Min(s + 0.25f, 1f), Mathf.Min(v - 0.05f, 1f));

                    return;
                }
        }

        private void FloorLightConvert()
        {
            foreach (SkyLight skyLight in skyLights)
                if (skyLight.Equals(GetNowSkyLight()))
                {
                    float flood = GetFlood();

                    if (flood < skyLight.floorChangeStartTime)
                        light.intensity = skyLight.floorIntensityStart;
                    else if (skyLight.floorChangeEndTime < flood)
                        light.intensity = skyLight.floorIntensityEnd;
                    else
                        light.intensity = skyLight.floorIntensityStart +
                                          (skyLight.floorIntensityEnd - skyLight.floorIntensityStart) *
                                          (flood - skyLight.floorChangeStartTime) / (skyLight.floorChangeEndTime -
                                              skyLight.floorChangeStartTime);

                    return;
                }
        }
    }
}