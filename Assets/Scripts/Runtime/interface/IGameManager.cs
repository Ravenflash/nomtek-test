using Nomtec.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec
{
    public interface IGameManager
    {
        IObjectPoolManager<MonoBehaviour> PoolManager { get; set; }
        GameState State { get; }
        List<IEatable> EatableObjects { get; }


    }
}
