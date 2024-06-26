using System;
using System.Collections.Generic;

namespace CGames
{
    public class PlayerAttributes : CharacterAttributes
    {
        public const float MoveSpeedUpgradeIncrease = 1.5f;
        public const float JumpForceUpgradeIncrease = 1.5f;
        public const int LesserHealthUpgradeAmount = 25;
        public const int GreaterHealthUpgradeAmount = 75;
        public const int DamageUpgradeAmount = 5;
        public const int VampiricUpgradeRestoreAmount = 1;
        public const int ChanceForCriticalInPercents = 15;
        public const int RegenerationAmount = 10;

        private readonly Action<int> scheduleRegenerationAtTheEndOfTheLevel;

        public PlayerAttributes(CharacterConfig baseCharacterConfig, Dictionary<PlayerUpgradeType, bool> playerUpgradesDictionary, Action<int> scheduleRegenerationAtTheEndOfTheLevel) 
                                : base(baseCharacterConfig)
        {
            this.scheduleRegenerationAtTheEndOfTheLevel = scheduleRegenerationAtTheEndOfTheLevel;

            for (int i = 0; i < playerUpgradesDictionary.Count; i++)
            {
                PlayerUpgradeType upgradeType = (PlayerUpgradeType)i;

                if(playerUpgradesDictionary[upgradeType])
                    EnableUpgrade(upgradeType);
            }
        }

        public void EnableUpgrade(PlayerUpgradeType upgradeType)
        {
            switch (upgradeType)
            {
                case PlayerUpgradeType.MoveSpeed:
                    MovementConfig.IncreaseMoveSpeed(MoveSpeedUpgradeIncrease);
                    break;

                case PlayerUpgradeType.Jump:
                    AirborneConfig.IncreaseJumpForce(JumpForceUpgradeIncrease);
                    break;

                case PlayerUpgradeType.Endurance:
                    HealthSystemConfig.IncreaseMaxHealth(LesserHealthUpgradeAmount);
                    break;

                case PlayerUpgradeType.Armor:
                    HealthSystemConfig.IncreaseMaxHealth(GreaterHealthUpgradeAmount);
                    break;

                case PlayerUpgradeType.Sword:
                    AttackConfigsList.ForEach(x => x.IncreaseDamage(DamageUpgradeAmount));
                    break;

                case PlayerUpgradeType.Vampiric:
                    AttackConfigsList.ForEach(x => x.SetVampiricAmount(VampiricUpgradeRestoreAmount));
                    break;

                case PlayerUpgradeType.Critical:
                    AttackConfigsList.ForEach(x => x.SetCriticalChange(ChanceForCriticalInPercents));
                    break;

                case PlayerUpgradeType.Regeneration:
                    scheduleRegenerationAtTheEndOfTheLevel.Invoke(RegenerationAmount);
                    break;

                case PlayerUpgradeType.Midas:
                    // ? Used directly in PlayerDataView
                    break;
                    
                default:
                    throw new NotSupportedException();
            }
        }
    }
}