using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekCoverState : EnemyState
{
    private CoverPoint targetCover;
    public SeekCoverState(EnemyAIController enemy) : base(enemy) { }
    public override void Enter()
    {
        //punto es = a funcino de buscar punto
        targetCover = FindBestCoverPoint();

        //si punto no es nullo
        if (targetCover != null)
        {
            // el punto ahora esta opupado y se mueve a ese punto
            targetCover.isOccupied = true;
            enemy.currentCoverPoint = targetCover;
            enemy.agent.SetDestination(targetCover.transform.position);
        }
        //si no encuentra punto, vuelve a patrullar
        else
        {
            enemy.TransitionToState(enemy.patrolState);
        }

    }
    public override void Update()
    {
        if (targetCover == null)
        {
            enemy.TransitionToState(enemy.patrolState);
            return;
        }

        //si llego a la cobertura
        if (!enemy.agent.pathPending && enemy.agent.remainingDistance < 1f)
        {
            //aqui se puede estar un rato, cambiar de estado o volver a patrullar
            enemy.agent.isStopped = true;

            // Si está en alerta, ya tomó decisión, y no ve al jugador
            if (enemy.isAlerted && enemy.hasMadeCoverDecision && !enemy.canCurrentlySeePlayer)
            {
                enemy.TransitionToState(enemy.investigateState);
                return;
            }
        }
    }

    public override void Exit()
    {
        if (targetCover != null)
        {
            //nulifica el punto
            targetCover.isOccupied = false;
            enemy.currentCoverPoint = null;
        }
    }

    private CoverPoint FindBestCoverPoint()
    {
        //Arreglo de componente CoverPoint
        CoverPoint[] allPoints = GameObject.FindObjectsOfType<CoverPoint>();
        CoverPoint best = null;
        float bestScore = float.MinValue;

        foreach (CoverPoint point in allPoints)
        {
            //si el punto esta ocupado continua
            if (point.isOccupied) continue;

            //el punto debe bloquar la vista desde el jugador
            if (!point.BlockViewFrom(enemy.player.position, enemy.obstacleMask)) continue;

            float dist = Vector3.Distance(enemy.transform.position, point.transform.position);
            float score = -dist;//mas cerca = mejor

            if (score > bestScore)
            {
                bestScore = score;
                best = point;
            }
        }
        return best;
    }
}
