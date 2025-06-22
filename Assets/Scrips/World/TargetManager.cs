using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    // Lista privada que guarda todos los objetivos que tienen target 
    public List<TargetData> targets = new List<TargetData>(); 

    private void Awake()
    {
        // Implementación Singleton segura para MonoBehaviour
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Registra un nuevo objetivo si no está en la lista
    public void RegisterTarget(TargetData target)
    {
        if (!targets.Contains(target))
            targets.Add(target);
    }

    // Elimina un objetivo si ya esta en la lista
    public void UnregisterTarget(TargetData target)
    {
        if (targets.Contains(target))
            targets.Remove(target);
    }

    // Devuelve todos los objetivos registrados
    public List<TargetData> GetAllTargets()
    {
        return targets;
    }
}