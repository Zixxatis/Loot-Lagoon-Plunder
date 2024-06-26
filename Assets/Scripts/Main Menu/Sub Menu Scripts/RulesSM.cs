using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class RulesSM : SubMenu
    {
        [Header("Rules - Elements")]
        [SerializeField] private Image rulesFirstImage;
        [SerializeField] private  Image rulesSecondImage;
        [Space]
        [SerializeField] private TextLocalizer descriptionLTMP;
        [Space]
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        
        private List<RulePageSO> rulePagesList;
        private int currentPageIndex;

        [Inject]
        private void Construct(ResourceSystem resourceSystem)
        {
            rulePagesList = resourceSystem.RulePagesList;
        }

        protected override void Awake()
        {
            base.Awake();

            leftButton.onClick.AddListener(ShowPreviousPage);
            rightButton.onClick.AddListener(ShowNextPage);
        }

        private void ShowNextPage()
        {
            currentPageIndex++;
            UpdatePageInfo();
        }

        private void ShowPreviousPage()
        {
            currentPageIndex--;
            UpdatePageInfo();
        }

        public override void PrepareForOpening()
        {
            currentPageIndex = 0;
            UpdatePageInfo();
        }

        private void UpdatePageInfo()
        {
            RulePageSO rulePage = rulePagesList[currentPageIndex];

            rulesFirstImage.sprite = rulePage.RuleFirstSprite;
            rulesSecondImage.sprite = rulePage.RuleSecondSprite;
            descriptionLTMP.SetKeyAndUpdate(rulePage.DescriptionKey);

            HandleNavigationButtons();
        }

        private void HandleNavigationButtons()
        {
            leftButton.interactable = currentPageIndex != 0;
            rightButton.interactable = currentPageIndex != rulePagesList.Count -1;

            if(currentPageIndex == 0)
                rightButton.Select();

            if(currentPageIndex == rulePagesList.Count -1)
                leftButton.Select();
        }

        public override void PrepareForClosing() { }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            leftButton.onClick.RemoveListener(ShowPreviousPage);
            rightButton.onClick.RemoveListener(ShowNextPage);
        }
    }
}