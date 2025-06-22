using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject enemyBullet;
    public Transform spawnBulletPoint;//de donde salen las balas

    private Transform player;
    bool followplayer;//variable boliana directa del Enemy

    public float bulletSpeed;//Veocidad a la que puede disparar

    private float distanceToPlayer;//float de la distancia del vector entre el enemigo y el jugador
    private float distanceToShootPlayer = 20;//Distancia minima para reaccionar

    void Start()
    {
        //busca al jugador y conseuimos su transform
        player = FindAnyObjectByType<PlayerMove>().transform;

    }

    void Update()
    {
        //de este objeto iguala la variable de enemy followplayer
        followplayer = GetComponent<Enemy>().followplayer;
        //de este objeto iguala la variable de enemy distanceToPlayer
        distanceToPlayer = GetComponent<Enemy>().distanceToPlayer;

        //si distacia al jugador es menor a distancia para disparar y seguir sea verdadera
        if (distanceToPlayer <= distanceToShootPlayer && followplayer)
        {
            Debug.Log(distanceToPlayer);
            ShootEnemy();
        }
    }

    public void ShootEnemy()
    {

        Vector3 playerDirection = player.position - transform.position;

        GameObject newbullet;

        newbullet = Instantiate(enemyBullet, spawnBulletPoint.position, spawnBulletPoint.rotation);

        newbullet.GetComponent<Rigidbody>().AddForce(playerDirection * bulletSpeed, ForceMode.Force);
        
        Destroy(newbullet, 5);

    }
}
