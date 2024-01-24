using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Items;
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
                    if (!DialogueManager.Instance.ConversationProgress(6))
                        StopChat();
                }

                if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
                {
                    _time = 0f;
                    if (!DialogueManager.Instance.ConversationProgress(6))
                        StopChat();
                }    
            }
        }

        private bool CheckInventoryUfoRemote()
        {
            var inherentItem = InventoryDB.Instance.items.Find(inventoryItem => inventoryItem.item.itemType == Item.ItemType.Inherent);

            if (InventoryDB.Instance.items.Contains(inherentItem))
            {
                if (inherentItem.item.itemName == "UFO리모컨")
                {
                    _director.Play();
                    return true;
                }
            }
            return false;
        } 

        public void StartDialogUfoRemoteGuide()
        {
            DialogueManager.Instance.ConversationProgress(6);
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
