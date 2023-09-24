using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionInteraction : MonoBehaviour
{
    private InteractableObject _interactable;
    [SerializeField] private GameObject _sellUI;

    private void Start()
    {
        _interactable = GetComponent<InteractableObject>();
        _interactable.InteractionEvent.AddListener(SetUI);
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
