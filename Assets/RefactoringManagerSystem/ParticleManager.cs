using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class ParticleManager : PManager<ParticleManager>
    {
        public void Make(int id, GameObject parent, Vector3 position, float duration)
        {
            var particle = Object.Instantiate(DataBase.data.GetParticles()[id], parent.transform);
            particle.transform.localPosition = position;
            Object.Destroy(particle, duration);
            //DataBase.data.DestroyData(, duration);
        }
    }
}
