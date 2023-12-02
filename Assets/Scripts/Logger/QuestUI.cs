using TMPro;
using UnityEngine;

namespace Logger
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _mainText;
    
        void Start()
        {
        }
    
        void Update()
        {
            var processQuests = QuestManager.Instance.processQuests;
            Debug.Log(_mainText.text);
            
            var a = (ProcessQuestElements<int>)processQuests[0].elements[0];
            _mainText.text = $"{processQuests[0].description}" +
                             $"{a.summary}" +
                             $"{a.targetEntity.name} : 0 / {a.value}";
        }
    }
}
