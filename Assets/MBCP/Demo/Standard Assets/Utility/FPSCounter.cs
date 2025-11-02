using System;
using UnityEngine;
using UnityEngine.UI; // 👈 necesario para usar Text

namespace UnityStandardAssets.Utility
{
    public class FPSCounter : MonoBehaviour
    {
        const float fpsMeasurePeriod = 0.5f;
        private int fpsAccumulator = 0;
        private float fpsNextPeriod = 0;
        private int currentFps;
        const string display = "{0} FPS";

        private Text uiText; // 👈 reemplazo de GUIText

        private void Start()
        {
            fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            uiText = GetComponent<Text>(); // 👈 busca el componente Text del Canvas

            if (uiText == null)
                Debug.LogError("FPSCounter: No se encontró componente Text en este objeto. Añádelo desde UI → Text.");
        }

        private void Update()
        {
            fpsAccumulator++;

            if (Time.realtimeSinceStartup > fpsNextPeriod)
            {
                currentFps = (int)(fpsAccumulator / fpsMeasurePeriod);
                fpsAccumulator = 0;
                fpsNextPeriod += fpsMeasurePeriod;

                if (uiText != null)
                    uiText.text = string.Format(display, currentFps);
            }
        }
    }
}

