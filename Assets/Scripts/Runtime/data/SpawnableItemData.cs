using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec.Data
{
    [CreateAssetMenu(fileName = "Spawnable Item Data", menuName = "Nomtek/new Spawnable Item Data", order = 20)]
    public class SpawnableItemData : ScriptableObject
    {
        public string title;
        public Sprite thumbnail;
        public ISpawnable spawnableItem;

        
    }
}
