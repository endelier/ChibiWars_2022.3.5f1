using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : EnemyState
{
    private float fireRate = 1f;
    private float fireCooldown;

    private float bulletSpeed = 50f;

    public ShootState(EnemyAIController enemy) : base(enemy) { }

    public override void Enter()
    {
        fireCooldown = fireRate;
    }

    public override void Update()
    {

        Debug.Log("Disparo");
        //al entrar en el estado de disparo detiene al enemigo
        if (!enemy.hasMadeCoverDecision)
            enemy.agent.isStopped = true;

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f && enemy.currentTarget != null)
        {
            Shoot(enemy.currentTarget);
            fireCooldown = fireRate;
        }

        // Si esta en alerta y no ve al jugador, vuelve a patrullar
        /*if (enemy.isAlerted && !enemy.canCurrentlySeePlayer)
        {
            enemy.TransitionToState(enemy.patrolState);
            //regresa tope a false para poder vover a tomar decisiones
            enemy.tope = false;
            return;
        }*/

        if (enemy.isAlerted && !enemy.canCurrentlySeePlayer)
        {
            enemy.TransitionToState(enemy.investigateState);
        }

        // Si está en alerta, ya tomó decisión, y ve al jugador
        /*if (enemy.isAlerted && enemy.hasMadeCoverDecision && enemy.canCurrentlySeePlayer)
        {
            // si Rando.value(0-1) es menor a probabilidad de esconderse 0.5
            if (Random.value < enemy.seekCoverProbability)
            {
                enemy.agent.isStopped = false;
                enemy.agent.speed = enemy.patrolRun;
                enemy.TransitionToState(enemy.seekCoverState);
                return;
            }
            else
            {
                // Ya tomó la decisión y decidió no esconderse, evitar que lo vuelva a intentar
                enemy.hasMadeCoverDecision = false; // o mantenerlo en true si no quieres repetir
            }
        }*/
    }

    public override void Exit()
    {
        enemy.agent.isStopped = false;
    }

    private void Shoot(TargetData target)
    {
        GameObject bullet = EnemyBulletPool.Instance.GetBullet();

        // Posicion de disparo
        bullet.transform.position = enemy.barrel.position;

        // Dirección hacia el centro del cuerpo del objetivo
        //Vector3 targetPosition = target.targetTransform.position + Vector3.up * 1f;
        Vector3 targetPosition = target.targetTransform.position;
        Vector3 direction = (targetPosition - enemy.barrel.position).normalized;

        bullet.transform.rotation = Quaternion.LookRotation(direction);
        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);

        //Debug.DrawRay(enemy.barrel.position, direction * 10f, Color.red, 1f);
    }

}