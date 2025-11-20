using UnityEngine;
using TMPro;   // ← Importante

public class AmmoCount : MonoBehaviour
{
    [Header("UI Text References")]
    public TMP_Text ammunitionText;
    public TMP_Text magText;

    public static AmmoCount ocurrence;

    private void Awake()
    {
        ocurrence = this;
      
        // Verificaciones
        if (ammunitionText == null)
            Debug.LogError("❌ ammunitionText NO está asignado en el Inspector!");

        if (magText == null)
            Debug.LogError("❌ magText NO está asignado en el Inspector!");
    }

    // Actualizar munición actual
    public void UpdateAmmoText(int presentAmunition)
    {
        if (ammunitionText != null)
        {
            ammunitionText.text = "Ammo: " + presentAmunition;
        }
        else
        {
            Debug.LogError("❌ ammunitionText es NULL!");
        }
    }

    // Actualizar cantidad de cargadores
    public void UpdateMagText(int mag)
    {
        if (magText != null)
        {
            magText.text = "Magazines: " + mag;
        }
        else
        {
            Debug.LogError("❌ magText es NULL!");
        }
    }

    // Actualizar ambos
    public void UpdateAllAmmoText(int presentAmunition, int mag)
    {
        UpdateAmmoText(presentAmunition);
        UpdateMagText(mag);
    }
}