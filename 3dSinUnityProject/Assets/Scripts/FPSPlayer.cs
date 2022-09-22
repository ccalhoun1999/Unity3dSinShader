using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FPSPlayer : MonoBehaviour
{
    [SerializeField]
    private float _hp;
    public float HealthPoints
    {
        get
        {
            return _hp;
        }
        private set
        {
            _hp = value;
        }
    }

    public void TakeDamage(float damage)
    {
        HealthPoints -= damage;
        if (HealthPoints < 0)
        {
            HealthPoints = 0;
        }
        Debug.Log(HealthPoints);
    }
}
