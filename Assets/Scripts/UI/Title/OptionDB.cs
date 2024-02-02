using Ciart.Pagomoa.Systems.Save;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Title
{
    public class OptionDB : MonoBehaviour
    {
        public static OptionDB instance = null;

        [SerializeField] public int scale;
        [SerializeField] public float audioValue;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if(instance != this)
                    Destroy(gameObject);
            }
            SaveManager.Instance.LoadOption();
            GameObject.Find("Canvas").GetComponent<CanvasScaler>().scaleFactor = scale;
        }
    }
}
