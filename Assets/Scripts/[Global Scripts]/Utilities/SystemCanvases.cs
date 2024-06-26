using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CGames
{
    public class SystemCanvases : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        [ContextMenu("Attach camera to all child canvases")]
        private void OnInspectorGUI()
        {
            List<Canvas> canvases = this.transform.GetComponentsInChildren<Canvas>().ToList();

            foreach (Canvas canvas in canvases)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = mainCamera;
            }
        }
    }
}