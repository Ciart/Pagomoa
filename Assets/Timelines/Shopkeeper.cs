using System.Collections.Generic;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Timelines;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

namespace Ciart.Pagomoa
{
    public enum ShopkeeperStreamName
    {
        MainCamera,
        Pago,
        Moa,
        Shopkeeper,
        Signal,
    }
    public class Shopkeeper : CutScene
    {
        [Header("CutScene Character")]
        [SerializeField] private GameObject _pago;
        [SerializeField] private GameObject _shopkeeper;
        [SerializeField] private GameObject _moa;
        
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

            instanceShopkeeper = Instantiate(_shopkeeper);
            instancePago = Instantiate(_pago);
            instanceMoa = Instantiate(_moa);

            actors = new List<GameObject>
            {
                instancePago,
                instanceMoa,
                instanceShopkeeper
            };
            
            foreach (var output in outputs)
            {
                if (output.streamName == nameof(ShopkeeperStreamName.MainCamera))
                {
                    director.SetGenericBinding(output.sourceObject, DataBase.data.GetCutSceneController().mainCinemachine);
                }
                else if (output.streamName == nameof(ShopkeeperStreamName.Pago))
                {
                    director.SetGenericBinding(output.sourceObject, instancePago);
                } 
                else if (output.streamName == nameof(ShopkeeperStreamName.Moa))
                {
                    director.SetGenericBinding(output.sourceObject, instanceMoa);
                }
                else if (output.streamName == nameof(ShopkeeperStreamName.Shopkeeper))
                {
                    director.SetGenericBinding(output.sourceObject, instanceShopkeeper);
                }
                else if (output.streamName == nameof(ShopkeeperStreamName.Signal))
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
