using Nomtec.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec.View
{
    public class SpawnableCube : MonoBehaviour, ISpawnable, IEatable
    {
        private int poolHashCode;
        private Rigidbody _rb;
        public Rigidbody Rigidbody { get { if (!_rb) _rb = GetComponent<Rigidbody>(); return _rb; } }

        public bool isConsumed { get; private set; } = false;

        public ISpawnable SpawnCopy()
        {
            SpawnableCube copy;

            // Pooling
            try
            {
                int key = GetHashCode();

                ObjectPool<MonoBehaviour> pool;
                if (!GameManager.Instance.ObjectPools.TryGetValue(key, out pool))
                {
                    Debug.Log($"Creating new object pool {name}, key: {key}");
                    pool = new ObjectPool<MonoBehaviour>(this);
                    GameManager.Instance.ObjectPools.Add(key, pool);
                }
                copy = pool.Get() as SpawnableCube;
                copy.poolHashCode = key;
            }
            catch
            {
                copy = Instantiate(this);
            }

            copy.Rigidbody.isKinematic = true;
            return copy;
        }

        public void Despawn()
        {
            try
            {
                Rigidbody.angularVelocity = Vector3.zero;
                Rigidbody.velocity = Vector3.zero;
                transform.rotation = Quaternion.identity;
                GameManager.Instance.ObjectPools[poolHashCode].ReturnToPool(this);
            }
            catch
            {
                Destroy(gameObject);
            }

        }

        public void Place(Vector3 spawnPoint)
        {
            Rigidbody.isKinematic = false;
            GameEventsManager.InvokeEateablePlaced(this);
        }

        public void Consume()
        {
            isConsumed = true;
            GameEventsManager.InvokeEatableConsumed(this);
            Despawn();
        }
    }
}
