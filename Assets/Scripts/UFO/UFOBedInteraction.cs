using System.Collections;
using Player;
using UnityEngine;

namespace UFO
{
    public class UFOBedInteraction : MonoBehaviour
    {
        private Transform _player;

        private InteractableObject _interactable;
        
        private TimeManagerTemp _timeManager;
        
        void Start()
        {
            _interactable = GetComponent<InteractableObject>();
            
            _interactable.InteractionEvent.AddListener(GotoBed);
            
            _timeManager = FindObjectOfType<TimeManagerTemp>();
        }

        // 플레이어 움직임 제어
        // 씬 슬립
        // 플레이어 슬립 애니메이션
        // 잘때 3초정도 잠자는 분위기 애니메이션 있으면 좋을듯
        // 컨트롤러 기능 제어
        // interactive에 할당, 구독
        // 기능은 여기서만 & 콜리션 제거

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Player") _player = collision.transform;
        }
        
        public void GotoBed()
        {
            if (_timeManager.canSleep)
            {
                _timeManager.Sleep();
                
                _player.GetComponent<PlayerMovement>().canMove = false;
                _player.GetComponent<SpriteRenderer>().enabled = false;
            
                StartCoroutine(nameof(Sleeping));
            } else {
                Debug.Log("잘 시간이 아닙니다.");
            }
        }

        private IEnumerator Sleeping()
        {
            yield return new WaitForSeconds(3f);
            
            _player.GetComponent<SpriteRenderer>().enabled = true;
            _player.GetComponent<PlayerMovement>().canMove = true;
        }
    }
}