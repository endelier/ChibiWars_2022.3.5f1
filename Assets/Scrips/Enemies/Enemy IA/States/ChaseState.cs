using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyState
{

    //Cuando alguien cree un PatrolState, debe pasarme un EnemyAIController. Yo lo voy a enviar a mi clase base EnemyState para que también lo tenga.
    public ChaseState(EnemyAIController enemy) : base(enemy) { }

    public override void Enter()
    {
        // Nada especial por ahora
    }

    //Actualiza el perseguir al jugador
    public override void Update()
    {
        //distancia al jugador = vector3 Distancia(enemigo posicion, enemigo,variable(referecia), posicion)
        //float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        // Si el jugador se aleja, vuelve a patrullar
        /*if (distanceToPlayer > enemy.chaseDistance * 1.5f)
        {
            enemy.TransitionToState(enemy.patrolState);
            return;
        }*/

        Debug.Log("persiguiendo");
        // Persigue al jugador
        //enemy.agent.SetDestination(enemy.player.transform.position);
    }

    public override void Exit()
    {
        // Nada por ahora
    }
}
