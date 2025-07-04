using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyState
{

    //Cuando alguien cree un PatrolState, debe pasarme un EnemyAIController. Yo lo voy a enviar a mi clase base EnemyState para que también lo tenga.
    public ChaseState(EnemyAIController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.isStopped = true;
        enemy.agent.speed = enemy.patrolRun;
        enemy.agent.stoppingDistance = enemy.minChaseDistance;
    }

    //Actualiza el perseguir al jugador
    public override void Update()
    {
        //si no puede ver al jugador o no hay posiciondel jugador pasa a patrullaje
        if (enemy.currentTarget == null || enemy.currentTarget.targetTransform == null)
        {
            enemy.TransitionToState(enemy.patrolState);
        }

        Transform target = enemy.currentTarget.targetTransform;
        float distance = Vector3.Distance(enemy.transform.position, target.position);

        //&& distance <= enemy.minChaseDistance
        if (enemy.canCurrentlySeePlayer && distance <= enemy.minChaseDistance)
        {
            enemy.agent.isStopped = true;

            //si esta en rango transiciona a disparo
            enemy.TransitionToState(enemy.shootState);
            return;
        }

        //si el jugador esta lejos persigue al jugador
        if (distance > enemy.minChaseDistance && distance <= enemy.maxChaseDistance)
        {
            enemy.agent.isStopped = false;
            enemy.agent.SetDestination(target.position);
        }

        if (!enemy.canCurrentlySeePlayer)
        {
            enemy.TransitionToState(enemy.investigateState);
        }

    }

    public override void Exit()
    {
        enemy.agent.isStopped = false;
    }
}
