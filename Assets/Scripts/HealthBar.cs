using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   public Slider healthBarSlider;

   public void GiveFullHealth(float health)
   {
      healthBarSlider.maxValue = health;  // ✅ maxValue (minúscula)
      healthBarSlider.value = health;     // ✅ value (minúscula)
   }

   public void SetHealth(float health)
   {
      healthBarSlider.value = health;     // ✅ Correcto
   }
}
