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
        
        public void SetPlayerPlayTimeOutput()
        {
            if (gameObject.name == "Day-N")
            {
                _tmpGuiText.text = TimeManager.instance.date.ToString();
            }
            public void SetPlayerPlayTimeOutput()
            {
                var hour = TimeManager.instance.hour;
                var minute = TimeManager.instance.minute;
                
                _tmpGuiText.text = $"{hour:D2}:{minute:D2}";
            }
        }
    }
}
