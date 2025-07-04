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
        //si el enemigo esta con vida
        if (enemy.enemylife.life)
        {
            //al entrar en el estado de disparo detiene al enemigo
            if (!enemy.hasMadeCoverDecision)
                enemy.agent.isStopped = true;

            fireCooldown -= Time.deltaTime;

            if (fireCooldown <= 0f && enemy.currentTarget != null)
            {
                Shoot(enemy.currentTarget);
                fireCooldown = fireRate;
            }

            if (enemy.isAlerted && !enemy.canCurrentlySeePlayer && enemy.currentTarget.priority <= 3)
            {
                enemy.TransitionToState(enemy.investigateState);
            }

            float distance = Vector3.Distance(enemy.transform.position, enemy.currentTarget.targetTransform.position);

            // Si el jugador se aleja demasiado, volver a persecución
            if (distance > enemy.minChaseDistance && enemy.currentTarget.priority <= 3)
            {
                enemy.agent.isStopped = false;
                enemy.TransitionToState(enemy.chaseState);
                return;
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

        //si el objetivo esta entre 1 y 3
        if (enemy.currentTarget.priority >= 1 && enemy.currentTarget.priority <= 3)
        {
            // Dirección hacia el centro del cuerpo del objetivo
            Vector3 targetPosition = target.targetTransform.position;
            Vector3 direction = (targetPosition - enemy.barrel.position).normalized;

            bullet.transform.rotation = Quaternion.LookRotation(direction);
            bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
        }
        
        //si el objetivo esta en 4
        if (enemy.currentTarget.priority == 4)
        {
            // Eleva el disparo
            Vector3 targetPosition = target.targetTransform.position + Vector3.up * 2f;
            Vector3 direction = (targetPosition - enemy.barrel.position).normalized;
            enemy.LookAt(enemy.currentTarget.targetTransform.position);

            bullet.transform.rotation = Quaternion.LookRotation(direction);
            bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
        }

        //Debug.DrawRay(enemy.barrel.position, direction * 10f, Color.red, 1f);
    }

}