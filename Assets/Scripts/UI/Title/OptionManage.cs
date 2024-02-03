using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Title
{
    public class OptionManage : MonoBehaviour
    {
        public static OptionManage instance = null;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
            LoadOption();
        }
        public void LoadOption()
        {
            if(OptionDB.instance)
                gameObject.GetComponent<CanvasScaler>().scaleFactor = OptionDB.instance.scale;
        }
    }
}
