using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class UIContainer : MonoBehaviour
    {
        [Header("UI Data")]
        public Image oxygenBar;
        public Image hungryBar;
        public GameObject inventoryUIPrefab;
        public GameObject dialogueUIPrefab;
        public GameObject quickSlotContainerUIPrefab;
        public GameObject escUI;
        public GameObject interactableUI;
        public GameObject questCompleteUI;
        [CanBeNull] public DialogueUI dialogueUI;
        public CinemachineVirtualCamera inventoryCamera;
    }
}
