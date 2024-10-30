using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class ParticleManager : PManager
    {
        public static ParticleManager instance { get; private set; }

        public ParticleManager()
        {
            instance ??= this;
        }
        
        public void Make(int id, GameObject parent, Vector3 position, float duration)
        {
            var particle = Object.Instantiate(DataBase.data.GetParticles()[id], parent.transform);
            particle.transform.localPosition = position;
            DataBase.data.DestroyData(particle, duration);
        }
    }
}
