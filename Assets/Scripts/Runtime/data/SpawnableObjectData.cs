using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec.Data
{
    [CreateAssetMenu(fileName = "Spawnable Item Data", menuName = "Nomtek/new Spawnable Item Data", order = 20)]
    public class SpawnableObjectData : ScriptableObject
    {
        public Sprite thumbnail;

        [SerializeField, InterfaceType(typeof(ISpawnable))]
        MonoBehaviour _spawnableObject;
        public ISpawnable SpawnableObject => _spawnableObject as ISpawnable;

    }
}
