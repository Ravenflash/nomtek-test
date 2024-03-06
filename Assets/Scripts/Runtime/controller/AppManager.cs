using Ravenflash.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec.Logic
{
    public class AppManager : Singleton<AppManager>
    {
        [SerializeField, InterfaceType(typeof(IGameManager))] 
        private MonoBehaviour _game;
        public IGameManager Game
        {
            get
            {
                if (_game is null) _game = new GameObject().AddComponent<GameManager>();
                return _game as IGameManager;
            }
        }
    }
}
