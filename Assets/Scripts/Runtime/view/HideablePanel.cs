using DG.Tweening;
using Nomtec.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec.View
{
    public class HideablePanel : MonoBehaviour
    {
        [SerializeField] private float _enterTimeSeconds = 1f, _exitTimeSeconds = 1f;
        [SerializeField] private Vector2 _hiddenPosition;
        private Vector2 _displayedPosition;

        private void Start()
        {
            _displayedPosition = transform.position;
        }

        private void OnEnable()
        {
            GameEventsManager.onGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            GameEventsManager.onGameStateChanged -= HandleGameStateChanged;
        }

        private void Show()
        {
            transform.DOMove(_displayedPosition, _enterTimeSeconds).SetEase(Ease.OutCubic).OnComplete(() => GameEventsManager.InvokeGUIDisplayed());
        }

        private void Hide()
        {
            transform.DOMove(_hiddenPosition, _exitTimeSeconds).SetEase(Ease.InBack).OnComplete(() => GameEventsManager.InvokeGUIHidden()); ;
        }

        private void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Selection: Show(); break;
                case GameState.Placement: Hide(); break;
                default: break;
            }
        }
    }
}
