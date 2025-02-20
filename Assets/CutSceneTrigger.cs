using System;
using System.Collections;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Timelines;
using UnityEngine;

namespace Ciart.Pagomoa
{
    public class CutSceneTrigger : MonoBehaviour
    {
        public CutScene playableCutScene;
        
        private BoxCollider2D _boxCollider;
        
        private const string TargetTag = "Player";
        [SerializeField] private GameObject _shopKeeperPrefab;
        
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
            DataBase.data.GetCutSceneController().StartCutScene(playableCutScene);
            
            Destroy(this.gameObject);
        }

        private IEnumerator StartFade()
        {
            Game.Instance.UI.PlayFadeAnimation(FadeFlag.FadeIn, 1.0f);

            yield return new WaitForSeconds(1.0f);
            
            StartCutScene();
        }
        
        // TODO : 상속으로 컷신 추가시 컷신마다의 기능으로 변경되어야 함. 
        /*public void OnCutSceneTrigger(Action afterCutScene)
        {
            Game.Instance.Time.SetTimer(600, afterCutScene);
        }*/

        public void OffCutSceneTrigger()
        {
            Instantiate(_shopKeeperPrefab, new Vector2(13.0f, 0.0f), Quaternion.identity);
        }
    }
}
