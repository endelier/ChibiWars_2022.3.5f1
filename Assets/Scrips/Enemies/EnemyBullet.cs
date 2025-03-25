using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //Solo pasa a destruirse la bala en un cierto tiempo o si choca con el jugador
    void Start()
    {
        Destroy(gameObject, 3);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
}
