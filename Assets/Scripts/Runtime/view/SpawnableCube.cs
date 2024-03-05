using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec.View
{
    public class SpawnableCube : MonoBehaviour, ISpawnable, IEatable
    {


        private Rigidbody _rb;
        public Rigidbody Rigidbody { get { if (!_rb) _rb = GetComponent<Rigidbody>(); return _rb; } }

        public bool isConsumed { get; private set; } = false;

        public ISpawnable SpawnCopy()
        {
            ISpawnable copy = Instantiate(this);
            copy.Rigidbody.isKinematic = true;
            return copy;
        }
        public void Despawn()
        {
            Destroy(gameObject);
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
