using System;
using System.Collections;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Timelines;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CutSceneTrigger : MonoBehaviour
    {
        [SerializeField] private bool _useTrigger;
        public CutScene playableCutScene;
        public Vector2  instantiatedPos = Vector2.zero;
        public BoxCollider2D boxCollider;
        protected CutSceneTrigger _trigger;
        
        void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            //_boxCollider.isTrigger = _useTrigger;
        }
        
        public virtual void OnCutSceneTrigger(int tick) { }
        public virtual void OffCutSceneTrigger() { }
    }
}
