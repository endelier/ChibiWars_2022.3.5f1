using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPoint : MonoBehaviour
{
    //Si el punto esta ocupado
    public bool isOccupied = false;

    //verificar si el punto esta proteguido desde aqui
    public bool ProvidesCoverFrom(Vector3 targetPosition, LayerMask obstacleMask)
    {
        Vector3 directionToTarget = targetPosition - transform.position;
        float distance = directionToTarget.magnitude;
        directionToTarget.Normalize();

        return Physics.Raycast(transform.position, directionToTarget, distance, obstacleMask);
    }

    //para saber si el enemigo se oculta del jugador si se pone en ese punto
    public bool BlockViewFrom(Vector3 observerPosition, LayerMask obstacleMask)
    {
        Vector3 dir = transform.position - observerPosition;
        float distance = dir.magnitude;
        dir.Normalize();

        return Physics.Raycast(observerPosition, dir, distance, obstacleMask);
    }
}
