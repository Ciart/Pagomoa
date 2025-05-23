using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public struct Resolution
    {
        public int width;
        public int height;
    }

    public enum ResolutionMenu
    {
        FULLSCREEN = 0,
        WINDOWED, 
        PAGOMOA,
        HD1280X720 ,
        FHD1920X1080,
        UHD3840X2160,
    }
    
    public class ResolutionOption : MonoBehaviour
    {
        public Resolution currentResolution;
        [SerializeField] private TMP_Dropdown _dropDown;
        private Resolution[] _resolutions;
        
        void Start()
        {
            List<string> options = new List<string>();
            for (var i = 0; i <= (int)ResolutionMenu.UHD3840X2160; i++)
            {
                ResolutionMenu option = (ResolutionMenu)i;
                options.Add(option.ToString());
            }
            _dropDown.AddOptions(options);
            SetOption();
            _dropDown.onValueChanged.AddListener(delegate { SetEvent(); });
        }
        
        private void SetOption()
        {
            _resolutions = new Resolution[_dropDown.options.Count];
            for (var i = 0; i < _dropDown.options.Count; i++)
            {
                switch ((ResolutionMenu)i)
                {
                    case ResolutionMenu.FULLSCREEN:
                    case ResolutionMenu.WINDOWED:
                        _resolutions[i] = new Resolution()
                            { width = 0, height = 0 };
                        break;
                    case ResolutionMenu.PAGOMOA:
                        _resolutions[i] = new Resolution() 
                            { width = 640, height = 360 };
                        break;
                    case ResolutionMenu.HD1280X720:
                        _resolutions[i] = new Resolution() 
                            { width = 1280, height = 720 };
                        break;
                    case ResolutionMenu.FHD1920X1080:
                        _resolutions[i] = new Resolution() 
                            { width = 1920, height = 1080 };
                        break;
                    case ResolutionMenu.UHD3840X2160:
                        _resolutions[i] = new Resolution() 
                            { width = 3840, height = 2160 };
                        break;
                }
            }
        }

        private void SetEvent()
        {
            Debug.Log(_dropDown.value);
            ResolutionMenu menu = (ResolutionMenu)_dropDown.value;
            switch (menu)
            {
                case ResolutionMenu.FULLSCREEN:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    return;
                case ResolutionMenu.WINDOWED:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    return;
                default:
                    Screen.SetResolution(_resolutions[_dropDown.value].width
                        , _resolutions[_dropDown.value].height
                        , FullScreenMode.ExclusiveFullScreen);
                    break;
            }

            
        }
    }
}
