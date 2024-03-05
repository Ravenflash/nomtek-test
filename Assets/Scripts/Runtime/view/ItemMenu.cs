using DG.Tweening;
using Nomtec.Data;
using Nomtec.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec.View
{
    public class ItemMenu : MonoBehaviour
    {
        [SerializeField, InterfaceType(typeof(ISpawnableButton))]
        private MonoBehaviour _buttonPrefab;

        [SerializeField] private ItemMenuCollectionData dataCollection;
        [SerializeField] private Transform _layout;

        public List<ISpawnableButton> SpawnableButtons { get; } = new List<ISpawnableButton>();

        private void Start()
        {
            RenderMenu();
        }

        private void OnEnable()
        {
            GameEventsManager.onGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            GameEventsManager.onGameStateChanged -= HandleGameStateChanged;
        }

        public void Filter(string searchString)
        {
            searchString = searchString.Trim().ToLower();

            foreach (ISpawnableButton btn in SpawnableButtons)
            {
                btn.gameObject.SetActive(searchString.Length<=0 || btn.Title.ToLower().Contains(searchString));
            }
        }

        private void RenderMenu()
        {
            ISpawnableButton btn;

            try
            {
                SpawnableButtons.Clear();
                foreach (MenuItemDataEntry<SpawnableObjectData> item in dataCollection.items)
                {
                    btn = Instantiate(_buttonPrefab, _layout) as ISpawnableButton;
                    btn.Initialize(item.key, item.value);
                    SpawnableButtons.Add(btn);
                }
            }
            catch (Exception e) { Debug.LogException(e); }

        }

        private void SetInteractive(bool status)
        {
            foreach (ISpawnableButton btn in SpawnableButtons)
            {
                btn.SetInteractive(status);
            }
        }

        private void HandleGameStateChanged(GameState state)
        {
            SetInteractive(state == GameState.Selection);
        }
    }
}
