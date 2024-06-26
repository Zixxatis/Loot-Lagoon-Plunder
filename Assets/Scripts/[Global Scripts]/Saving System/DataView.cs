using System.IO;
using UnityEngine;

namespace CGames
{
    /// <summary> This class should inherited by any class that will hold DW (data view) for Saving / Loading and based on "Data" class. </summary>
    /// <typeparam name="T"> Type of "Data" class. </typeparam>
    public abstract class DataView<T>
    {
        protected abstract string LogPrefix { get; }
        protected abstract string FileName { get; }
        protected string FullPath => Path.Combine(Application.persistentDataPath, FileName);
        protected abstract T NewData { get; }

        protected ResourceSystem resourceSystem;
        
        public DataView(MonoProxy monoProxy, ResourceSystem resourceSystem)
        {
            this.resourceSystem = resourceSystem;

            LoadData();

            monoProxy.OnAppQuit.AddListener(OnApplicationQuit);
        }

        public void LoadData()
        {
            T loadedData;

            if (File.Exists(FullPath))
            {
                loadedData = FileHandler.Load<T>(FullPath);

                if (loadedData != null)
                {
                    ApplyData(loadedData);
                    Debug.Log($"[{LogPrefix}] <color=#43BF0D>Loaded.</color>");
                    return;
                }
                else
                    Debug.LogError($"[{LogPrefix}] Got a null data file.");
            }

            ApplyData(NewData);
            Debug.LogWarning($"[{LogPrefix}] No data file found. Creating a new one.");
        }
        protected abstract void ApplyData(T data);

        public void SaveData()
        {
            if(SavingNotification.Instance != null)
                SavingNotification.Instance.ShowNotification();

            FileHandler.Save(ReadAllData(), FullPath);
            Debug.Log($"[{LogPrefix}] <color=#E67D12>Saved.</color>");
        }
        protected abstract T ReadAllData();

        protected void OnApplicationQuit()
        {
            if(ConfigData.IsDeveloperMode)
            {
                FileHandler.Save(ReadAllData(), FullPath);
                Debug.Log($"[DEV / {LogPrefix}] <color=#E67D12>Saved.</color>");
            }
        }
    }
}