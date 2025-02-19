using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class ParticleManager : Manager<ParticleManager>
    {
        public void Make(int id, GameObject parent, Vector3 position, float duration)
        {
            GameObject particle;

            particle = Object.Instantiate(DataBase.data.GetParticles()[id]);
            particle.transform.position = position;
            
            //particle.transform.localPosition = position;
            Object.Destroy(particle, duration);
        }
    }
}
