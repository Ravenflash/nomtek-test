using Nomtec.Data;
using TMPro;
using UnityEngine.UI;

namespace Nomtec
{
    public interface ISpawnableButton
    {
        public SpawnableItemData Data { get; }
        public bool Initialize(SpawnableItemData data);
        public void SetInteractive(bool status);
    }
}
