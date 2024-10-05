using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa
{
    public class CameraShake : MonoBehaviour
    {
        [SerializeField]
        private float mRoughness;      //거칠기 정도
        [SerializeField]
        private float mMagnitude;      //움직임 범위
        
        public void ShakeCamera()
        {
            StartCoroutine(Shake(2f));
            Debug.Log("in");
        }
        
        private IEnumerator Shake(float duration)
        {
            float halfDuration = duration / 2;
            float elapsed = 0f;
            float tick = Random.Range(-10f, 10f);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime / halfDuration;

                tick += Time.deltaTime * mRoughness;
                
                var xNoise = Mathf.PerlinNoise(tick, 0) - .5f;
                var yNoise = Mathf.PerlinNoise(0, tick) - .5f;
                
                transform.position = new Vector3( xNoise - .5f, yNoise - .5f, 0f) * (mMagnitude * Mathf.PingPong(elapsed, halfDuration));

                yield return null;
            }
        }    
    }
}
