using System;
using UnityEngine;
using UnityEngine.UI; // 👈 nuevo namespace
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement; // 👈 reemplaza Application.LoadLevelAsync

[RequireComponent(typeof(Image))] // 👈 reemplazo de GUITexture
public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        // Si se presiona el botón "ResetObject"...
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            // ... recarga la escena actual
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }
}