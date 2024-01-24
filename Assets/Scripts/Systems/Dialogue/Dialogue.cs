using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "New Dialogue/Dialogue")]
    [Tooltip("대화 시 띄울 텍스트, emotionId 와 수량을 1대1 대응 시켜주세요!")]
    public class Dialogue : ScriptableObject
    {
        public int id;
        public List<string> talkerName = new List<string>();
        public List<string> talk = new List<string>();
        public List<Sprite> sprite = new List<Sprite>();
    }
}
