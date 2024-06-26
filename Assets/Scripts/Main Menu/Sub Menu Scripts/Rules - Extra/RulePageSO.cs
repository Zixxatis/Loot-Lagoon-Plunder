using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    [CreateAssetMenu(fileName = "Rule Page", menuName = "Scriptable Objects/Rules/Rule Page", order = 0)]
    public class RulePageSO : ScriptableObject, ILocalizable
    {
        [field: SerializeField] public int PageIndex { get; private set; }
        [field: Space]
        [field: SerializeField] public LocalizationKeyField DescriptionKey { get; private set; }
        [field: Space]
        [field: SerializeField] public Sprite RuleFirstSprite { get; private set; }
        [field: SerializeField] public Sprite RuleSecondSprite { get; private set; }

        public List<LocalizationKeyField> GetAllLocalizationKeys()
        {
            return new()
            {
                DescriptionKey
            };
        }
    }
}