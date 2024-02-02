using UnityEngine;
using TMPro;

public class OutputPlayerPlayTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tmpGuiText;

    private void Update()
    {
        SetPlayerPlayTimeOutput();
    }
    
    public void SetPlayerPlayTimeOutput()
    {
        if (gameObject.name == "Day-N")
        {
            _tmpGuiText.text = TimeManager.instance.date.ToString();
        }
        if(gameObject.name == "Time")
        {
            var hour = TimeManager.instance.hour;
            var minute = TimeManager.instance.minute;
            
            _tmpGuiText.text = $"{hour:D2}:{minute:D2}";
        }
    }
}
