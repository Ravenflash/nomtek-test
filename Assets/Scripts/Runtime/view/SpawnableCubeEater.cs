using Nomtec.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec
{
    public class SpawnableCubeEater : MonoBehaviour, ISpawnable
    {
        [SerializeField] private float moveSpeed = 1f, rotationSpeed = 360f;
        private IEatable eatableTarget;
        private Coroutine _eatingCoroutine;
        private WaitForFixedUpdate nextFixedUpdate = new WaitForFixedUpdate();

        private Rigidbody _rb;
        public Rigidbody Rigidbody { get { if (!_rb) _rb = GetComponent<Rigidbody>(); return _rb; } }

        [SerializeField] private CubeConsumer _consumer;
        private CubeConsumer CubeConsumer { get { if (!_consumer) _consumer = GetComponentInChildren<CubeConsumer>(); return _consumer; } }


        List<IEatable> EatableObjects => AppManager.Instance.Game.EatableObjects;

        private void Start()
        {
            Rigidbody.isKinematic = true;
            Rigidbody.rotation = Quaternion.Euler(0, 180f, 0);
            if (CubeConsumer) CubeConsumer.consumeOnCollision = false;
        }

        public ISpawnable SpawnCopy()
        {
            ISpawnable copy = Instantiate(this);
            return copy;
        }

        public void Despawn()
        {
            GameEventsManager.onEatablePlaced -= HandleEatablePlaced;
            Destroy(gameObject);
        }

        public void Place(Vector3 spawnPoint)
        {
            GameEventsManager.onEatablePlaced += HandleEatablePlaced;
            StartEating();
        }

        private void StartEating()
        {
            if (_eatingCoroutine is object) StopCoroutine(_eatingCoroutine);
            _eatingCoroutine = StartCoroutine(Eating());
        }

        private IEnumerator Eating()
        {
            if (!Rigidbody) yield break;
            if (!transform) yield break;

            if (CubeConsumer) CubeConsumer.consumeOnCollision = true;

            eatableTarget = GetNearestEatable(eatableTarget);
            Vector3 delta, direction;

            while (EatableObjects.Count > 0)
            {
                if (eatableTarget.isConsumed) eatableTarget = GetNearestEatable();

                delta = eatableTarget.transform.position - Rigidbody.position;
                direction = delta.normalized;

                Rigidbody.MovePosition(transform.forward * moveSpeed * Time.fixedDeltaTime + Rigidbody.position);
                Rigidbody.MoveRotation(Quaternion.RotateTowards(Rigidbody.rotation, Quaternion.LookRotation(direction, Vector3.up), rotationSpeed * Time.fixedDeltaTime));

                yield return nextFixedUpdate;
            }
        }

        private IEatable GetNearestEatable(IEatable currentTarget = null)
        {
            try
            {
                if (EatableObjects.Count <= 0) return null;

                IEatable result = currentTarget is null || currentTarget.isConsumed ? EatableObjects[0] : currentTarget;
                float min_distance = Vector3.Distance(result.transform.position, transform.position);
                float distance;

                foreach (IEatable item in EatableObjects)
                {
                    if (item == result) continue;

                    distance = Vector3.Distance(item.transform.position, transform.position);
                    if (distance < min_distance)
                    {
                        min_distance = distance;
                        result = item;
                    }
                }

                return result;
            }
            catch { return null; }
        }

        private void HandleEatablePlaced(IEatable eatable)
        {
            StartEating();
        }
    }
}
