using Nomtec.Data;
using Nomtec.View;
using Ravenflash.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Nomtec.Logic
{
    public enum GameState { Selection, Placement }

    public class GameManager : MonoBehaviour, IGameManager
    {
        private GameState _state = GameState.Selection;

        private ISpawnable _selectedItem;
        private Coroutine _updateCoroutine;

        public List<IEatable> EatableObjects { get; private set; } = new List<IEatable>();

        [SerializeField, InterfaceType(typeof(IRaycaster))]
        private MonoBehaviour _raycaster;
        private IRaycaster Raycaster
        {
            get
            {
                if (!_raycaster) _raycaster = GetComponent<IRaycaster>() as MonoBehaviour;
                if (!_raycaster) _raycaster = Camera.main.GetComponent<IRaycaster>() as MonoBehaviour;
                return _raycaster as IRaycaster;
            }
        }

        public GameState State
        {
            get => _state;
            private set
            {
                // Invalid states
                if (_state == value) return;

                // Valid state
                _state = value;
                GameEventsManager.InvokeGameStateChanged(_state);
            }
        }

        public IObjectPoolManager<MonoBehaviour> PoolManager { get; set; } = new ObjectPoolManager<MonoBehaviour>();

        private void Start()
        {
            if (Raycaster is null) Debug.LogWarning($"{name} is missing a Raycaster reference!");
            InitSelectionMode();

        }

        private void OnEnable()
        {
            GameEventsManager.onButtonSelected += HandleButtonSelected;
            GameEventsManager.onEatablePlaced += HandleEatablePlaced;
            GameEventsManager.onEatableConsumed += HandleEatableConsumed;
        }

        private void OnDisable()
        {
            GameEventsManager.onButtonSelected -= HandleButtonSelected;
            GameEventsManager.onEatablePlaced -= HandleEatablePlaced;
            GameEventsManager.onEatableConsumed -= HandleEatableConsumed;
        }

        IEnumerator SelectionUpdate()
        {
            while (this && State == GameState.Placement)
            {
                // Nothing to do here so far
                // Left for future implementation
                // Remove this Coroutine if not needed
                yield return null;
            }
        }

        IEnumerator PlacementUpdate()
        {
            Vector3 newPosition;
            bool isValidPosition;

            Raycaster.HitTest(out newPosition);
            _selectedItem.transform.position = newPosition;

            while (this && _selectedItem is object && State == GameState.Placement)
            {
                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    InitSelectionMode();
                    yield break;
                }
                isValidPosition = Raycaster.HitTest(out newPosition);
                _selectedItem.Rigidbody.position = newPosition;

                if (isValidPosition && Input.GetMouseButtonDown(0))
                {
                    _selectedItem.Place(newPosition);
                    _selectedItem = null;
                    InitSelectionMode();
                    yield break;
                }

                yield return null;
            }
        }

        private void InitPlacementMode(ISpawnable spawnable)
        {
            // Clean up
            if (_updateCoroutine is object) StopCoroutine(_updateCoroutine);

            _selectedItem = spawnable.SpawnCopy();

            // Camera Direction
            _selectedItem.transform.forward = Vector3.back;

            // New state
            State = GameState.Placement;
            _updateCoroutine = StartCoroutine(PlacementUpdate());
        }

        private void InitSelectionMode()
        {
            // Clean up
            if (_updateCoroutine is object) StopCoroutine(_updateCoroutine);
            if (_selectedItem is object) _selectedItem.Despawn();

            // New state
            State = GameState.Selection;
            _updateCoroutine = StartCoroutine(SelectionUpdate());
        }

        private void HandleButtonSelected(ISpawnableButton button)
        {
            InitPlacementMode(button.Data.SpawnableObject);
        }

        private void HandleEatablePlaced(IEatable eatable)
        {
            EatableObjects.Add(eatable);
        }

        private void HandleEatableConsumed(IEatable eatable)
        {
            EatableObjects.Remove(eatable);
        }

    }
}
