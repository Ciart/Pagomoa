using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace Ciart.Pagomoa
{
    public class CameraResolution : MonoBehaviour
    {
        private const int PagoMoaResolutionX = 640;
        private const int PagoMoaResolutionY = 360;
        private const int PagoMoaPixelPerUnit = 16;
        
        
        [SerializeField] PixelPerfectCamera _pixelPerfectCamera;
        Camera _mainCamera;
        
        private void Start()
        {
            if (Camera.main is not null)
                _mainCamera = Camera.main;
            
            _pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
            /*_pixelPerfectCamera.cropFrameX = true;
            _pixelPerfectCamera.cropFrameY = true;*/
        }

        private void LateUpdate()
        {
            if (Screen.width < PagoMoaResolutionX)
            {
                Screen.SetResolution(PagoMoaResolutionX, Screen.height, FullScreenMode.Windowed);
            }
            if (Screen.height < PagoMoaResolutionY)
            {
                Screen.SetResolution(Screen.width, PagoMoaResolutionY, FullScreenMode.Windowed);
            }
        }
    }
}
