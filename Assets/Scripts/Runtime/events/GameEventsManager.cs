using Nomtec.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomtec
{
    public static class GameEventsManager
    {
        // GAME MANAGER
        public static event Action<GameState> onGameStateChanged;
        public static void InvokeGameStateChanged(GameState newState) => onGameStateChanged?.Invoke(newState);


        // HIDEABLE PANEL
        public static event Action onGUIDisplayed;
        public static void InvokeGUIDisplayed() => onGUIDisplayed?.Invoke();

        public static event Action onGUIHidden;
        public static void InvokeGUIHidden() => onGUIHidden?.Invoke();


        // BUTTONS
        public static event Action<ISpawnableButton> onButtonSelected;
        public static void InvokeButtonSelected(ISpawnableButton button) => onButtonSelected?.Invoke(button);


        // OBJECTS
        public static event Action<IEatable> onEatablePlaced, onEatableConsumed;
        public static void InvokeEateablePlaced(IEatable eatableObject) => onEatablePlaced?.Invoke(eatableObject); 
        public static void InvokeEatableConsumed(IEatable eatableObject) => onEatableConsumed?.Invoke(eatableObject);

    }
}
