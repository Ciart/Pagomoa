using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Timelines;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ciart.Pagomoa
{
    public class ShopkeeperCutScene : CutScene
    {
        [Header("CutScene Character")]
        [SerializeField] private GameObject mPago;
        [SerializeField] private GameObject mShopkeeper;

        public GameObject instancePago;
        public GameObject instanceShopkeeper;

        private SignalReceiver _signalReceiver;
        
        void Start()
        {
            _signalReceiver = GetComponent<SignalReceiver>();
        }

        public override void SetInstanceCharacter()
        {
            instancePago.transform.position = new Vector2(-1, 0);
            instanceShopkeeper.transform.position = new Vector2(-3, 0);
        }

        public override void SetBinding(PlayableDirector director)
        {
            var outputs = GetTimelineClip().outputs;

            instanceShopkeeper = Instantiate(mShopkeeper);
            instancePago = Instantiate(mPago);
            
            foreach (var output in outputs)
            {
                if (output.streamName == StreamName.Pago.ToString())
                {
                    director.SetGenericBinding(output.sourceObject, instancePago);
                } 
                else if (output.streamName == StreamName.Shopkeeper.ToString())
                {
                    director.SetGenericBinding(output.sourceObject, instanceShopkeeper);
                }
                /*else if (output.streamName == StreamName.Carriage.ToString())
                {
                    director.SetGenericBinding(output.sourceObject, instancePago);
                } */
                else if (output.streamName == StreamName.Signal.ToString())
                {
                    director.SetGenericBinding(output.sourceObject, gameObject);
                }
            }
        }
    }
}
