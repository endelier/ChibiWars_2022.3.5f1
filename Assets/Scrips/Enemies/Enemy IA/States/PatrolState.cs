using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyState
{
    private Vector3 patrolTarget; //Vector3 de direccion al punto al que irá
    private float patrolRadius = 15f; // radio para patrullar

    //Cuando alguien cree un PatrolState, debe pasarme un EnemyAIController. Yo lo voy a enviar a mi clase base EnemyState para que también lo tenga.
    public PatrolState(EnemyAIController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.stoppingDistance = 0f;
        // Busca un punto aleatorio al entrar
        patrolTarget = GetRandomPoint();
        enemy.agent.SetDestination(patrolTarget);
    }

    //Actualiza el patrullaje del enemigo
    public override void Update()
    {
        //si el enemigo esta alerta su radio de patrulla y velocidad se reduce 
        if (enemy.isAlerted)
        {
            patrolRadius = 12.5f;
            enemy.agent.speed = enemy.patrolAlertSpeed;
        }

        // Si llegó al punto de patrullaje, busca un nuevo punto de patrullaje Y NO ESTA EN ALERTA
        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 1f && !enemy.isAlerted)
        {
            patrolTarget = GetRandomPoint();
            enemy.agent.SetDestination(patrolTarget);
        }

        if (enemy.canCurrentlySeePlayer)
        {
            enemy.TransitionToState(enemy.chaseState);
        }

        // Si está en alerta, ya tomó decisión, y no ve al jugador
        if (enemy.isAlerted && enemy.hasMadeCoverDecision && !enemy.canCurrentlySeePlayer)
        {
            // si Rando.value(0-1) es menor a probabilidad de esconderse 0.5
            if (Random.value <= enemy.actionProbability)
            {
                enemy.agent.speed = enemy.patrolRun;
                enemy.TransitionToState(enemy.seekCoverState);
                return;
            }
            else
            {
                // Ya tomó la decisión y decidió no esconderse, evitar que lo vuelva a intentar
                enemy.hasMadeCoverDecision = false; // o mantenerlo en true si no quieres repetir
            }
        }

        // Si llegó al punto de patrullaje, busca un nuevo punto de patrullaje Y ESTA EN ALERTA
        float valor = Random.value;
        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 1f && enemy.isAlerted && valor >= (enemy.actionProbability + 0.3f))
        {
            enemy.TransitionToState(enemy.investigateState);
        }
        if(!enemy.agent.pathPending && enemy.agent.remainingDistance < 1f && enemy.isAlerted && valor <= (enemy.actionProbability + 0.3f))
        {
            patrolTarget = GetRandomPoint();
            enemy.agent.SetDestination(patrolTarget);
        }
        
    }

    public override void Exit()
    {
        // Aquí podrías limpiar cosas si es necesario
    }

    // Devuelve un punto aleatorio dentro del NavMesh
    private Vector3 GetRandomPoint()
    {
        for (int i = 0; i < 30; i++) // intenta varias veceses encontrar un punto para patruyar
        {
            //direccion random en una esfera con un radio
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            //le pasa la direccion del vector3 a la posicion del enemigo
            randomDirection += enemy.transform.position;

            //Si la posicion deada es valida la regresa
            //posicion rando, informacion de la posicion encontrada, en un radio de 2, busca en toda las areas del navmesh sin restricciones
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                return hit.position;
        }

        return enemy.transform.position; //si no hay posisicion valida se queda en su lugar
    }

}