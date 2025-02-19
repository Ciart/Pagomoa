using System;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Inventory;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class TutorialChat : Chat
    {
        [SerializeField] private Transform target;
    
        [Space]
        public List<string> startingChat = new List<string>();
        [Space]
        public List<string> annoyingChat = new List<string>();
        [Space]
        public List<string> lastRepeatChat = new List<string>();
        [Space]
        public List<string> getUfoRemoteChat = new List<string>();
        [Space]
    
        public float initialStartTime = 4.62f;
        public float floatingTime = 3f;
        public float updateTime = 4f;

        private GameObject _tutorialChat;
    
        private int _chatIndex = 0;
        private int _loopCount = 0;
        private bool _getUfoRemote;
        private int _repeatCount = 0;

        private void Start()
        {
            _tutorialChat = transform.parent.gameObject;

            StopChat();
            StartChatReservation(initialStartTime);
        }

        private void Update()
        {
            transform.position = target.position;

            if (CheckInventoryUfoRemote())
            {
                _getUfoRemote = true;
            }
        }

        private bool CheckInventoryUfoRemote()
        {
            /*var inventory = Game.instance.player.inventory;
            
            // var inherentItem = InventoryDB.Instance.itemss.Find(inventoryItem => inventoryItem.item.itemType == Item.ItemType.Inherent);
            int idx = Array.FindIndex(inventory.inventorySlots, element => element.GetSlotItem().type == ItemType.Inherent);
            if (idx != -1)
            {
                /*InventorySlot item = inventory.inventoryItems[idx];
                if (item.GetSlotItem().name == "UFO리모컨") return true;#1#
            }*/
            return false;
        } 

        private IEnumerator Chat()
        {
            if (startingChat.Count == 0 || annoyingChat.Count == 0)
                yield break;

            if (_getUfoRemote)
            {
                GetUfoRemoteChat();
            } else {
                if (_loopCount < 3)
                    _loopCount = StartingChat();
                else if (_loopCount < 5)
                    _loopCount = AnnoyingChat();
                else
                    LastRepeatingChat();
            }

            yield return new WaitForSeconds(floatingTime);
        
            chatBalloon.balloon.SetActive(false);
            StartChatReservation();
        }
    
        private void StartChat()
        {
            StartCoroutine(nameof(Chat));
        }

        private void StopChat()
        {
            CancelInvoke(nameof(StartChat));
            StopCoroutine(nameof(Chat));
            chatBalloon.balloon.SetActive(false);
        }
    
        public void StartChatReservation()
        {
            Invoke(nameof(StartChat), updateTime);
        }

        public void StartChatReservation(float time)
        {
            Invoke(nameof(StartChat), time);
        }

        private void InitNextChat()
        {
            _chatIndex++;
            if (_chatIndex == 3)
                _loopCount += 1;
        
            if (_chatIndex >= annoyingChat.Count)
                _chatIndex = 0;
        }
    
        private int StartingChat()
        {
            Chatting(startingChat[_chatIndex]);

            InitNextChat();
        
            return _loopCount;
        }
    
        private int AnnoyingChat()
        {
            Chatting(annoyingChat[_chatIndex]);

            InitNextChat();
        
            return _loopCount;  
        }

        private void LastRepeatingChat()
        {
            Chatting(lastRepeatChat[_chatIndex]);
        
            _chatIndex++;
            if (_chatIndex >= lastRepeatChat.Count)
                _chatIndex = 0;
        }

        private void GetUfoRemoteChat()
        {
            if (_repeatCount == 5)
            {
                Destroy(_tutorialChat);
            }
        
            Chatting(getUfoRemoteChat[_chatIndex]);
        
            _chatIndex++;
            if (_chatIndex >= lastRepeatChat.Count)
            {
                _chatIndex = 0;
                _repeatCount++;
            }
        }
    
    }
}
