using Ciart.Pagomoa.Systems.Time;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems {
    public class OutputPlayerPlayTime : MonoBehaviour
    { 
        private enum TargetTimeValue
        {
            TargetDate = 0,
            TargetTime = 1,
        }
        
        private TargetTimeValue _targetTimeValue;
        public void SetTargetTimeValue(int value) { _targetTimeValue = (TargetTimeValue)value; }
        
        private TextMeshProUGUI _tmpGuiText;

        private void Start()
        {
            _tmpGuiText = GetComponent<TextMeshProUGUI>();    
        }

        public void UpdatePlayTimeOutput()
        {
            var timeManager = TimeManager.instance;
            
            if (_targetTimeValue == TargetTimeValue.TargetDate)
            {
                _tmpGuiText.text = timeManager.date.ToString();
            }
            else if (_targetTimeValue == TargetTimeValue.TargetTime)
            {
                var hour = timeManager.hour;
                var minute = timeManager.minute;
                
                _tmpGuiText.text = $"{hour:D2}:{minute:D2}";
            }
        }
    }
}
