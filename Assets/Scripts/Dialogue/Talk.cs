using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Player {
    public class Talk : MonoBehaviour
    {
        [Tooltip("Canvas 내부의 chat 할당해주세요.")]
        public GameObject talkPannel;
        [Tooltip("Canvas - chat 내부의 interlocuterImage 할당해주세요.")]
        public Image talkImage;
        [Tooltip("Canvas - chat 내부의 talking 할당해주세요.")]
        public TextMeshProUGUI talkText;

        int talkIndex;
        bool isTalking = false;

        float talkableDistance = 5;
        public void talkWith(NPC npc)
        {
            string talkData = npc.GetTalk(talkIndex);
            if (talkData == null)
            {
                npc.StopTalking();
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<PlayerMovement>().canMove = true;
                Time.timeScale = 1;
                isTalking = false;
                talkIndex = 0;
                return;
            }
            if (talkIndex == 0)
            {
                npc.StartTalking();
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<PlayerMovement>().canMove = false;
            }
            talkText.text = talkData;
            talkImage.sprite = npc.GetSprite(talkIndex);
            isTalking = true;
            talkIndex++;
            //Time.timeScale = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, talkableDistance);
                GameObject TalkableNPC = null;

                float distance = talkableDistance;
                // distance 거리 이내의 가장 가까운 NPC색출
                foreach (Collider2D collider in colliders) {
                    if (collider.GetComponent<NPC>())
                    {
                        float distanceComparison = Vector2.Distance(collider.transform.position, transform.position);

                        if (distanceComparison < distance)
                        {
                            TalkableNPC = collider.gameObject;
                            distance = distanceComparison;
                        }
                    }
                }

                if (TalkableNPC)
                {
                    talkWith(TalkableNPC.GetComponent<NPC>());
                    talkPannel.SetActive(isTalking);
                }
                else
                {
                    talkIndex = 0;
                    isTalking = false;
                    talkPannel.SetActive(isTalking);
                }

            }
        }
    }
}