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
            UIManager.instance.PlayFadeAnimation(FadeFlag.FadeIn, 1.0f);

            yield return new WaitForSeconds(1.0f);
            
            StartCutScene();
        }
    }
}
