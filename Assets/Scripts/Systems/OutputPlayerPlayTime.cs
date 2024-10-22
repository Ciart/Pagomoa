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
            if (gameObject.name == "Day-N")
            {
                _tmpGuiText.text = Game.Get<TimeManager>().date.ToString();
            }
            else if (gameObject.name == "Time")
            {
                var hour = Game.Get<TimeManager>().hour;
                var minute = Game.Get<TimeManager>().minute;
                
                _tmpGuiText.text = $"{hour:D2}:{minute:D2}";
            }
        }
    }
}
