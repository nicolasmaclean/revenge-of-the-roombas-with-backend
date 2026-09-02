using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility.ObjectPool;

namespace Game.Weapons
{
    public class BulletPool : ObjectPoolBase<Bullet>
    {
        public static BulletPool CreatePool(Bullet prefab)
        {
            GameObject instance = new GameObject("OP_EnemyPool");

            BulletPool pool = instance.AddComponent<BulletPool>();
            pool.prefab = prefab;

            return pool;
        }
    }
}