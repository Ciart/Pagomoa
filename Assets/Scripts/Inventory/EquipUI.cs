using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUI : MonoBehaviour
{
    public void OnUI()
    {
        transform.gameObject.SetActive(true);
    }
    public void OffUI()
    {
        transform.gameObject.SetActive(false);
    }
}
