using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Timelines;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.CutScenes
{
    public enum IntroStreamName
    {
        // A Animator, G GameObject, 
        MainCamera,
        GTwinkleEffect,
        GFallingMoa,
        AFallingMoa,
        GCredit,
        APago,
        AMoa,
    }
    public class Intro : CutScene
    {
        [Header("Credit")] 
        [SerializeField] private GameObject _twinkleEffect;     //
        [SerializeField] private GameObject _fallingMoa;        //
        [SerializeField] private GameObject _credit;
        [Header("Actor")]
        [SerializeField] private GameObject _pago;
        [SerializeField] private GameObject _moa;
        
        public List<PlayableBinding> playables;
        private GameObject? _instancePago;
        private GameObject? _instanceMoa;
        
        public override List<GameObject> GetActors() { return actors; }
        public override void SetInstanceCharacter() { }

        public override void SetBinding(PlayableDirector director)
        {
            var outputs = GetTimelineClip().outputs;

            _instancePago = Instantiate(_pago);
            _instanceMoa = Instantiate(_moa);
            actors = new List<GameObject>
            {
                _instancePago,
                _instanceMoa
            };
            
            foreach (var output in outputs)
            {
                if (output.streamName == nameof(ShopkeeperStreamName.MainCamera))
                {
                    director.SetGenericBinding(output.sourceObject, DataBase.data.GetCutSceneController().mainCinemachine);
                }
                else if (output.streamName == nameof(IntroStreamName.APago))
                {
                    director.SetGenericBinding(output.sourceObject, _instancePago);
                } 
                else if (output.streamName == nameof(IntroStreamName.AMoa))
                {
                    director.SetGenericBinding(output.sourceObject, _instanceMoa);
                }
                else if (output.streamName == nameof(IntroStreamName.GCredit))
                {
                    director.SetGenericBinding(output.sourceObject, _credit);
                }
                else if (output.streamName == nameof(IntroStreamName.GTwinkleEffect))
                {
                    director.SetGenericBinding(output.sourceObject, _twinkleEffect);
                }
                else if (output.streamName is 
                         nameof(IntroStreamName.GFallingMoa) 
                         or nameof(IntroStreamName.AFallingMoa))
                {
                    director.SetGenericBinding(output.sourceObject, _fallingMoa);
                }
                else if (output.streamName.Contains("Signal"))
                {
                    director.SetGenericBinding(output.sourceObject, DataBase.data.GetCutSceneController().GetSignalReceiver());
                }
                else if (output.streamName == "FadeUI")
                {
                    director.SetGenericBinding(output.sourceObject, Game.Instance.UI.fadeUI.gameObject);
                }
                else if (output.streamName == "FadeAnimator")
                {
                    director.SetGenericBinding(output.sourceObject, Game.Instance.UI.fadeUI.animator);
                }
            }
            index = 0;
        }
    }
}

