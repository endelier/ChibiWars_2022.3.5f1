using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    // Start is called before the first frame update

    public int damageBullet=25;

    public bool destroyBullet=false;

    private void OnCollisionEnter(Collision collision)
    {
        if(destroyBullet==true &&collision.gameObject.CompareTag("Building")){
            Destroy(gameObject);
        }

        if(collision.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
}
