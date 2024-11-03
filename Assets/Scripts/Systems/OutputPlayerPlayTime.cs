using Ciart.Pagomoa.Systems.Time;
using UnityEngine;
using TMPro;

namespace Ciart.Pagomoa.Systems {
    public class OutputPlayerPlayTime : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpGuiText;

        private void Update()
        {
            SetPlayerPlayTimeOutput();
        }

        private void SetPlayerPlayTimeOutput()
        {
            var timeManager = TimeManager.instance;
            
            if (gameObject.name == "Day-N")
            {
                _tmpGuiText.text = timeManager.date.ToString();
            }
            else if (gameObject.name == "Time")
            {
                var hour = timeManager.hour;
                var minute = timeManager.minute;
                
                _tmpGuiText.text = $"{hour:D2}:{minute:D2}";
            }
        }
    }
}
