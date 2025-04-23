using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ciart.Pagomoa.Logger
{
    public class QuestCompleteIcon : MonoBehaviour
    {
        public GameObject availableIcon;
        public GameObject completableIcon;

        private void Awake()
        {
            InactiveIcon();
        }

        public void ActiveAvailableIcon()
        {
            availableIcon.SetActive(true);
            completableIcon.SetActive(false);
        }

        public void ActiveCompletableIcon()
        {
            availableIcon.SetActive(false);
            completableIcon.SetActive(true);
        }

        public void InactiveIcon()
        {
            availableIcon.SetActive(false);
            completableIcon.SetActive(false);
        }
    }
}
