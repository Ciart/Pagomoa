using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa
{
    public class ButtonOnClick : MonoBehaviour
    {
        public void OnClick(Component component)
        {
            switch (component)
            {
                case AssignQuestData assignQuestData :
                    assignQuestData.ClickToQuest();
                    break;
                //case Image image :
                //    image.color = Color.white;
                //    break;
                default:
                    break;
            }
        }
    }
}
