using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec.View
{

    public class ScreenRaycaster : MonoBehaviour, IRaycaster
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _defaultDistance = 5f;
        [SerializeField] private float _maxDistance = 100f;

        [SerializeField] private Camera _camera;
        private Camera Camera { get { if (!_camera) _camera = Camera.main; return _camera; } }

        public bool HitTest(out Vector3 hitPoint)
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            // Perform the raycast
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, _maxDistance, _layerMask))
            {
                hitPoint = hitInfo.point;
                return true;
            }
            else
            {
                // Return some kind of a default value
                hitPoint = _defaultDistance * ray.direction + ray.origin; // Vector3.Zero
                return false;
            }
        }
    }
}
