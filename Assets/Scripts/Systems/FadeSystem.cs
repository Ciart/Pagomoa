using System.Collections;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Systems.Time;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems
{
    public enum FadeState { FadeIn = 0, FadeOut, FadeInOut, FadeLoop }
    public class FadeSystem : MonoBehaviour
    {
        [SerializeField] [Range(0.01f, 10f)] private float fadeTime;

        [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.Linear(0,0,1,1);
    
        private Image _image;

        private FadeState _fadeState;
    
        private void Start()
        {
            _image = GetComponent<Image>();

            // TimeManager.instance.FadeEvent.AddListener(OnFade);
            EventManager.AddListener<FadeEvent>(Faded);

        }
        
        private void Faded(FadeEvent e)
        {
            OnFade(e.state);
        }
        
        private void OnFade(FadeState state)
        {
            _fadeState = state;
        
            switch ( _fadeState )
            {
                case FadeState.FadeIn:
                    StartCoroutine(Fade(0, 1));
                    break;
                case FadeState.FadeOut:
                    StartCoroutine(Fade(1, 0));
                    break;
                case FadeState.FadeInOut:
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

                if (_fadeState == FadeState.FadeInOut)
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
                currentTime += UnityEngine.Time.deltaTime;
                percent = currentTime / fadeTime;

                Color color = _image.color;
                color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
                _image.color = color;
            
                yield return null;
            }
        }
    }
}