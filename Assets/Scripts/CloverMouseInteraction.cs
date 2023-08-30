using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverMouseInteraction : MonoBehaviour
{
    private InteractableObject _interactable;
    [SerializeField] private GameObject _buyUI;

    private void Start()
    {
        _interactable = GetComponent<InteractableObject>();
        _interactable.InteractionEvent.AddListener(SetUI);
    }
    private void SetUI()
    {
        Debug.Log("호출되니");
        if(_buyUI.activeSelf == false)
            _buyUI.SetActive(true);
    }
    public void OffUI()
    {
        _buyUI.SetActive(false);
    }
}
