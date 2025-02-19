using System;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Time;
using UnityEngine;
using UnityEngine.UI;

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
        
        public Animator animator { get; private set; }
        
        void Start()
        {
            animator = GetComponent<Animator>();
            
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
                    animator.speed = duration;
                    
                    FadeIn();
                    break;
                case FadeFlag.FadeOut: case FadeFlag.MoonFadeOut:
                    animator.speed = duration;
                    
                    FadeOut();
                    Game.Instance.Time.SetTimer(duration, standby);
                    break;
                case FadeFlag.FadeInOut: case FadeFlag.MoonFadeInOut:
                    animator.speed = duration;
                    FadeIn();
                    Game.Instance.Time.SetTimer(duration, fadeOut);
                    Game.Instance.Time.SetTimer(duration * 2, standby);
                    break;
            }
        }

        public void StandByFadeUI() { animator.SetTrigger(_animatorInActive); gameObject.SetActive(false); }
        public void FadeIn() { animator.SetTrigger(FadeFlag.FadeIn.ToString()); }
        public void FadeOut() { animator.SetTrigger(FadeFlag.FadeOut.ToString()); }
    }
}
