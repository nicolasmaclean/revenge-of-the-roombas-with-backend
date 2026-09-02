using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        internal EnemyBase enemyPrefab;

        [SerializeField]
        internal float initialDelay = 0;

        [SerializeField]
        internal Vector2 waitInterval = new Vector2(4, 8);

        void OnEnable()
        {
            StartCoroutine(SpawnLoop());
        }

        void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator SpawnLoop()
        {
            yield return new WaitForSeconds(initialDelay);

            while (true)
            {
                SpawnEnemy();

                float waitTime = Random.Range(waitInterval.x, waitInterval.y);
                yield return new WaitForSeconds(waitTime);
            }
        }

        void SpawnEnemy()
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform);
        }
    }
}