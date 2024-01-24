using UnityEngine;

namespace Ciart.Pagomoa.UI.Title
{
    public class SetQuestionUI : MonoBehaviour
    {
        public void SetUI()
        {
            bool activeUI = false;
            if (gameObject.activeSelf == true)
                activeUI = false;
            else
                activeUI = true;
            gameObject.SetActive(activeUI);
        }
    }
}
