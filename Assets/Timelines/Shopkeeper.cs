using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Timelines;
using UnityEngine;
using UnityEngine.Playables;
using Vector3 = UnityEngine.Vector3;

namespace Ciart.Pagomoa
{
    public class Shopkeeper : CutScene
    {
        [Header("CutScene Character")]
        [SerializeField] private GameObject mPago;
        [SerializeField] private GameObject mShopkeeper;
        [SerializeField] private GameObject mMoa;
        
        [HideInInspector] public GameObject? instancePago;
        [HideInInspector] public GameObject? instanceShopkeeper;
        [HideInInspector] public GameObject? instanceMoa;
        
        public override void SetInstanceCharacter() 
        {
            instancePago.transform.position = Vector3.zero;
            instanceMoa.transform.position = Vector3.zero;
            instanceShopkeeper.transform.position = Vector3.zero;
            
            instancePago.transform.position = new Vector3(0.5f, 1f);
            instanceMoa.transform.position = new Vector3(1.5f, 2f);
            instanceShopkeeper.transform.position = new Vector3(10f, 0f);
        }

        public override List<GameObject> GetActors() { return actors; }
        
        public override void SetBinding(PlayableDirector director)
        {
            var outputs = GetTimelineClip().outputs;

            instanceShopkeeper = Instantiate(mShopkeeper);
            instancePago = Instantiate(mPago);
            instanceMoa = Instantiate(mMoa);

            actors = new List<GameObject>
            {
                instancePago,
                instanceMoa,
                instanceShopkeeper
            };
            
            foreach (var output in outputs)
            {
                if (output.streamName == nameof(StreamName.MainCamera))
                {
                    director.SetGenericBinding(output.sourceObject, CutSceneController.Instance.mainCinemachine);
                }
                else if (output.streamName == nameof(StreamName.Pago))
                {
                    director.SetGenericBinding(output.sourceObject, instancePago);
                } 
                else if (output.streamName == nameof(StreamName.Moa))
                {
                    director.SetGenericBinding(output.sourceObject, instanceMoa);
                }
                else if (output.streamName == nameof(StreamName.Shopkeeper))
                {
                    director.SetGenericBinding(output.sourceObject, instanceShopkeeper);
                }
                else if (output.streamName == nameof(StreamName.Signal))
                {
                    director.SetGenericBinding(output.sourceObject, GetCutSceneController().GetSignalReceiver());
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
