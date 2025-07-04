using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    public TargetData data; // referencia a TargetData
    public TargetType targetType = TargetType.Player;
    public int priority = 1; // prioridad base del objetivo

    void Start()
    {
        // Crea la data
        data = new TargetData(transform, targetType, priority);

        // Se registra con el TargetManager que ahora es MonoBehaviour
        TargetManager.Instance.RegisterTarget(data);
    }

    void OnDestroy()
    {
        if (TargetManager.Instance != null)
        {
            TargetManager.Instance.UnregisterTarget(data);
        }
    }

    //es obligatori esta funcion para asignar un nuevo valor a la prioridad
    public void SetPriority(int newPriority)
    {
        if (data != null)
        {
            Debug.Log("enviando prio " + newPriority);
            data.priority = newPriority;
        }
    }

    //se le da un nuevo tipo de target
    public void SetTargetType(TargetType newType)
    {
        if (data != null)
        {
            data.targetType = newType;
        }
    }
}