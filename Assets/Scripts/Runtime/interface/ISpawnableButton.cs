using Nomtec.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nomtec
{
    public interface ISpawnableButton
    {
        public GameObject gameObject { get; }
        public string Title { get; }
        public SpawnableObjectData Data { get; }
        public bool Initialize(string title, SpawnableObjectData data);
        public void SetInteractive(bool status);
    }
}
