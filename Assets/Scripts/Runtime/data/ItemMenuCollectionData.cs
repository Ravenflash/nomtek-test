using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Nomtec.Data
{
    [CreateAssetMenu(fileName = "Menu Items", menuName = "Nomtek/new Menu Item Collection", order = 20)]
    public class ItemMenuCollectionData : ScriptableObject
    {
        public List<SpawnableItemData> items;
    }
}
