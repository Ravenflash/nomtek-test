using Ravenflash.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec.Logic
{
    public enum GameState { Selection, Placement }

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameState _state;
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

        private void OnEnable()
        {
            GameEventsManager.onButtonSelected += HandleButtonSelected;
        }

        private void OnDisable()
        {
            GameEventsManager.onButtonSelected -= HandleButtonSelected;
        }

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                State = GameState.Selection;
            }
        }

        private void HandleButtonSelected(ISpawnableButton button)
        {
            State = GameState.Placement;
        }

    }
}
