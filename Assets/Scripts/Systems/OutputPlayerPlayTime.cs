using Ciart.Pagomoa.Systems.Time;
using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class OutputPlayerPlayTime : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpGuiText;

        private void FixedUpdate()
        {
            SetPlayerPlayTimeOutput();
        }
        public void SetPlayerPlayTimeOutput()
        {
            if (gameObject.name == "Day-N")
            {
                _tmpGuiText.text = TimeManager.Instance.GetDay().ToString();
            }
            if(gameObject.name == "Time")
            {
                string hour = TimeManager.Instance.GetHour().ToString();
                string minute = TimeManager.Instance.GetMinute().ToString();

                if (int.Parse(hour) < 10)
                    hour = string.Concat(0, hour);
                if(int.Parse(minute) < 10) 
                    minute = string.Concat(0, minute);

                _tmpGuiText.text = string.Format("{0}:{1}", hour, minute);
            }
        }
    }
}
