using System.Collections;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Timelines
{
    public class ShopkeeperTrigger : CutSceneTrigger
    {
        [Header("ForShopkeeperCutScene")]
        private const string TargetTag = "Player";
        [SerializeField] private GameObject _shopKeeperPrefab; 
        
        private void OnTriggerEnter2D(Collider2D targetObject)
        {
            if (targetObject.CompareTag(TargetTag))
                StartCoroutine(StartFade());
        }
        private IEnumerator StartFade()
        {
            Game.Instance.UI.PlayFadeAnimation(FadeFlag.FadeIn, 1.0f);
            yield return new WaitForSeconds(1.0f);
            StartCutScene();
        }
        
        public override void OnCutSceneTrigger(int tick)
        {
            if (tick == 18000 && Game.Instance.Time.date == 1)
            {
                _trigger = Instantiate(this, instantiatedPos, Quaternion.identity);
                Game.Instance.Time.UnregisterTickEvent(OnCutSceneTrigger);
            }
        }
        public override void OffCutSceneTrigger()
        {
            Instantiate(_shopKeeperPrefab, new Vector2(13.0f, 0.0f), Quaternion.identity);
            Destroy(_trigger);
        }
        
        private void StartCutScene()
        {
            CutSceneController.Instance.SetCutSceneTrigger(this);
            CutSceneController.Instance.StartCutScene(playableCutScene);
            
            Destroy(gameObject);
        }
        
        
    }
}