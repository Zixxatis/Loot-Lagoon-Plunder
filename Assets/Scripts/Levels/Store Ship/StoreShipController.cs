using System;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class StoreShipController : MonoBehaviour, ILosableLevelController
    {
        private Action saveDataAction;


        [Inject]
        private void Construct(PlayerDataView playerDV)
        {
            this.saveDataAction = playerDV.SaveData;
        }

        private void Start()
        {
            saveDataAction?.Invoke();

            PlayerInputHandler.EnablePlayerControls();
            PlayerInputHandler.EnablePauseListener();
        }
        
        public void SetAsLost(){ }
    }
}