using UnityEngine;

public class Objects : MonoBehaviour
{
    public float objectHealth = 100;

    public void objectHitDamage(float amount)
    {
        objectHealth -= amount;
       
        if (objectHealth <= 0f)
        {
            //destroy
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
