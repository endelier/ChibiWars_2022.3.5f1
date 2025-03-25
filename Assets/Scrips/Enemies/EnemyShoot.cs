using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject enemyBullet;
    public Transform spawnBulletPoint;

    private Transform playerPosition;
    private Enemy enemyfollow;

    public float bulletSpeed;

    void Start()
    {
        //busca al jugador y conseuimos su transform
        playerPosition = FindAnyObjectByType<PlayerMove>().transform;

        enemyfollow = GetComponent<Enemy>();

        Invoke("ShootEnemy", 3);
    }

    void Update()
    {
        if(enemyfollow.followplayer == true){
            //Invoke("ShootEnemy", .5f);
        }
    }

    public void ShootEnemy(){

        Vector3 playerDirection = playerPosition.position - transform.position;

        GameObject newbullet;

        newbullet = Instantiate(enemyBullet, spawnBulletPoint.position, spawnBulletPoint.rotation);

        newbullet.GetComponent<Rigidbody>().AddForce(playerDirection * bulletSpeed, ForceMode.Force);

        Invoke("ShootEnemy", .5f);
    }
}
