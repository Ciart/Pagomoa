using Ciart.Pagomoa.Systems;
using UnityEngine;

public class DaySummaryUI : MonoBehaviour {
    public void OnSubmit() {
        Game.instance.MoveToNextDay();
        gameObject.SetActive(false);
    }
}
