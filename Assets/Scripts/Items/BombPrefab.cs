using System.Collections;
using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    public class BombPrefab : MonoBehaviour
    {
        [SerializeField] private float _waitingTime;
        [SerializeField] private GameObject _bombImage;
        [SerializeField] private GameObject _bombEffect;


        private IEnumerator corutine;
        void Start()
        {
            corutine = Wait(3, gameObject);
            StartCoroutine(corutine);
        }
    
        private IEnumerator Wait(float WaitTime, GameObject Bomb)
        {
            yield return new WaitForSeconds(WaitTime);
            SetBombImage(Bomb);
            Destroy(Bomb, _waitingTime);
        }
        private void SetBombImage(GameObject Bomb)
        {
            _bombImage.SetActive(false);
            _bombEffect.transform.localScale = new Vector3(10, 10, 1);
            _bombEffect.SetActive(true);
            _bombEffect.transform.position = _bombImage.transform.position;
            var point = _bombEffect.transform.position + new Vector3(-2, -2.2f);
        
            for (int j = 0; j < 3; j++)
            {
                point.y += 1;
                for (int i = 0; i < 3; i++)
                {
                    point.x += 1;
                    var pointInt = WorldManager.ComputeCoords(point);
                    WorldManager.instance.BreakGround(pointInt.x, pointInt.y, 99999, true);
                }
                point.x = _bombEffect.transform.position.x - 2;
            }
            SoundManager.instance.PlaySfx("FootSteps", this.transform.position);
        }
        private void Destroy(GameObject Bomb, int time)
        {
            Destroy(Bomb, time);
        }
    }
}
