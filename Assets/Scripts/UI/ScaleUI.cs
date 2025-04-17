using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI
{
    public class UIScaler : MonoBehaviour
    {
        public int baseHeight = 360;
        public int baseWidth = 640;
        public int widestElement = 510;
        private RectTransform rectTransform;
        private CanvasScaler scaler;
        private int[] res = new int[2];
        private int newheight = 0;
        private int newwidth = 0;
    
        private void Start()
        {
            scaler = GetComponent<CanvasScaler>();
            rectTransform = GetComponent<RectTransform>();
            ScaleUI();
        }
    
        private void LateUpdate()
        {
          
            if (res[0] != Screen.height || res[1] != Screen.width)
            {
                Debug.Log("Scale Changed");
                ScaleUI();
            }
    
        }
    
        private void ScaleUI()
        {
            scaler.scaleFactor = Mathf.RoundToInt((float)Screen.height / baseHeight);
    
            if (widestElement * scaler.scaleFactor > Screen.width)
            {
                scaler.scaleFactor = Mathf.RoundToInt((float)Screen.width / baseWidth);
            }
          
            res[0] = Screen.height;
            res[1] = Screen.width;
        }
    }
}
