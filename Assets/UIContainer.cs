using Ciart.Pagomoa.Logger;
using Ciart.Pagomoa.Systems.Inventory;
using Ciart.Pagomoa.UI;
using Ciart.Pagomoa.UI.Title;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        public Button escUIButton;
        public FadeUI fadeUIPrefab;
         
        public CinemachineVirtualCamera inventoryCamera;
        
        [Header("상호 작용 아이콘")]
        public GameObject interactableUI;
        public QuestCompleteIcon questCompleteUI;
        
        [Header("For Run Time Can be None")] 
        public DialogueUI? dialogueUI;
        
        private void Awake()
        {
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
