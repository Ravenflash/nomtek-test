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

        private List<ISpawnableButton> _spawnableButtons = new List<ISpawnableButton>();

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

        private void RenderMenu()
        {
            ISpawnableButton btn;

            try
            {
                _spawnableButtons.Clear();
                foreach (SpawnableItemData itemData in dataCollection.items)
                {
                    btn = Instantiate(_buttonPrefab, _layout) as ISpawnableButton;
                    btn.Initialize(itemData);
                    _spawnableButtons.Add(btn);
                }
            }
            catch(Exception e) { Debug.LogException(e); }

        }

        private void SetInteractive(bool status)
        {
            foreach(ISpawnableButton btn in  _spawnableButtons)
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
