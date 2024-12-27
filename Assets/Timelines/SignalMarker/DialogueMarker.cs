using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ciart.Pagomoa.Timelines
{
    [Serializable, DisplayName("Dialogue Marker")]
    public class DialogueMarker : Marker, INotification
    {
        public TextAsset story;
        
        public PropertyName id => new("0");
    }
}