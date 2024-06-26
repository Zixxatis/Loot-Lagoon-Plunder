using UnityEngine;
using UnityEngine.Events;

namespace CGames
{
    /// <summary> This script is used for injection to non-mono classes to use coroutines, etc. </summary>
    public class MonoProxy : MonoBehaviour 
    {
        /// <summary> Subscribe to this event to be able to invoke OnApplicationQuit. </summary>
        /// <remarks> This is self-unsubscribable, you don't have to unsubscribe manually. </remarks> 
        public UnityEvent OnAppQuit;

        private void OnApplicationQuit()
        {
            OnAppQuit.Invoke();
            OnAppQuit.RemoveAllListeners();
        }
    }
}