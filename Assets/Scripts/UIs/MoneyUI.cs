using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmpGuiText;

    private void FixedUpdate()
    {
        SetMoneyUI();
    }

    private void SetMoneyUI()
    {
        _tmpGuiText.text = InventoryDB.Instance.Gold.ToString();
    }
}
