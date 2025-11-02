using System;
using UnityEngine;
using UnityEngine.UI; // 👈 nuevo namespace para usar Text UI

namespace UnityStandardAssets.Utility
{
    public class SimpleActivatorMenu : MonoBehaviour
    {
        // Simple menú que cambia entre varios objetos (por ejemplo cámaras)
        public Text camSwitchButton; // 👈 reemplazo de GUIText
        public GameObject[] objects;

        private int currentActiveObject;

        private void OnEnable()
        {
            // El objeto activo comienza siendo el primero del array
            currentActiveObject = 0;
            if (camSwitchButton != null && objects.Length > 0)
                camSwitchButton.text = objects[currentActiveObject].name;
        }

        public void NextCamera()
        {
            if (objects == null || objects.Length == 0)
                return;

            int nextActiveObject = currentActiveObject + 1 >= objects.Length ? 0 : currentActiveObject + 1;

            // Activa sólo el siguiente objeto y desactiva los demás
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(i == nextActiveObject);
            }

            currentActiveObject = nextActiveObject;

            if (camSwitchButton != null)
                camSwitchButton.text = objects[currentActiveObject].name;
        }
    }
}