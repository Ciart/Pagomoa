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
        public MinimapUI miniMapPanel;
        public StateUI statePanel;
        public ShopUI shopUI;
        public GameObject inventoryUIPrefab;
        public GameObject dialogueUIPrefab;
        public GameObject quickSlotContainerUIPrefab;
        public GameObject escUI;
        public GameObject interactableUI;
        public GameObject questCompleteUI;
        public GameObject fadeUI;
        public CinemachineVirtualCamera inventoryCamera;
        
        [Header("For Run Tine Can be None")] public DialogueUI? dialogueUI;
    }
}
