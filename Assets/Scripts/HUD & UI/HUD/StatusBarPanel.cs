using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class StatusBarPanel : MonoBehaviour
    {
        [SerializeField, Min(0)] private float distanceFromEdge = 50f;
        [Space]
        [SerializeField] private Image statusBarImagePrefab;
        
        private RectTransform rectTransform;
        private PlayerDataView playerDV;
        private Func<List<PlayerUpgradeSO>> getAllUpgradesList;
        private Dictionary<PlayerUpgradeSO, Image> statusBarImagesDictionary;

        [Inject]
        private void Construct(PlayerDataView playerDataView, ResourceSystem resourceSystem)
        {
            playerDV = playerDataView;
            getAllUpgradesList = () => resourceSystem.PlayerUpgradesList;
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            SetStatusBarIcons();
        }

        private void Start() => DisplayInitialUpgrades();

        private void SetStatusBarIcons()
        {
            statusBarImagesDictionary = new();

            List<PlayerUpgradeSO> upgradesListByRarity = getAllUpgradesList().OrderBy(x => x.UpgradeRarity).ToList();
            
            foreach (PlayerUpgradeSO playerUpgradeSO in upgradesListByRarity)
            {
                Image icon = Instantiate(statusBarImagePrefab, this.gameObject.transform);
                icon.sprite = playerUpgradeSO.IconSprite;

                statusBarImagesDictionary.Add(playerUpgradeSO, icon);
            }
        }

        private void DisplayInitialUpgrades()
        {
            for (int i = 0; i < playerDV.UpgradesDictionary.Count; i++)
            {
                statusBarImagesDictionary.First(x => x.Key.UpgradeType == (PlayerUpgradeType)i).Value
                                     .ChangeGameObjectActivation(playerDV.UpgradesDictionary[(PlayerUpgradeType)i]);
            }

            UpdateStatusBarObjectVisibility();
        }

        public void DisplayNewUpgrade(PlayerUpgradeType playerUpgradeType)
        {
            statusBarImagesDictionary.First(x => x.Key.UpgradeType == playerUpgradeType).Value
                                     .ChangeGameObjectActivation(playerDV.UpgradesDictionary[playerUpgradeType]);
           
            UpdateStatusBarObjectVisibility();
        }
        
        private void UpdateStatusBarObjectVisibility()
        {
            if (playerDV.HasAllUpgradesLocked())
            {
                this.gameObject.DeactivateObject();
                return;
            }

            this.gameObject.ActivateObject();
            StartCoroutine(ChangePanelPosition());
        }
        
        private IEnumerator ChangePanelPosition()
        {
            yield return null;

            Vector2 newPosition = new(-(rectTransform.sizeDelta.x / 2 + distanceFromEdge), rectTransform.anchoredPosition.y);
        
            rectTransform.anchoredPosition = newPosition;
        }
    }
}