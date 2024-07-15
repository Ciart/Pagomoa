using System;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Dialogue;
using Ciart.Pagomoa.Systems.Inventory;
using UnityEngine;
using UnityEngine.Playables;

namespace Ciart.Pagomoa
{
    public class TutorialManager : MonoBehaviour
    {
        public PlayerMovement player;
        private PlayableDirector _director;
        private float _time = 0f;
    
        void Start()
        {
            _director = GetComponentInChildren<PlayableDirector>();
        }

        void Update()
        {
            if (!CheckInventoryUfoRemote()) return;
        
            if (_director.state == PlayState.Playing)
            {
                _time += Time.deltaTime;
        
                if (_time >= 5f)
                {
                    _time = 0f;
                    //if (!DialogueManager.Instance.ConversationProgress(6))
                        StopChat();
                }

                if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
                {
                    _time = 0f;
                    //if (!DialogueManager.Instance.ConversationProgress(6))
                        StopChat();
                }    
            }
        }

        private bool CheckInventoryUfoRemote()
        {
            int idx = Array.FindIndex(GameManager.player.inventoryDB.items, element => element.item.itemType == Item.ItemType.Inherent);
            if (idx != -1)
            {
                InventoryItem item =  GameManager.player.inventoryDB.items[idx];
                if (item.item.itemName == "UFO리모컨")
                {
                    _director.Play();
                    return true;
                }
            }
            return false;
        } 

        public void StartDialogUfoRemoteGuide()
        {
            //DialogueManager.Instance.ConversationProgress(6);
        }

        public void ControlPlayerState()
        {
            player.canMove = !player.canMove;
        }

        private void StopChat()
        {
            ControlPlayerState();
            _director.Stop();
            _director.enabled = false;
        }
    }
}
