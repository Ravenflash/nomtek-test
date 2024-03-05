using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec.Data
{
    [CreateAssetMenu(fileName = "Menu Items", menuName = "Nomtek/new Menu Item Collection", order = 20)]
    public class ItemMenuCollectionData : ScriptableObject
    {
        public List<MenuItemDataEntry<SpawnableObjectData>> items;
    }
}
