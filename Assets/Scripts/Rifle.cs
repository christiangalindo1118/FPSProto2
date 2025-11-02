using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")] 
    public Camera camera;
    public float giveDamageOf = 10f;

    public float shootingRange = 100f;
    
    //[Header("Rifle Ammunition and shooting")]
    
    //[Header("Rifle Effects")]
    
    //[Header("Sounds and UI)]
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);
            
            Objects objects = hitInfo.transform.GetComponent<Objects>();

            if (objects != null)
            {
                objects.objectHitDamage(giveDamageOf);
            }
        }
    }
}
