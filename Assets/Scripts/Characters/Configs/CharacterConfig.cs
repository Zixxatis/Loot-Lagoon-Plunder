using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    [CreateAssetMenu(fileName = "Character Config", menuName = "Scriptable Objects/Configs/Character Config", order = 0)]
    public class CharacterConfig : ScriptableObject
    {
        [field: Header("Character Config")]
        [field: SerializeField] public HealthSystemConfig HealthSystemConfig { get; private set; }
        [field:Space]
        [field: SerializeField] public MovementConfig MovementConfig { get; private set; }
        [field: SerializeField] public AirborneConfig AirborneConfig { get; private set; }
        [field:Space]
        [field: SerializeField] public  List<AttackConfig> AttackConfigsList { get; private set; }
    }
}