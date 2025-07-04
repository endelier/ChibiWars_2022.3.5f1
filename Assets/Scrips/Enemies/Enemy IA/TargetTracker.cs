using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTracker
{
    private Transform enemyposition;
    [HideInInspector]public float detectionRadius;
    private LayerMask obstacleMask;

    //propiedad
    public TargetData currentTarget { get; private set; }

    public TargetTracker(Transform owner, float radius, LayerMask obstacleMask)
    {
        this.enemyposition = owner;
        this.detectionRadius = radius;
        this.obstacleMask = obstacleMask;
    }

    public void UpdateTarget()
    {
        if (TargetManager.Instance == null) return;

        List<TargetData> allTargets = TargetManager.Instance.GetAllTargets();
        TargetData bestTarget = null;
        int bestPriority = int.MinValue;
        float closestDistance = float.MaxValue;

        foreach (TargetData target in allTargets)
        {
            if (target == null || target.targetTransform == null)
                continue;

            Vector3 dirToTarget = target.targetTransform.position - enemyposition.position;
            float distance = dirToTarget.magnitude;

            int priority = target.GetPriority(enemyposition.position);
            //prioridad es = a prioridad mayor o igual a 4
            bool isHighPriority = priority >= 4;

            if (distance > detectionRadius)
            {
                continue;
            }

            // Solo aplica raycast si no es prioridad alta
            if (!isHighPriority && Physics.Raycast(enemyposition.position, dirToTarget.normalized, distance, obstacleMask))
            {
                continue;
            }

            // es escoje el mas cercano
            //if(distance < closestDistance || (Mathf.Approximately(distance, closestDistance) && priority > bestPriority))
            //revisa el mejor objetivo o si tienen la misma prioridad envia al mas cercano y escoje el que tiene mayor prioridad
            if (priority > bestPriority || (priority == bestPriority && distance < closestDistance))
            {
                bestTarget = target;
                bestPriority = priority;
                closestDistance = distance;
            }
        }

        currentTarget = bestTarget;
    }

    public bool HasTarget()
    {
        return currentTarget != null;
    }
}