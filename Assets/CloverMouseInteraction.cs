using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverMouseInteraction : MonoBehaviour
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
        Debug.Log("호출되니");
        if(_sellUI.activeSelf == false)
            _sellUI.SetActive(true);
    }
    public void OffUI()
    {
        _sellUI.SetActive(false);
    }
}
