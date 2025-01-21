using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.UI;
using Cinemachine;
using UnityEngine;

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
        
        [Header("For Run Tine Can be None")] 
        public DialogueUI? dialogueUI;
        public ISlot chosenSlot { get; private set; }
        public void SetChosenSlot (ISlot slot) => chosenSlot = slot;
    }
}
