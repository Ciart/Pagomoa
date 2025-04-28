using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.UI;
using Ciart.Pagomoa.UI.Title;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa
{
    public class UIContainer : MonoBehaviour
    {
        public TitleUI title;
        public MinimapUI miniMapPanelPrefab;
        public StatusUI statusPanelPrefab;
        public ShopUI shopUIPrefab;
        public BookUI bookUIPrefab;
        public QuickUI quickUIPrefab;
        public GameObject dialogueUIPrefab;
        public DaySummaryUI daySummaryUIPrefab;
        public EscOptionUI escUI;
        public FadeUI fadeUIPrefab;
        public CinemachineVirtualCamera inventoryCamera;
        
        [Header("상호 작용 아이콘")]
        public GameObject interactableUI;
        public GameObject questCompleteUI;
        
        [Header("For Run Time Can be None")] 
        public DialogueUI? dialogueUI;
    }
}
