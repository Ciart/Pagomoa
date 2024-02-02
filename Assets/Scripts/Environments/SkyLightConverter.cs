using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems.Time;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Ciart.Pagomoa.Environments
{
    public class SkyLightConverter : MonoBehaviour
    {
        public List<SkyLight> skyLights;

        public Light2D light;

        public SpriteRenderer skyShadow;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var nowSkyLight = GetNowSkyLight();
            var time = ComputeSkyLightTime(nowSkyLight);

            ChangeSkyColor(nowSkyLight, time);
            ChangeFloorLight(nowSkyLight, time);
        }

        /// <summary>
        /// 현재 Tick에 해당하는 SkyLight를 반환합니다.
        /// </summary>
        /// <returns>예기치 못한 상황에서 null이 반환될 수 있습니다.</returns>
        [CanBeNull]
        private SkyLight GetNowSkyLight()
        {
            var time = TimeManager.instance.tick / (float)TimeManager.HourTick;
            var sumLength = 0f;

            foreach (var skyLight in skyLights)
            {
                sumLength += skyLight.length;

                if (time <= sumLength)
                {
                    return skyLight;
                }
            }

            return null;
        }

        /// <summary>
        /// SkyLight의 진행도를 계산합니다.
        /// </summary>
        /// <param name="skyLight">확인할 SkyLight. 보통 </param>
        /// <returns>현재 SkyLight를 전달할 경우 0 ~ 1 값입니다. 이외에는 예기치 못한 값일 수 있습니다.</returns>
        /// <exception cref="Exception">전달된 SkyLight를 skyLights에서 찾을 수 없습니다.</exception>
        private float ComputeSkyLightTime(SkyLight skyLight)
        {
            var sumLength = 0f;

            foreach (var value in skyLights)
            {
                if (value == skyLight)
                {
                    var startTick = (sumLength + value.length) * TimeManager.HourTick;
                    var endTick = sumLength * TimeManager.HourTick;

                    return (TimeManager.instance.tick - startTick) / (endTick - startTick);
                }

                sumLength += value.length;
            }

            throw new Exception("Not found skyLight in list");
        }

        private void ChangeSkyColor(SkyLight skyLight, float time)
        {
            var color = skyLight.skyColorSpectrum.Evaluate(time);
            Color.RGBToHSV(color, out var h, out var s, out var v);

            _camera.backgroundColor = color;
            skyShadow.color = Color.HSVToRGB(h, Mathf.Min(s + 0.25f, 1f), Mathf.Min(v - 0.05f, 1f));
        }

        private void ChangeFloorLight(SkyLight skyLight, float time)
        {
            if (time < skyLight.floorChangeStartTime)
                light.intensity = skyLight.floorIntensityStart;
            else if (skyLight.floorChangeEndTime < time)
                light.intensity = skyLight.floorIntensityEnd;
            else
                light.intensity = skyLight.floorIntensityStart +
                                  (skyLight.floorIntensityEnd - skyLight.floorIntensityStart) *
                                  (time - skyLight.floorChangeStartTime) /
                                  (skyLight.floorChangeEndTime - skyLight.floorChangeStartTime);
        }
    }
}