using UnityEngine;
using UnityEngine.UI;


namespace Ciart.Pagomoa.UI
{
    public class UIScaler : MonoBehaviour
    {
        const int BaseHeight = 360;
        const int BaseWidth = 640;
        public int widestElement = 510;
        private CanvasScaler scaler;
        private int[] res = new int[2];
        private int newheight = 0;
        private int newwidth = 0;
    
        private void Start()
        {
            scaler = GetComponent<CanvasScaler>();
            ScaleUI();
        }
    
        private void LateUpdate()
        {
#if !UNITY_EDITOR
            if (Screen.width < BaseWidth || Screen.height < BaseHeight)
            {
                Screen.SetResolution(BaseWidth, BaseHeight, FullScreenMode.Windowed);
            }
#endif
            
            if (res[0] != Screen.height || res[1] != Screen.width)
            {
                //Debug.Log("Scale Changed");
                ScaleUI();
            }
        }
    
        private void ScaleUI()
        {
            scaler.scaleFactor = Mathf.RoundToInt((float)Screen.height / BaseHeight);
    
            if (widestElement * scaler.scaleFactor > Screen.width)
            {
                scaler.scaleFactor = Mathf.RoundToInt((float)Screen.width / BaseWidth);
            }
          
            res[0] = Screen.height;
            res[1] = Screen.width;
        }
    }
}
