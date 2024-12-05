using System;
using Ciart.Pagomoa.Systems.Time;
using Unity.VisualScripting;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public enum FadeFlag 
    { 
        FadeIn = 0, 
        FadeOut, 
        FadeInOut, 
        
        MoonFadeIn , 
        MoonFadeOut, 
        MoonFadeInOut, 
    }
    
    public class FadeUI : MonoBehaviour
    {
        private readonly int _animatorInActive = Animator.StringToHash("InActive");
        public GameObject moon;
        
        private Animator _animator;
        
        void Start()
        {
            _animator = GetComponent<Animator>();
            
            moon.SetActive(false);
        }
    
        public void Fade(FadeFlag flag, float duration = 1.0f)
        {
            if (flag <= FadeFlag.FadeInOut) moon.SetActive(false);
            else moon.SetActive(true);
            
            switch (flag)
            {
                case FadeFlag.FadeIn : case FadeFlag.MoonFadeIn:
                    FadeIn();
                    break;
                case FadeFlag.FadeOut: case FadeFlag.MoonFadeOut:
                    FadeOut();
                    break;
                case FadeFlag.FadeInOut: case FadeFlag.MoonFadeInOut:
                    _animator.SetTrigger(FadeFlag.FadeIn.ToString());
                    Action fadeOut = FadeOut;
                    
                    TimeManager.instance.SetTimer(duration, fadeOut);
                    break;
            }
        }

        public void InActiveFadeUI() { _animator.SetTrigger(_animatorInActive); }
        public void FadeIn() { _animator.SetTrigger(FadeFlag.FadeIn.ToString()); }
        public void FadeOut() { _animator.SetTrigger(FadeFlag.FadeOut.ToString()); }
    }
}
