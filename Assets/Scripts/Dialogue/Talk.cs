using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Player
{
    public class Talk : MonoBehaviour
    {
        float talkableDistance = 5;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, talkableDistance);
                GameObject TalkableNPC = null;

                float distance = talkableDistance;
                // distance �Ÿ� �̳��� ���� ����� NPC����
                foreach (Collider2D collider in colliders)
                {
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
                    TalkableNPC.GetComponent<NPC>().Talking();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {

                DialogueManager.Instance.ConversationProgress(1);
            }
        }
    }
}