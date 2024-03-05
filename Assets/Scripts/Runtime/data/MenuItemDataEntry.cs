using System;

namespace Nomtec.Data
{
    [Serializable]
    public class MenuItemDataEntry<T>
    {
        public string key;
        public T value;
    }
}
