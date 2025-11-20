
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")] 
    public Camera camera;
    public float giveDamageOf = 10f;

    public float shootingRange = 100f;

    public float fireCharge = 15f;
    
    public Animator animator;
    public PlayerScript player;

    [Header("Rifle Ammunition and shooting")]
    private int maximunAmmunition = 20;

    private int mag = 1;
    private int presentAmunition;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;
    private float nextTimeToShoot = 0f;
    
    
    [Header("Rifle Effects")] 
    public ParticleSystem muzzleSpark;

    public GameObject impactEffect;
    public GameObject goreEffect;
    public GameObject droneEffect;

    [Header("Sounds and UI")]
            [SerializeField] private GameObject AmmoOutUI;    
    [SerializeField] private int timeToShowUI = 1;

    private void Awake()
    {
        presentAmunition = maximunAmmunition;
    }
    // Update is called once per frame
    void Update()
    {
        if (setReloading)
            return;
        if (presentAmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
            
            nextTimeToShoot = Time.time + 1f / fireCharge;
            
            Shoot();
        }
        else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }
        else if (Input.GetButton("Fire2") && Input.GetButton("Fire1"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false); 
        }
        else
        {
            animator.SetBool("Fire", false);
            animator.SetBool("Idle", true);
            animator.SetBool("FireWalk", false);
            animator.SetBool("Reloading", false);
        }
    }

   void Shoot()
{
    if (mag == 0)
    {
        StartCoroutine(ShowAmmoOut());
        return;
    }
    
    presentAmunition--;

    if (presentAmunition == 0)
    {
        mag--;
    }
    
    // ✅ ACTUALIZAR UI - CORRECTO
    if (AmmoCount.ocurrence != null)
    {
        AmmoCount.ocurrence.UpdateAmmoText(presentAmunition);
        AmmoCount.ocurrence.UpdateMagText(mag);
    }
    else
    {
        Debug.LogError("❌ AmmoCount.ocurrence es NULL!");
    }
    
    muzzleSpark.Play();
    RaycastHit hitInfo;

    if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo, shootingRange))
    {
        Debug.Log(hitInfo.transform.name);
        
        Objects objects = hitInfo.transform.GetComponent<Objects>();
        if (objects == null) objects = hitInfo.transform.GetComponentInParent<Objects>();
        
        Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
        if (enemy == null) enemy = hitInfo.transform.GetComponentInParent<Enemy>();
        
        EnemyDrone enemyDrone = hitInfo.transform.GetComponent<EnemyDrone>();
        if (enemyDrone == null) enemyDrone = hitInfo.transform.GetComponentInParent<EnemyDrone>();

        if (objects != null)
        {
            objects.objectHitDamage(giveDamageOf);
            if (impactEffect != null)
            {
                GameObject impactGO = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGO, 1f);
            }
        }
        else if (enemy != null)
        {
            enemy.enemyHitDamage(giveDamageOf);
            if (goreEffect != null)
            {
                GameObject impactGO = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGO, 2f);
            }
        }
        else if (enemyDrone != null)
        {
            enemyDrone.enemyDroneHitDamage(giveDamageOf);
            if (droneEffect != null)
            {
                GameObject impactGO = Instantiate(droneEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGO, 1f);
            }
        }
    }
}

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        presentAmunition = maximunAmmunition;
        player.playerSpeed = 1.9f;
        player.playerSprint = 3;
        setReloading = false;
    }

    IEnumerator ShowAmmoOut()
    {
        AmmoOutUI.SetActive(true);
        yield return new WaitForSeconds(timeToShowUI);
        AmmoOutUI.SetActive(false);
    }
}
