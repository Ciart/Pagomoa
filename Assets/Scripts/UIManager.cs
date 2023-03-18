using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image oxygenbar;
    [SerializeField] Image hungrybar;
    public void UpdateOxygenBar()
    {
        Status playerstat = transform.parent.GetComponent<Status>();
        
        oxygenbar.fillAmount = playerstat.oxygen / playerstat.max_oxygen;
    }

    public void UpdateHungryBar()
    {
        Status playerstat = transform.parent.GetComponent<Status>();
        hungrybar.fillAmount = playerstat.hungry / playerstat.max_hungry;
    }
    public void SetPlayerUIDirection()
    {
        transform.localScale = new Vector3(transform.parent.localScale.x, 1, 1);
    }
}
