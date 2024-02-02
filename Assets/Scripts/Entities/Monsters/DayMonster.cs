using UnityEngine;

namespace Ciart.Pagomoa.Entities.Monsters
{
    public class DayMonster : Monster
    {
        private SpriteRenderer _sleepingAnimation;

        private void Awake()
        {

            _animator = GetComponent<Animator>();
            status = GetComponent<MonsterStatus>();
            // _attack = GetComponent<Attack>();

            _controller = GetComponent<MonsterController>();

            _sleepingAnimation = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        public static void GetSleep()
        {
            DayMonster[] Monsters = GameObject.FindObjectsOfType<DayMonster>();
            foreach (DayMonster monster in Monsters)
            {
                monster._controller.StateChanged(MonsterState.Sleep);
                monster._sleepingAnimation.enabled = true;
            }
        }
        public static void AwakeSleep()
        {
            DayMonster[] Monsters = GameObject.FindObjectsOfType<DayMonster>();
            foreach (DayMonster monster in Monsters)
            {
                monster._controller.StateChanged(MonsterState.Active);
                monster._sleepingAnimation.enabled = false;
            }
        }
    }
}
