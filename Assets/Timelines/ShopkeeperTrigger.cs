using System.Collections;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Logger.ProcessScripts;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Timelines
{
    public class ShopkeeperTrigger : CutSceneTrigger
    {
        [Header("ForShopkeeperCutScene")]
        private const string TargetTag = "Player";
        [SerializeField] private GameObject _shopKeeperPrefab;
        private int _tutorialCounter;
        private Vector3 _beforePosition;
        
        private void OnTriggerEnter2D(Collider2D targetObject)
        {
            if (targetObject.CompareTag(TargetTag))
                StartCoroutine(StartFade());
        }
        private IEnumerator StartFade()
        {
            Game.Instance.UI.PlayFadeAnimation(FadeFlag.FadeIn, 1.0f);
            yield return new WaitForSeconds(1.0f);
            var pos = Game.Instance.player.transform.position;
            _beforePosition = new Vector3(pos.x, pos.y);
            Game.Instance.player.transform.position = Vector3.zero + new Vector3(0, 1f, 0);
            StartCutScene();
        }
        
        public override void OnCutSceneTrigger(int tick)
        {
            if (!_trigger && Game.Instance.Time.date == 0)
            {
                _tutorialCounter = 0;
                _trigger = Instantiate(this, instantiatedPos, Quaternion.identity);
                DontDestroyOnLoad(_trigger);
                EventManager.AddListener<QuestUpdated>(CheckTutorialQuestEnd);
            }
        }
        public override void OffCutSceneTrigger()
        {
            Game.Instance.player.transform.position = _beforePosition;
            Instantiate(_shopKeeperPrefab, new Vector2(13.0f, 0.0f), Quaternion.identity);
            Destroy(_trigger);
            EventManager.RemoveListener<QuestUpdated>(CheckTutorialQuestEnd);
        }

        private void CheckTutorialQuestEnd(QuestUpdated e)
        {
            if (e.quest.state != QuestState.Finish) return;
            var contains = e.quest.id.Contains("tutorial");
            if (contains) _tutorialCounter++;
            if (_tutorialCounter == 2)
            {
                boxCollider = gameObject.GetComponent<BoxCollider2D>();
                _trigger.boxCollider.enabled= true;
            }
        }
        
        private void StartCutScene()
        {
            DataBase.data.GetCutSceneController().SetCutSceneTrigger(this);
            DataBase.data.GetCutSceneController().StartCutScene(playableCutScene);
            
            Destroy(gameObject);
        }
        
        
    }
}