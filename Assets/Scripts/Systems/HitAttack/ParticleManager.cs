using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.HitAttack
{
    public class ParticleManager : MonoBehaviour
    {
        public List<GameObject> particles = new List<GameObject>();

        static ParticleManager instance;

        public static ParticleManager Instance
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
        }

        public void Make(int id, GameObject parent, Vector3 position, float duration)
        {
            var particle = Instantiate(particles[id], parent.transform);
            particle.transform.localPosition = position;
            Destroy(particle, duration);
        }
    }
}
