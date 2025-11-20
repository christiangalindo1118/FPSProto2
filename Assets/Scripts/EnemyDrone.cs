using UnityEngine;

public class EnemyDrone : MonoBehaviour
{
   [Header("Enemy Health and Damage")] 
    private float enemyHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public HealthBar healthBar;
    
    [Header("Enemy Things")] 
    public UnityEngine.AI.NavMeshAgent enemyAgent;
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
    public GameObject shootingEffect;
    
   //[Header("Enemy Animations")] 
    public Animator anim;
    public ParticleSystem muzzleSpark;
    public ParticleSystem muzzleFlame;

    [Header("Enemy mood/situation")] 
    public float visionRadius = 15f;
    public float shootingRadius = 8f;
    private float defaultVisionRadius = 15f;
    private float defaultShootingRadius = 8f;
    
    public bool playerInVisionRadius;
    public bool playerInShootingRadius;
    
    private bool isDead = false;
    private string currentState = ""; // Para controlar cambios de estado

    private void Awake()
    {
        // Verificar walkPoints
        if (walkPoints == null || walkPoints.Length == 0)
        {
            Debug.LogError("ERROR: walkPoints está vacío!");
            enabled = false;
            return;
        }
        
        // Buscar jugador
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj == null)
        {
            Debug.LogError("ERROR: No se encontró el Player!");
            enabled = false;
            return;
        }
        healthBar.GiveFullHealth(enemyHealth);
        playerBody = playerObj.transform;
        
        // Obtener NavMeshAgent
        enemyAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (enemyAgent == null)
        {
            Debug.LogError("ERROR: No hay NavMeshAgent!");
            enabled = false;
            return;
        }
        
        // Verificar Animator
        if (anim == null)
        {
            Debug.LogWarning("ADVERTENCIA: No hay Animator asignado!");
        }
        
        // Verificar ShootingRaycastArea
        if (ShootingRaycastArea == null)
        {
            Debug.LogError("ERROR: ShootingRaycastArea no está asignado!");
        }
        
        // Verificar LookPoint
        if (LookPoint == null)
        {
            Debug.LogWarning("ADVERTENCIA: LookPoint no está asignado, usando playerBody");
            LookPoint = playerBody;
        }
        
        enemyAgent.speed = enemySpeed;
        defaultVisionRadius = visionRadius;
        defaultShootingRadius = shootingRadius;
        presentHealth = enemyHealth;
    }

    private void Start()
    {
        // Verificar que está en NavMesh
        if (!enemyAgent.isOnNavMesh)
        {
            Debug.LogError("ERROR: Enemy NO está en NavMesh!");
        }
        
        Debug.Log("Enemy configurado correctamente");
        VerificarAnimator();
    }
    
    private void VerificarAnimator()
    {
        if (anim == null)
        {
            Debug.LogError("❌ ANIMATOR ES NULL!");
            return;
        }
        
        if (anim.runtimeAnimatorController == null)
        {
            Debug.LogError("❌ ANIMATOR NO TIENE CONTROLLER ASIGNADO!");
            return;
        }
        
        Debug.Log("✅ Animator encontrado: " + anim.runtimeAnimatorController.name);
        Debug.Log("📋 PARÁMETROS DISPONIBLES:");
        
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            Debug.Log($"   - {param.name} (Tipo: {param.type})");
        }
        
        // Verificar parámetros necesarios
        string[] parametrosNecesarios = { "Walk", "AimRun", "Shoot", "Die" };
        foreach (string nombreParam in parametrosNecesarios)
        {
            bool existe = false;
            foreach (AnimatorControllerParameter param in anim.parameters)
            {
                if (param.name == nombreParam)
                {
                    existe = true;
                    Debug.Log($"✅ Parámetro '{nombreParam}' encontrado");
                    break;
                }
            }
            
            if (!existe)
            {
                Debug.LogError($"❌ FALTA PARÁMETRO: '{nombreParam}'");
            }
        }
    }

    private void Update()
    {
        // Si está muerto, no hacer nada
        if (isDead) return;
        
        // CORRECCIÓN: Usar los radios ORIGINALES para detectar, no los expandidos
        playerInVisionRadius = Physics.CheckSphere(transform.position, defaultVisionRadius, PlayerLayer);
        playerInShootingRadius = Physics.CheckSphere(transform.position, defaultShootingRadius, PlayerLayer);

        // Determinar estado y cambiar solo cuando sea necesario
        string newState = "";
        
        if (!playerInVisionRadius && !playerInShootingRadius)
        {
            newState = "Guard";
            if (currentState != newState)
            {
                Debug.Log("🔄 Cambiando a: GUARD");
                currentState = newState;
            }
            Guard();
        }
        else if (playerInVisionRadius && !playerInShootingRadius)
        {
            newState = "Pursue";
            if (currentState != newState)
            {
                Debug.Log("🔄 Cambiando a: PURSUE");
                currentState = newState;
            }
            Pursueplayer();
        }
        else if (playerInShootingRadius)
        {
            newState = "Shoot";
            if (currentState != newState)
            {
                Debug.Log("🔄 Cambiando a: SHOOT");
                currentState = newState;
            }
            ShootPlayer();
        }
    }

    private void Guard()
    {
        // CORRECCIÓN: Solo cambiar animación si no está ya en Guard
        if (anim != null && currentState == "Guard")
        {
            anim.SetBool("Walk", true);
            anim.SetBool("AimRun", false);
            anim.SetBool("Die", false);
        }
        
        // NO modificar los radios aquí - mantener valores por defecto
        
        // Verificar que el agente puede moverse
        if (!enemyAgent.enabled || !enemyAgent.isOnNavMesh)
        {
            return;
        }
        
        // Si llegó al punto, cambiar al siguiente
        float distanceToWalkPoint = Vector3.Distance(walkPoints[currentEnemyPosition].transform.position, transform.position);
        
        if (distanceToWalkPoint < walkingpointRadius)
        {
            currentEnemyPosition++;
            if (currentEnemyPosition >= walkPoints.Length)
            {
                currentEnemyPosition = 0;
            }
        }
       
        enemyAgent.SetDestination(walkPoints[currentEnemyPosition].transform.position);
    }

    private void Pursueplayer()
    {
        // CORRECCIÓN: Solo cambiar animación al entrar en este estado
        if (anim != null && currentState == "Pursue")
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", true);
            anim.SetBool("Die", false);
        }
        
        // NO expandir radios - usar valores por defecto
        
        if (enemyAgent.enabled && enemyAgent.isOnNavMesh)
        {
            enemyAgent.SetDestination(playerBody.position);
        }
    }
    
    private void ShootPlayer()
    {
        // Detenerse
        if (enemyAgent.enabled && enemyAgent.isOnNavMesh)
        {
            enemyAgent.SetDestination(transform.position);
        }
        
        // Mirar al jugador
        if (LookPoint != null)
        {
            transform.LookAt(LookPoint);
        }
        
        // CORRECCIÓN: Solo cambiar animación al entrar en este estado
        if (anim != null && currentState == "Shoot")
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", false);
            anim.SetBool("Die", false);
        }
        
        if (!previouslyShoot)
        {
            // Reproducir efecto de disparo
            if (muzzleSpark != null)
            {
                muzzleSpark.Play();
            }
            
            // Activar trigger de disparo
            if (anim != null)
            {
                anim.SetTrigger("Shoot");
                Debug.Log("💥 Disparando!");
            }
            
            if (ShootingRaycastArea == null)
            {
                Debug.LogError("ShootingRaycastArea es NULL!");
                return;
            }
            
            RaycastHit hit;
            Vector3 rayOrigin = ShootingRaycastArea.transform.position;
            Vector3 rayDirection = ShootingRaycastArea.transform.forward;
            
            // Debug: Visualizar el raycast
            Debug.DrawRay(rayOrigin, rayDirection * defaultShootingRadius, Color.red, 0.5f);
            
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, defaultShootingRadius))
            {
                Debug.Log("🎯 Disparo impactó en: " + hit.transform.name);
                
                // Activar efecto de disparo si existe
                if (shootingEffect != null)
                {
                    Instantiate(shootingEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
                
                // Buscar PlayerScript en el objeto golpeado o sus padres
                PlayerScript playerScript = hit.transform.GetComponent<PlayerScript>();
                if (playerScript == null)
                {
                    playerScript = hit.transform.GetComponentInParent<PlayerScript>();
                }
                
                if (playerScript != null)
                {
                    Debug.Log("💔 ¡Daño aplicado al jugador: " + giveDamage);
                    playerScript.playerHitDamage(giveDamage);
                }
            }
            
            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timebtwShoot);
        }
    }

    private void ActiveShooting()
    {
        previouslyShoot = false;
    }

    public void enemyDroneHitDamage(float takeDamage)
    {
        if (isDead) return;
        
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);
        Debug.Log($"💔 Enemy recibió daño: {takeDamage} | Vida restante: {presentHealth}/{enemyHealth}");

        if (presentHealth <= 0)
        {
            enemyDie();
        }
    }

    private void enemyDie()
    {
        if (isDead) return;
        
        isDead = true;
        Debug.Log("☠️ Enemy muriendo");
        
        // Activar animación de muerte (Bool)
        if (anim != null)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", false);
            anim.SetBool("Die", true);
        }
        
        // Detener el agente
        if (enemyAgent.enabled && enemyAgent.isOnNavMesh)
        {
            enemyAgent.SetDestination(transform.position);
            enemyAgent.enabled = false;
        }
        
        // Desactivar detección
        playerInVisionRadius = false;
        playerInShootingRadius = false;
        
        // Destruir después de 5 segundos
        Object.Destroy(gameObject, 5.0f);
    }
    
    // Visualizar los radios de detección en el editor
    private void OnDrawGizmosSelected()
    {
        // Radio de visión (amarillo)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, defaultVisionRadius);
        
        // Radio de disparo (rojo)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, defaultShootingRadius);
        
        // Línea hacia el jugador si está detectado
        if (Application.isPlaying && playerBody != null && (playerInVisionRadius || playerInShootingRadius))
        {
            Gizmos.color = playerInShootingRadius ? Color.red : Color.yellow;
            Gizmos.DrawLine(transform.position, playerBody.position);
        }
    }
}
