using System;
using System.Collections;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Timelines;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa
{
    public class CutSceneTrigger : MonoBehaviour
    {
        public CutScene playableCutScene;
        public Vector2  instantiatedPos;
        
        private BoxCollider2D _boxCollider;
        private const string TargetTag = "Player";
        [SerializeField] private GameObject _shopKeeperPrefab;

        private CutSceneTrigger _trigger;
        
        void Start()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _boxCollider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D targetObject)
        {
            if (targetObject.CompareTag(TargetTag))
                StartCoroutine(StartFade());
        }

        private void StartCutScene()
        {
            DataBase.data.GetCutSceneController().SetCutSceneTrigger(this);
            DataBase.data.GetCutSceneController().StartCutScene(playableCutScene);
            
            Destroy(gameObject);
        }

        private IEnumerator StartFade()
        {
            Game.Instance.UI.PlayFadeAnimation(FadeFlag.FadeIn, 1.0f);

            yield return new WaitForSeconds(1.0f);
            
            StartCutScene();
        }
        

        public void OnCutSceneTrigger(int tick)
        {
            if (tick == 18000 && Game.Instance.Time.date == 1)
            {
                _trigger = Instantiate(this, instantiatedPos, Quaternion.identity);
                Game.Instance.Time.UnregisterTickEvent(OnCutSceneTrigger);
            }
        }

        public void OffCutSceneTrigger()
        {
            Instantiate(_shopKeeperPrefab, new Vector2(13.0f, 0.0f), Quaternion.identity);
            Destroy(_trigger);
        }
    }
}
