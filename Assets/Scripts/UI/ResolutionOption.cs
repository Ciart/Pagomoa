using System;
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
        PAGOMOA = 0,
        HD1280X720 ,
        FHD1920X1080,
        UHD3840X2160,
    }
    
    public class ResolutionOption : MonoBehaviour
    {
        [Header("Resolution")]
        [SerializeField] private TMP_Dropdown _dropDown;
        private Resolution[] _resolutions;
        
        [Header("ScreenState")]
        [SerializeField] private Toggle _toggle;
        [SerializeField] private TextMeshProUGUI _toggleText;
        
        private void Start()
        {
            List<string> options = new List<string>();
            for (var i = 0; i <= (int)ResolutionMenu.UHD3840X2160; i++)
            {
                ResolutionMenu option = (ResolutionMenu)i;
                options.Add(option.ToString());
            }
            _dropDown.AddOptions(options);
            SetOption();
            _dropDown.onValueChanged.AddListener(delegate { SetResolutionEvent(); });
            _toggle.onValueChanged.AddListener(delegate { SetWindowStateEvent(); });
        }

        private void OnEnable()
        {
            _toggle.isOn = Screen.fullScreen;
        }

        private void SetOption()
        {
            _resolutions = new Resolution[_dropDown.options.Count];
            for (var i = 0; i < _dropDown.options.Count; i++)
            {
                switch ((ResolutionMenu)i)
                {
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

        private void SetResolutionEvent()
        {
            ResolutionMenu menu = (ResolutionMenu)_dropDown.value;
            Screen.SetResolution(_resolutions[_dropDown.value].width
                , _resolutions[_dropDown.value].height
                , Screen.fullScreenMode);
        }

        private void SetWindowStateEvent()
        {
            var isFullScreen = _toggle.isOn;
            if (isFullScreen)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                _toggleText.text = "FullScreen";
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
                _toggleText.text = "Windowed";
            }
        }
    }
}
