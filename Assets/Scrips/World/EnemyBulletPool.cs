using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    //instancia SOLO una instancia de EnemyBulletPool asi todos los enemigos sacan balas de una sola lista de balas
    public static EnemyBulletPool Instance;

    public GameObject bulletPrefab;//referencia de la bala
    public int poolSize = 20;//cantidad de balas que tendra el enemigo

    //lista de las balas, de todos los enemigos
    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;//se instancea

        //por la cantidad de balas crea las balas, las desactiva y mete a la lista
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            //Enqueue agrega un elemento al final de Queue.
            bulletPool.Enqueue(bullet);
        }
    }

    //funcion para tomar una bala de Queue
    public GameObject GetBullet()
    {
        //si la lista es mas grande que cero
        if (bulletPool.Count > 0)
        {
            //Dequeue quita el elemento más antiguo del principio de Queue.
            GameObject bullet = bulletPool.Dequeue();
            //la activa y la regresa
            bullet.SetActive(true);
            return bullet;
        }

        // Si el pool se queda sin balas, crea más (opcional)
        GameObject newBullet = Instantiate(bulletPrefab);
        return newBullet;
    }

    //funcion de regresar la bala a la lista
    public void ReturnBullet(GameObject bullet)
    {
        //desactiva la bala
        bullet.SetActive(false);
        //la mete al final de la lista
        bulletPool.Enqueue(bullet);
    }
}
