using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Health and Damage")] 
    private float enemyHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public HealthBar healthBar;
    
    [Header("Enemy Things")] 
    public NavMeshAgent enemyAgent;
    public Transform LookPoint;
    public Camera ShootingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;

    [Header("Enemy Guarding Var")] 
    public GameObject[] walkPoints;
    private int currentEnemyPosition = 0;
    public float enemySpeed = 3.5f;
    private float walkingpointRadius = 2;

    [Header("Enemy Shooting Var")] 
    public float timebtwShoot = 2f;
    private bool previouslyShoot;
    
    [Header("Enemy Animations")] 
    public Animator anim;
    public ParticleSystem muzzleSpark;

    [Header("Enemy mood/situation")] 
    public float visionRadius = 15f;
    public float shootingRadius = 8f;
    public bool playerInVisionRadius;
    public bool playerInShootingRadius;
    
    // CAMBIO 1: Variable para evitar repetir estados
    private string lastState = "";

    private void Awake()
    {
        presentHealth = enemyHealth;
        healthBar.GiveFullHealth(enemyHealth);
        playerBody = GameObject.Find("Player").transform;
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position, shootingRadius, PlayerLayer);

        // CAMBIO 2: Determinar estado solo una vez
        string currentState = "";
        
        if(!playerInVisionRadius && !playerInShootingRadius)
        {
            currentState = "Guard";
            Guard();
        }
        else if(playerInVisionRadius && !playerInShootingRadius)
        {
            currentState = "Pursue";
            Pursueplayer();
        }
        else if(playerInShootingRadius)
        {
            currentState = "Shoot";
            ShootPlayer();
        }
        
        // CAMBIO 3: Log solo cuando cambia de estado
        if (currentState != lastState && currentState != "")
        {
            Debug.Log($"🔄 Estado: {currentState}");
            lastState = currentState;
        }
    }

    private void Guard()
    {
        // CAMBIO 4: Establecer animaciones solo al entrar en Guard
        if (lastState != "Guard" && anim != null)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("AimRun", false);
            anim.SetBool("Shoot", false);
            anim.SetBool("Die", false);
        }
        
        if(Vector3.Distance(walkPoints[currentEnemyPosition].transform.position, transform.position) < walkingpointRadius)
        {
            currentEnemyPosition = Random.Range(0, walkPoints.Length);
            if(currentEnemyPosition >= walkPoints.Length)
            {
                currentEnemyPosition = 0;
            }
        }
        
        transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentEnemyPosition].transform.position, Time.deltaTime * enemySpeed);
        transform.LookAt(walkPoints[currentEnemyPosition].transform.position);
    }

    private void Pursueplayer()
    {
        // CAMBIO 5: Establecer animaciones solo al entrar en Pursue
        if (lastState != "Pursue" && anim != null)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", true);
            anim.SetBool("Shoot", false);
            anim.SetBool("Die", false);
        }
        
        enemyAgent.SetDestination(playerBody.position);
        transform.LookAt(LookPoint);
    }
    
    private void ShootPlayer()
    {
        enemyAgent.SetDestination(transform.position);
        transform.LookAt(LookPoint);
        
        if(!previouslyShoot)
        {
            if (muzzleSpark != null)
            {
                muzzleSpark.Play();
            }
            
            // CAMBIO 6: Activar animación de disparo aquí
            if (anim != null)
            {
                anim.SetBool("Walk", false);
                anim.SetBool("AimRun", false);
                anim.SetBool("Shoot", true);
                anim.SetBool("Die", false);
            }
            
            if (ShootingRaycastArea == null)
            {
                Debug.LogError("❌ ShootingRaycastArea es NULL!");
                return;
            }
            
            RaycastHit hit;
            Vector3 rayOrigin = ShootingRaycastArea.transform.position;
            Vector3 rayDirection = ShootingRaycastArea.transform.forward;
            
            Debug.DrawRay(rayOrigin, rayDirection * shootingRadius, Color.red, 1f);
            
            if(Physics.Raycast(rayOrigin, rayDirection, out hit, shootingRadius))
            {
                Debug.Log($"🎯 Golpeó: {hit.transform.name}");
                
                // CAMBIO 7: Buscar PlayerScript en objeto Y en padres
                PlayerScript playerScript = hit.transform.GetComponent<PlayerScript>();
                if (playerScript == null)
                {
                    playerScript = hit.transform.GetComponentInParent<PlayerScript>();
                }
                
                if(playerScript != null)
                {
                    Debug.Log($"💥 Daño aplicado: {giveDamage}");
                    playerScript.playerHitDamage(giveDamage);
                }
                else
                {
                    Debug.LogWarning($"⚠️ '{hit.transform.name}' no tiene PlayerScript");
                }
            }
            
            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timebtwShoot);
        }
    }

    private void ActiveShooting()
    {
        previouslyShoot = false;
        
        // CAMBIO 8: Resetear animación de disparo
        if (anim != null)
        {
            anim.SetBool("Shoot", false);
        }
    }

    public void enemyHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);
        Debug.Log($"Enemy vida: {presentHealth}/{enemyHealth}");
        
        if(presentHealth <= 0)
        {
            if (anim != null)
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Shoot", false);
                anim.SetBool("AimRun", false);
                anim.SetBool("Die", true);
            }
            
            enemyDie();
        }
    }

    private void enemyDie()
    {
        enemyAgent.SetDestination(transform.position);
        enemySpeed = 0f;
        shootingRadius = 0f;
        visionRadius = 0f;
        playerInVisionRadius = false;
        playerInShootingRadius = false;
        Object.Destroy(gameObject, 5.0f);
    }
}