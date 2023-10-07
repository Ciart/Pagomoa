using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dropdown : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<TMP_Dropdown>().value = OptionDB.instance.scale - 1;
    }
}
