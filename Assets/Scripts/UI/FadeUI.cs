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
            
            gameObject.SetActive(false);
            moon.SetActive(false);
        }
    
        public void Fade(FadeFlag flag, float duration = 1.0f)
        {
            if (flag <= FadeFlag.FadeInOut) moon.SetActive(false);
            else moon.SetActive(true);
            
            Action fadeOut = FadeOut;
            Action standby = StandByFadeUI;
            
            switch (flag)
            {
                case FadeFlag.FadeIn : case FadeFlag.MoonFadeIn:
                    _animator.speed = duration;
                    
                    FadeIn();
                    break;
                case FadeFlag.FadeOut: case FadeFlag.MoonFadeOut:
                    _animator.speed = duration;
                    
                    FadeOut();
                    TimeManager.instance.SetTimer(duration, standby);
                    break;
                case FadeFlag.FadeInOut: case FadeFlag.MoonFadeInOut:
                    _animator.speed = duration;
                    FadeIn();
                    TimeManager.instance.SetTimer(duration, fadeOut);
                    TimeManager.instance.SetTimer(duration * 2, standby);
                    break;
            }
        }

        public void StandByFadeUI() { _animator.SetTrigger(_animatorInActive); gameObject.SetActive(false); }
        public void FadeIn() { _animator.SetTrigger(FadeFlag.FadeIn.ToString()); }
        public void FadeOut() { _animator.SetTrigger(FadeFlag.FadeOut.ToString()); }
    }
}
