using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec
{
    public class CubeConsumer : MonoBehaviour
    {
        public bool consumeOnCollision = true;

        private void OnCollisionEnter(Collision collision)
        {
            if (!consumeOnCollision) return;

            IEatable eatable;
            if (collision.gameObject.TryGetComponent<IEatable>(out eatable))
            {
                eatable.Consume();
            }
        }
    }
}
