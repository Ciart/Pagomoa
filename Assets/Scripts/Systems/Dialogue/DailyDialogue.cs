﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa
{
    [CreateAssetMenu(fileName = "Daily Dialogue Data", menuName = "New Dialogue/Dialogue Daily Data", order = int.MaxValue)]
    public class DailyDialogue : ScriptableObject
    {
        public TextAsset[] dailyDialogues;

        public TextAsset GetRandomDialogue()
        {
            List<TextAsset> textAssets = new List<TextAsset>();
            foreach (TextAsset asset in dailyDialogues)
                textAssets.Add(asset);

            return textAssets[Random.Range(0, textAssets.Count)];
        }
    }
}
