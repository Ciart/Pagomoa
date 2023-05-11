using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    // Hp(체력) - new
    [SerializeField] protected float _hp = 100;
    [SerializeField] protected float _max_hp = 100;

    [SerializeField] protected float _attackpower = 100;

    [Space]
    // SIght(시야)
    [SerializeField] protected float _sight = 1;
    [SerializeField] protected float _insight = 1;
    [SerializeField] protected float _outsight = 1;
    [Space]

    // 이동속도
    [SerializeField] protected float _speed = 5;
    [SerializeField] protected float _inspeed = 5;
    [SerializeField] protected float _outspeed = 5;

    [Space]


    // Dig Option(채굴옵션) - new
    [SerializeField] protected float _dig_speed = 100;

    // CrawlUp Speed (올라가기 속도) - new
    [SerializeField] protected float _crawlup_speed = 100;

    [Space]

    // OxyGen(산소)
    [SerializeField] protected float _oxygen = 100;
    [SerializeField] protected float _max_oxygen = 100;
    [SerializeField] protected float _min_oxygen = 0;
    [Space]

    // Hungry(배고픔)
    [SerializeField] protected float _hungry = 100;
    [SerializeField] protected float _max_hungry = 100;
    [SerializeField] protected float _min_hungry = 0;
    [Space]

    // Recovery(재생속도) - new
    [SerializeField] protected float _hp_recovery = 1;
    [SerializeField] protected float _oxygen_recovery = 1;
    [SerializeField] protected float _oxygen_consume = 1;
    [SerializeField] protected float _hungry_recovery = 1;
    [SerializeField] protected float _hungry_consume = 1;


    public float hp { get { return _hp; } set { _hp = value; } }
    public float maxhp { get { return _max_hp; } set { _max_hp = value; } }

    public float attackpower { get { return _attackpower; } set { _attackpower = value; } }

    // SIght(시야)
    public float sight { get { return _sight; } set { _sight = value; } }
    public float insight { get { return _insight; } set { _insight = value; } }
    public float outsight { get { return _outsight; } set { _outsight = value; } }

    // 이동속도
    public float speed { get { return _speed; } set { _speed = value; } }
    public float inspeed { get { return _inspeed; } set { _inspeed = value; } }
    public float outspeed { get { return _outspeed; } set { _outspeed = value; } }


    public float dig_speed { get { return _dig_speed; } set { _dig_speed = value; } }
    public float crawlup_speed { get { return _crawlup_speed; } set { _crawlup_speed = value; } }



    public float oxygen { get { return _oxygen; } set { _oxygen = value; } }
    public float max_oxygen { get { return _max_oxygen; } set { _max_oxygen = value; } }
    public float min_oxygen { get { return _min_oxygen; } set { _min_oxygen = value; } }
    public float hungry { get { return _hungry; } set { _hungry = value; } }
    public float max_hungry { get { return _max_hungry; } set { _max_hungry = value; } }
    public float min_hungry { get { return _min_hungry; } set { _min_hungry = value; } }


    public float hp_recovery { get { return _hp_recovery; } set { _hp_recovery = value; } }
    public float oxygen_recovery { get { return _oxygen_recovery; } set { _oxygen_recovery = value; } }
    public float oxygen_consume { get { return _oxygen_consume; } set { _oxygen_consume = value; } }
    public float hungry_recovery { get { return _hungry_recovery; } set { _hungry_recovery = value; } }
    public float hungry_consume { get { return _hungry_consume; } set { _hungry_consume = value; } }


    public Status copy()
    {
        return (Status)this.MemberwiseClone();
    }
}
