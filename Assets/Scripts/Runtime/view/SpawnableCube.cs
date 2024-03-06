using Nomtec.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Nomtec.View
{
    public class SpawnableCube : MonoBehaviour, ISpawnable, IEatable
    {
        private int poolKey;
        private Rigidbody _rb;
        public Rigidbody Rigidbody { get { if (!_rb) _rb = GetComponent<Rigidbody>(); return _rb; } }

        public bool isConsumed { get; private set; } = false;

        IObjectPoolManager<MonoBehaviour> PoolManager => AppManager.Instance.Game.PoolManager;

        public ISpawnable SpawnCopy()
        {
            SpawnableCube copy;

            // Pooling
            try
            {
                copy = PoolManager.GetObject(this) as SpawnableCube;
                copy.poolKey = GetHashCode();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                copy = Instantiate(this);
            }

            // Default Configuration
            try
            {
                copy.Rigidbody.isKinematic = true;
                copy.transform.rotation = Quaternion.identity;
            }
            catch (Exception e) { Debug.LogException(e); }

            return copy;
        }

        public void Despawn()
        {
            try
            {
                Rigidbody.angularVelocity = Vector3.zero;
                Rigidbody.velocity = Vector3.zero;
                transform.rotation = Quaternion.identity;

                if (!PoolManager.ReturnToPool(poolKey, this)) Destroy(gameObject);
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
