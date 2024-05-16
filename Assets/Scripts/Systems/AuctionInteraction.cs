using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class AuctionInteraction : MonoBehaviour
    {
        private InteractableObject _interactable;
        [SerializeField] private GameObject _sellUI;

        private void Start()
        {
            _interactable = GetComponent<InteractableObject>();
            _interactable.interactionEvent.AddListener(SetUI);
        }
        private void SetUI()
        {
            if (_sellUI.activeSelf == false)
            {
                _sellUI.SetActive(true);
            }
            else
                _sellUI.SetActive(false);
        }
        public void OffUI()
        {
            _sellUI.SetActive(false);
        }
    }
}
