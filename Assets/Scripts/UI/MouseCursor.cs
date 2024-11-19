using System;
using UnityEngine;

namespace Ciart.Pagomoa.UI
{
    public class MouseCursor : MonoBehaviour
    {
        public Texture2D cursorTexture;
        
        private void Start()
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}