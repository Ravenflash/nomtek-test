using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec
{
    public interface IRaycaster
    {
        bool HitTest(out Vector3 hitPoint);
    }
}
