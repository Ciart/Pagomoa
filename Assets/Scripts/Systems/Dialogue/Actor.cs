using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    [Serializable]
    public class Portrait
    {
        public string id;

        public float offsetX;

        public float offsetY;

        public Sprite? sprite;
    }

    [Serializable]
    public class Actor
    {
        public string id;

        public string name;

        public Portrait[] portraits;

        public void Init()
        {
            foreach (var portrait in portraits)
            {
                portrait.sprite = Resources.Load<Sprite>($"Portraits/{id}/{portrait.id}");
            }
        }

        public Portrait? GetPortrait(string portraitId)
        {
            foreach (var portrait in portraits)
            {
                if (portrait.id == portraitId)
                {
                    return portrait;
                }
            }
            
            return null;
        }
    }
}
