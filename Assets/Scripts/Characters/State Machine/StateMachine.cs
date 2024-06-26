using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public class StateMachine : IStateSwitcher
    {
        private List<IState> statesList;
        private IState currentState;

        public void Initialize(List<IState> statesList)
        {
            this.statesList = statesList;

            currentState = this.statesList[0];
            currentState.Enter();
        }

        public void SwitchState<T>() where T : IState
        {
            IState newState = statesList.Find(x => x is T);

            if(newState == null)
            {
                Debug.LogWarning($"Tried to enter a \"{typeof(T)}\" state, but it doesn't exists in the list. Won't enter to this state.");
                return;
            }

            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void Update() => currentState.Update();
    }
}
