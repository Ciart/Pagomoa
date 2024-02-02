using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public class ParticleManager : SingletonMonoBehaviour<ParticleManager>
    {
        public List<GameObject> particles = new List<GameObject>();

        public void Make(int id, GameObject parent, Vector3 position, float duration)
        {
        
            //Debug.Log("생성됨!" + particles.Count);

            GameObject particle = Instantiate(particles[id], parent.transform);
            particle.transform.localPosition = position;
            Destroy(particle, duration);
        }
    }
}
