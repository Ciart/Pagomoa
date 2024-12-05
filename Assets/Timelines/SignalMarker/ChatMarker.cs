using System;
using System.ComponentModel;
using Ciart.Pagomoa.Systems.Dialogue;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Ciart.Pagomoa.Timelines
{
    [Serializable, DisplayName("Chat Marker")]
    public class ChatMarker : Marker, INotification
    {
        public Chat targetTalker;

        public string content;
        
        [Range(0.1f, 10.0f)]public float duration = 1.0f;
        
        public PropertyName id => new("1");
    }
}