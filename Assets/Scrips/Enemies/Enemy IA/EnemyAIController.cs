using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    [Header("NavMesh")]
    public NavMeshAgent agent;

    [Header("Vivo")]
    public EnemyHealt enemylife;

    [Header("FOV (Field of View)")]
    public float radiusView = 15f; //radio de deteccion tanto de vista como de TargetTracker
    public float angleView = 90f;
    public LayerMask targetMask;     // Capa de objetivos como jugador, señuelos, estructuras, etc.
    public LayerMask obstacleMask;   // Capa de obstáculos que bloquean la visión
    [HideInInspector] public bool canCurrentlySeePlayer = false;

    [Header("Objetivo")]
    public Transform player;
    public Transform objeto;

    [Header("Disparo")]
    public Transform barrel; // Punto de disparo

    [Header("Estados")]
    // Estado actual y estados concretos
    private EnemyState currentState;
    public PatrolState patrolState;
    public SeekCoverState seekCoverState;
    public ChaseState chaseState;
    public ShootState shootState;
    public InvestigateState investigateState;
    public AttackObjectiveState attackObjectiveState;

    [Header("Patrol Variables")]
    [HideInInspector] public float patrolDefaultSpeed = 3.5f;//Velocidad Defaul del agente
    [HideInInspector] public float patrolAlertSpeed = 1.5f;//VElocidad en alerta
    [HideInInspector] public float patrolRun = 4.5f;//Velocidad al correr

    [Header("Alert")]
    public bool isAlerted = false;


    [Header("Cover Settings")]
    public CoverPoint currentCoverPoint;
    [HideInInspector] public float alertDecisionDelay = 1.5f;
    [HideInInspector] public float actionProbability = 0.5f;
    public bool hasMadeCoverDecision = false;

    [Header("Chase Settings")]
    public float minChaseDistance = 7f;
    public float maxChaseDistance = 15f;
    public bool tope = false;

    [Header("Target System")]
    // Sistema de prioridad de objetivos encapsulado-------------preguntar como funciona
    public TargetTracker targetTracker;
    public TargetData currentTarget => targetTracker?.currentTarget;

    public float targetDetectionRadius = 50f;


    void Start()
    {
        // Inicializar estados
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        shootState = new ShootState(this);
        seekCoverState = new SeekCoverState(this);
        investigateState = new InvestigateState(this);
        attackObjectiveState = new AttackObjectiveState(this);

        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Inicializar el TargetTracker
        targetTracker = new TargetTracker(transform, targetDetectionRadius, obstacleMask);

        //espera un poco para que los targets sean registrados en el TargetManager
        //StartCoroutine(DelayedStartLogic());

        targetTracker.UpdateTarget();

        if (currentTarget != null)
        {
            Debug.Log("Target encontrado: " + currentTarget.targetTransform);
        }
        else{
            Debug.Log("Target no encontrado"+currentTarget);
        }

        if (currentTarget != null && currentTarget.priority >= 4)
        {
            TransitionToState(attackObjectiveState);
        }
        else
        {
            TransitionToState(patrolState);
        }

        // Iniciar la rutina de visión
        StartCoroutine(FOVRoutine());

    }

    private IEnumerator DelayedStartLogic()
    {
        // Esperar 1 frame para que los TargetObject se registren
        yield return new WaitForSeconds(1f);

        targetTracker.UpdateTarget();

        if (currentTarget != null)
        {
            Debug.Log("Target encontrado: " + currentTarget.targetTransform);
        }
        else{
            Debug.Log("Target no encontrado"+currentTarget);
        }

        if (currentTarget != null && currentTarget.priority >= 4)
        {
            TransitionToState(attackObjectiveState);
        }
        else
        {
            TransitionToState(patrolState);
        }
    }

    void Update()
    {
        //si el enemigo esta vivo hace cosas
        if (enemylife.life)
        {
            // Actualiza objetivo actual con prioridades dinámicas
            targetTracker.UpdateTarget();

            // Actualiza lógica del estado actual
            currentState.Update();

            // Actualiza si el objetivo actual está dentro del campo de visión
            canCurrentlySeePlayer = CheckFieldOfView();

            if (isAlerted && !tope)
            {
                radiusView = 25;
                angleView = 120;
                //targetTracker.detectionRadius = radiusView;
                StartCoroutine(AlertDecisionRoutine());
                tope = true;
            }
        }
    }

    //rutina para estar viendo al jugador
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);
        while (true)
        {
            yield return delay;
            canCurrentlySeePlayer = CheckFieldOfView();
        }
    }

    //funcion que es el punto de vista del enemigo
    private bool CheckFieldOfView()
    {
        if (currentTarget == null || currentTarget.targetTransform == null)
            return false;
        if (currentTarget.priority == 0)
        {
            return false;
        }

        Vector3 directionToTarget = (currentTarget.targetTransform.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.targetTransform.position);

        // Verifica que el objetivo esté dentro del ángulo de visión
        if (distanceToTarget <= radiusView && Vector3.Angle(transform.forward, directionToTarget) < angleView / 2f)
        {
            // Verifica que no haya obstáculos
            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
            {
                isAlerted = true;
                AlertNearbyEnemiesToPlayer(currentTarget.targetTransform.position, 40f);
                LookAt(currentTarget.targetTransform.position);
                return true;
            }
        }

        return false;
    }

    //funcion para rotar hacia donde ve el jugador
    public void LookAt(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        direction.y = 0; // Evitar inclinación vertical
        transform.rotation = Quaternion.LookRotation(direction);
    }

    //funcion para transicionar a otro estado
    public void TransitionToState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private IEnumerator AlertDecisionRoutine()
    {
        yield return new WaitForSeconds(alertDecisionDelay);

        hasMadeCoverDecision = true;
    }

    public void AlertNearbyEnemies(Vector3 deathPosition, float alertRadius)
    {
        //la esfera devuelde todos los collider en su volument por eso se usa Collider
        Collider[] colliders = Physics.OverlapSphere(deathPosition, alertRadius);

        foreach (var collider in colliders)
        {
            EnemyAIController enemy = collider.GetComponent<EnemyAIController>();

            if (enemy != null && enemy.isActiveAndEnabled)
            {
                //no se alerta a si mismo si el enemigo y la posicion de murte estan muy cerca
                if (Vector3.Distance(enemy.transform.position, deathPosition) > 0.1f)
                {
                    enemy.BecomeAlert();
                }
            }
        }
    }

    private void AlertNearbyEnemiesToPlayer(Vector3 playerPosition, float alertRadius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, alertRadius);
        foreach (Collider collider in colliders)
        {
            EnemyAIController enemy = collider.GetComponent<EnemyAIController>();
            if (enemy != null && enemy != this) // que no se alerte a sí mismo
            {
                enemy.BecomeAlert();

                // También puede mirar hacia el jugador
                enemy.LookAt(playerPosition);
            }
        }
    }

    public void BecomeAlert()
{
    if (!isAlerted)
    {
        isAlerted = true;

        // Reinicia patrullaje si estaba patrullando
        if (currentState == patrolState)
        {
            TransitionToState(new PatrolState(this));
        }
    }
}
}