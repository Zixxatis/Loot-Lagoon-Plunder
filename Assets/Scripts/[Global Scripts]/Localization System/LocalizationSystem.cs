using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class LocalizationSystem : IInitializable, IDisposable
    {
        private List<TextLocalizer> savingSystemObjects;

        private readonly ConfigDataView configDataView; 
        private readonly SceneLoader sceneLoader;

        public LocalizationSystem(ConfigDataView configDataView, SceneLoader sceneLoader)
        {
            this.configDataView = configDataView;
            this.sceneLoader = sceneLoader;

            LocalizationDictionary.Construct(() => configDataView.Language);
            LocalizationDictionary.Initialize();
        }

        public void Initialize()
        {
            configDataView.OnLocalizationChanged += UpdateAllLocalizations;
            sceneLoader.OnLoadingFinished += ClearLocalizationFieldsReferences;
        }

        public void UpdateAllLocalizations()
        {
            savingSystemObjects ??= UnityEngine.Object.FindObjectsOfType<MonoBehaviour>(true).OfType<TextLocalizer>().ToList();

            foreach (TextLocalizer script in savingSystemObjects)
            {
                script.UpdateText();
            }
        }

        private void ClearLocalizationFieldsReferences() => savingSystemObjects = null;

        public void Dispose()
        {
            configDataView.OnLocalizationChanged -= UpdateAllLocalizations;
            sceneLoader.OnLoadingFinished -= ClearLocalizationFieldsReferences;
        }
    }

    public enum Language
    {
        English,
        Russian,
    }
}