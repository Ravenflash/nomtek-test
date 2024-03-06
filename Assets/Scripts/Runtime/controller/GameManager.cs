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

        #region Properties
        public List<IEatable> EatableObjects { get; private set; } = new List<IEatable>();

        [SerializeField, InterfaceType(typeof(IInputController))]
        private MonoBehaviour _inputController;
        private IInputController InputController
        {
            get
            {
                if (_inputController is null) _inputController = GetComponentInChildren<IInputController>() as MonoBehaviour;
                if (_inputController is null) _inputController = gameObject.AddComponent<PlayerInputController>();
                return _inputController as IInputController;
            }
        }

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
        #endregion

        #region Unity Methods
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

        private void OnDestroy()
        {
            ExitPlacementMode();
        }
        #endregion

        #region Placement Mode
        private void InitPlacementMode(ISpawnable spawnable)
        {
            // Clean up
            if (_updateCoroutine is object) StopCoroutine(_updateCoroutine);

            //Events
            InputController.onCancel += HandlePlacementCancel;
            InputController.onClick += HandlePlacementClick;

            // Spawn object
            _selectedItem = spawnable.SpawnCopy();
            _selectedItem.transform.forward = Vector3.back;

            // New state
            State = GameState.Placement;
            _updateCoroutine = StartCoroutine(PlacementUpdate());
        }

        private IEnumerator PlacementUpdate()
        {
            Vector3 newPosition;

            Raycaster.HitTest(out newPosition);
            _selectedItem.transform.position = newPosition;

            while (this && _selectedItem is object && State == GameState.Placement)
            {
                Raycaster.HitTest(out newPosition);
                _selectedItem.Rigidbody.position = newPosition;

                yield return null;
            }
        }

        private void HandlePlacementClick()
        {
            if (State != GameState.Placement) return;
            if (_selectedItem is null) return;

            Vector3 clickPosition;
            if(Raycaster.HitTest(out clickPosition))
            {
                _selectedItem.Place(clickPosition);
                _selectedItem = null;
                ExitPlacementMode();
                InitSelectionMode();
            }
        }

        private void HandlePlacementCancel()
        {
            ExitPlacementMode();
            InitSelectionMode();
        }

        private void ExitPlacementMode()
        {
            if(State != GameState.Placement) return;

            StopCoroutine(_updateCoroutine);
            InputController.onCancel -= HandlePlacementCancel;
            InputController.onClick -= HandlePlacementClick;
        }
        #endregion

        #region Selection Mode
        private void InitSelectionMode()
        {
            // Clean up
            if (_updateCoroutine is object) StopCoroutine(_updateCoroutine);
            if (_selectedItem is object) _selectedItem.Despawn();

            // New state
            State = GameState.Selection;
            _updateCoroutine = StartCoroutine(SelectionUpdate());
        }

        private IEnumerator SelectionUpdate()
        {
            while (this && State == GameState.Placement)
            {
                // Nothing to do here so far
                // Left for future implementation
                // Remove this Coroutine if not needed
                yield return null;
            }
        }
        #endregion

        #region Other Event Handlers
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
        #endregion

    }
}
