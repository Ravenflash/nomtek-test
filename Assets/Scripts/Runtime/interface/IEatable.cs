using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec
{
    public interface IEatable
    {
        Transform transform { get; }
        bool isConsumed { get; }
        void Consume();
    }
}
