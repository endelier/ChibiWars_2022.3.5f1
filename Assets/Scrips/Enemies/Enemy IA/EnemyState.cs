using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyState
{
    protected EnemyAIController enemy;//referencia del controlador IA

    //Espera un controlador de su herencias
    public EnemyState(EnemyAIController enemy){
        //resive el contrloador del enemigo
        this.enemy = enemy;
    }

    //Se llama al entrar en un estado
    public abstract void Enter();

    //Se llama cada que se actualiza un frame
    public abstract void Update();

    //se llama cuando sale de un estado
    public abstract void Exit();
}
