using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 20f;
    public int damageBullet = 10;//<<<<<<<<<<<<<<<<<<<<<<<<<<<-

    //cuando se activa la bala
    private void OnEnable()
    {
        // Invoka la funcion para que se desactive en 20 segundos
        Invoke(nameof(DeactivateBullet), lifeTime);
    }

    //Desactiva la bala mandandola a al Queue y la funcion ReturnBullet es la que la desactiva
    private void DeactivateBullet()
    {
        EnemyBulletPool.Instance.ReturnBullet(this.gameObject);
    }

    //si toca al jugador desactiva la bala
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeactivateBullet();
        }
    }
}