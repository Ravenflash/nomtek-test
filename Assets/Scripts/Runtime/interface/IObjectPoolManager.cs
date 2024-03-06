using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec
{
    public interface IObjectPoolManager<T>
    {
        T GetObject(T prefab);
        bool ReturnToPool(int key, T obj);
    }
}
