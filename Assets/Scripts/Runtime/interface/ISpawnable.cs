using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec
{
    public interface ISpawnable
    {
        public Transform transform { get; }
        public GameObject gameObject { get; }
        public Rigidbody Rigidbody { get; }
        public ISpawnable SpawnCopy();
        public void Despawn();
        public void Place(Vector3 spawnPoint);
    }
}
