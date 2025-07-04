using UnityEngine;

public enum TargetType
{
    Player,
    Decoy,
    Objective,
    Structure,
    // Agrega más si lo necesitas
}

public class TargetData
{
    public Transform targetTransform;//ubicaciond el objeto
    public TargetType targetType;//tipo del objeto del enum de arriba
    public int basePriority;//nivel de prioridad por default
    public int priority;// Prioridad actual, modificable en tiempo real

    // Estados dinámicos
    public bool isInvisible = false;//si es invisible
    public bool isProvoking = false;//si lo estan provocando
    public float provokeRange = 10f;//el rango de probocacion

    // se esta usando un constructo ya que no TargetData no hereda de MonoBehaivour
    //por lo que no puede usar las clases, Star, Awake, etc. para inicializar las variables
    public TargetData(Transform transform, TargetType type, int basepriority)
    {
        targetTransform = transform;
        targetType = type;
        basePriority = basepriority;
        priority = basePriority;
    }

    //Devuelve la prioridad actual tomando en cuenta si el objetivo está provocando o es invisible.
    public int GetPriority(Vector3 enemyPosition)
    {

        return priority;
    }
}
