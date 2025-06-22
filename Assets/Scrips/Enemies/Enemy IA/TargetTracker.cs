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

            if (distance > detectionRadius)
            {
                continue;
            }

            if (Physics.Raycast(enemyposition.position, dirToTarget.normalized, distance, obstacleMask))
                continue;

            int priority = target.GetPriority(enemyposition.position);

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