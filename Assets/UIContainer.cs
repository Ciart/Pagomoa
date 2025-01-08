using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.UI;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class UIContainer : MonoBehaviour
    {
        public MinimapUI miniMapPanelPrefab;
        public StateUI statePanelPrefab;
        public ShopUI shopUIPrefab;
        public BookUI bookUIPrefab;
        public QuickSlotUI quickSlotUIPrefab;
        public GameObject dialogueUIPrefab;
        public GameObject escUI;
        public FadeUI fadeUIPrefab;
        public CinemachineVirtualCamera inventoryCamera;
        
        [Header("상호 작용 아이콘")]
        public GameObject interactableUI;
        public GameObject questCompleteUI;
        
        [Header("For Run Tine Can be None")] public DialogueUI? dialogueUI;
    }
}
