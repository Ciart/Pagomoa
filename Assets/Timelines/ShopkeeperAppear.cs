using Ciart.Pagomoa.Timelines;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ciart.Pagomoa
{
    public enum StreamName
    {
        Pago,
        Moa,
        Shopkeeper,
        Signal, 
    }
    public class ShopkeeperAppear : CutScene
    {
        [Header("CutScene Character")]
        [SerializeField] private GameObject mPago;
        [SerializeField] private GameObject mShopkeeper;
        [SerializeField] private GameObject mMoa;

        public GameObject instancePago;
        public GameObject instanceShopkeeper;
        public GameObject instanceMoa;

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
                else if (output.streamName == StreamName.Moa.ToString())
                {
                    director.SetGenericBinding(output.sourceObject, instanceMoa);
                }
                else if (output.streamName == StreamName.Shopkeeper.ToString())
                {
                    director.SetGenericBinding(output.sourceObject, instanceShopkeeper);
                }
                else if (output.streamName == StreamName.Signal.ToString())
                {
                    director.SetGenericBinding(output.sourceObject, gameObject);
                }
            }
        }
    }
}
