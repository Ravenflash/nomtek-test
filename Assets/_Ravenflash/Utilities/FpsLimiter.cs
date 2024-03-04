using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenflash.Utilities
{
    public class FpsLimiter : MonoBehaviour
    {
        public enum FpsLimitValue { _12=12, _25=25, _30=30, _50=50, _60=60, _120=120, _140=140 }

        [SerializeField] FpsLimitValue fpsLimit = FpsLimitValue._60;

        // Start is called before the first frame update
        void Start()
        {
            SetFPSLimit(fpsLimit);
        }

        public void SetFPSLimit(FpsLimitValue value)
        {
            fpsLimit = value;
            Application.targetFrameRate = (int)fpsLimit;
        }

    }
}
