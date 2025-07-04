using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateState : EnemyState
{
    private bool isLookingAround = false;

    public InvestigateState(EnemyAIController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.isStopped = true;
        enemy.agent.stoppingDistance = 0f;
        isLookingAround = false;
//        enemy.StartCoroutine(LookAroundRoutine());
    }

    public override void Update()
    {
        Debug.Log(isLookingAround);
        if (enemy.currentTarget != null && enemy.currentTarget.priority >= 3)
        {
            enemy.TransitionToState(enemy.attackObjectiveState);
        }
        if(enemy.currentTarget==null && !isLookingAround || enemy.currentTarget !=null && !isLookingAround)
        {
            enemy.StartCoroutine(LookAroundRoutine());
        }
    }

    public override void Exit()
    {
        enemy.agent.isStopped = false;
    }

    private IEnumerator LookAroundRoutine()
    {
        isLookingAround = true;

        Debug.Log("iniciando corrutina");
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
            Debug.Log("girando");

            yield return new WaitForSeconds(0.3f); // pausa tras cada giro
        }

        enemy.TransitionToState(enemy.patrolState);
    }
}
