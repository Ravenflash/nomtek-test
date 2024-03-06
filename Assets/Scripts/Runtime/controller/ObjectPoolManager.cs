using Nomtec.View;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace Nomtec
{
    public class ObjectPoolManager<T> : IObjectPoolManager<T> where T : MonoBehaviour
    {
        public Dictionary<int, ObjectPool<T>> ObjectPools { get; } = new Dictionary<int, ObjectPool<T>>();

        public T GetObject(T prefab)
        {
            ObjectPool<T> pool;
            int key = prefab.GetHashCode();

            if (!TryGetPool(key, out pool))
            {
                pool = CreatePool(prefab);
                ObjectPools.Add(key, pool);
            }

            return pool.Get();
        }

        private bool TryGetPool(int key, out ObjectPool<T> pool)
        {
            return ObjectPools.TryGetValue(key, out pool);
        }

        private ObjectPool<T> CreatePool(T prefab)
        {
            Debug.Log($"Creating new object pool {prefab.name}, key: {prefab.GetHashCode()}");
            ObjectPool<T> pool = new ObjectPool<T>(prefab);
            return pool;
        }

        public bool ReturnToPool(int key, T obj)
        {
            ObjectPool<T> pool;
            if (TryGetPool(key, out pool))
            {
                pool.ReturnToPool(obj);
                return true;
            }
            else return false;

        }
    }
}
