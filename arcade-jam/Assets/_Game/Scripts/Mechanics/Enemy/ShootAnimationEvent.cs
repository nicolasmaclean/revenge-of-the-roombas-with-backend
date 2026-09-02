using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ShootAnimationEvent : MonoBehaviour
    {
        [SerializeField]
        Enemy.EnemySniper sniper;

        public void Shoot()
        {
            sniper.Shoot();
        }
    }
}