using System.Collections;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Time;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems
{
    public enum FadeState { MoonFadeIn = 0, MoonFadeOut, MoonFadeInOut, FadeIn, FadeOut, FadeInOut, FadeLoop }
    public class FadeSystem : MonoBehaviour
    {
        [SerializeField] [Range(0.01f, 10f)] private float fadeTime;

        [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.Linear(0,0,1,1);
    
        private Image _image;

        private FadeState _fadeState;
    
        private void Start()
        {
            _image = GetComponent<Image>();
            EventManager.AddListener<FadeEvent>(Faded);
        }
        
        private void Faded(FadeEvent e)
        {
            OnFade(e.state);
        }
        
        private void OnFade(FadeState state)
        {
            if (state is FadeState.FadeIn or FadeState.FadeOut or FadeState.FadeInOut)
            {
                _image.color = new Color(.0f, .0f,.0f);
            }
            
            _fadeState = state;
        
            switch ( _fadeState )
            {
                case FadeState.FadeIn:
                case FadeState.MoonFadeIn:
                    StartCoroutine(Fade(0, 1));
                    break;
                case FadeState.FadeOut:
                case FadeState.MoonFadeOut:
                    StartCoroutine(Fade(1, 0));
                    break;
                case FadeState.FadeInOut:
                case FadeState.MoonFadeInOut:    
                case FadeState.FadeLoop:
                    StartCoroutine(FadeInOut());
                    break;
            }
        }

        private IEnumerator FadeInOut()
        {
            while (true)
            {
                yield return StartCoroutine(Fade(0, 1));

                yield return new WaitForSeconds(4f);
            
                yield return StartCoroutine(Fade(1, 0));

                if (_fadeState != FadeState.FadeLoop)
                {
                    break;
                } 
            }
        }

        private IEnumerator Fade(float start, float end)
        {
            float currentTime = 0.0f;
            float percent = 0.0f;
        
            while (percent < 1)
            {
                currentTime += UnityEngine.Time.fixedDeltaTime;
                percent = currentTime / fadeTime;

                Color color = _image.color;
                color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
                _image.color = color;
            
                yield return null;
            }
        }
    }
}