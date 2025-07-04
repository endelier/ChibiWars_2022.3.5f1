using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectiveState : EnemyState
{

    public AttackObjectiveState(EnemyAIController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.patrolRun;
        enemy.agent.SetDestination(enemy.currentTarget.targetTransform.position);
        /*enemy.radiusView = 25;
        enemy.angleView = 120f;*/

    }
    // Update is called once per frame
    public override void Update()
    {

        //si no hay objetivo o el objetivo es mejor a prioridad 4 comienza a patrullar
        /*if (enemy.currentTarget == null || enemy.currentTarget.priority < 4)
        {
            enemy.TransitionToState(enemy.patrolState);
            return;
        }*/

        float distance = Vector3.Distance(enemy.transform.position, enemy.currentTarget.targetTransform.position);

        //si ve al jugador, deja a atacar al objetivo
        /*if (enemy.canCurrentlySeePlayer)
        {
            enemy.TransitionToState(enemy.chaseState);
            return;
        }*/
        
        if (!enemy.agent.pathPending && distance < 15f)
        {

            enemy.TransitionToState(enemy.shootState);
            return;
        }
    }

    public override void Exit()
    {
        enemy.agent.isStopped = false;
    }
}
