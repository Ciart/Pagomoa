using TMPro;
using UnityEngine;

namespace Logger
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _mainText;

        void Update()
        {
            var progressQuests = QuestManager.Instance.progressQuests;

            foreach (var progressQuest in progressQuests)
            {
                
            }
            
            /*var a = (progressQuestElements<int>)progressQuests[0].elements[0];
            _mainText.text = $"{progressQuests[0].description}" +
                             $"{a.summary}" +
                             $"{a.targetEntity.name} : 0 / {a.value}";*/
        }
    }
}
