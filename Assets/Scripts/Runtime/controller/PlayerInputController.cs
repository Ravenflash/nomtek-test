using PlasticPipe.PlasticProtocol.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec.Logic
{
    public class PlayerInputController : MonoBehaviour, IInputController
    {
        public event Action onClick;
        public event Action onCancel;
        Coroutine inputUpdate;

        private void OnEnable()
        {
            if (inputUpdate is object) StopCoroutine(inputUpdate);
            inputUpdate = StartCoroutine(UpdateInput());
        }

        private void OnDisable()
        {
            if (inputUpdate is object) StopCoroutine(inputUpdate);
        }

        IEnumerator UpdateInput()
        {
            while(this)
            {
                if (Input.GetKeyUp(KeyCode.Escape))
                    onCancel?.Invoke();

                if (Input.GetMouseButtonDown(0))
                    onClick?.Invoke();

                yield return null;
            }


        }
    }
}
