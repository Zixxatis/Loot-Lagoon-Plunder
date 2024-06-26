using UnityEngine;

namespace CGames
{
    public class ShipMerchant : InteractableObject
    {
        [Header("Ship Merchant Elements")]
        [SerializeField] private MerchantPanel merchantPanel;

        protected override bool ShouldHandleCursorVisibility => true;

        protected override void Awake()
        {
            base.Awake();

            merchantPanel.Initialize(() => RevertInteractionWithObject());
        }

        protected override void Interact()
        {
            Time.timeScale = 0;
            merchantPanel.ShowPanel();
        }

        protected override void RevertInteraction()
        {
            base.RevertInteraction();

            Time.timeScale = 1;
        }
    }
}