using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateState : EnemyState
{
    private bool isLookingAround = false;
    private bool investigating = false;

    public InvestigateState(EnemyAIController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.isStopped = true;
        //isLookingAround = false;
        enemy.StartCoroutine(LookAroundRoutine());
    }

    public override void Update()
    {
        // Si ve al jugador, vuelve al combate
        if (enemy.canCurrentlySeePlayer)
        {
            enemy.TransitionToState(enemy.shootState);
        }
        // Si terminó de mirar y no lo encontró, volverá a patrullar (esto se maneja al final de la rutina)
    }

    public override void Exit()
    {
        enemy.agent.isStopped = false;
    }

    private IEnumerator LookAroundRoutine()
    {
        isLookingAround = true;

        int steps = 6;
        float angleStep = 60f;
        float rotationSpeed = 180f; // grados por segundo

        for (int i = 0; i < steps; i++)
        {
            if (enemy.canCurrentlySeePlayer)
            {
                enemy.TransitionToState(enemy.shootState);
                yield break;
            }

            Quaternion startRotation = enemy.transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0, enemy.transform.eulerAngles.y + angleStep, 0);

            float t = 0f;
            while (Quaternion.Angle(enemy.transform.rotation, targetRotation) > 1f)
            {
                enemy.transform.rotation = Quaternion.RotateTowards(
                    enemy.transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
                yield return null;

                if (enemy.canCurrentlySeePlayer)
                {
                    enemy.TransitionToState(enemy.shootState);
                    yield break;
                }
            }

            yield return new WaitForSeconds(0.3f); // pausa tras cada giro
        }

        isLookingAround = false;
        enemy.TransitionToState(enemy.patrolState);
    }
}
