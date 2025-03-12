using Ciart.Pagomoa.Systems;
using TMPro;
using UnityEngine;

public class DaySummaryUI : MonoBehaviour {
    public TextMeshProUGUI dateText;

    public void OnSubmit() {
        Game.Instance.MoveToNextDay();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        var date = Game.Instance.Time.date;
        dateText.text = $"{date}일차 종료";
    }
}
