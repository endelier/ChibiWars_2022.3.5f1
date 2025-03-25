using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInterations : MonoBehaviour
{

    //Este scrip es el que se cominica con el GameManager, envia y recibe informacion
    public Transform starPosition;

    void OnTriggerEnter(Collider other)
    {
        
        //compara que el objeto con el que coliciono tenga el tag de municion
        if(other.gameObject.CompareTag("GunAmmo")){
            
            //le da a la variable del gamemanager y le da mas municion
            GameManager.Instance.gunAmmo += other.GetComponent<AmmoBox>().ammo;

            //destruye la caja de municion
            Destroy(other.gameObject);
        }


        //Si entra en la zona de muerte, pierde vida o respawnea
        if(other.gameObject.CompareTag("DeathZone")){

            GameManager.Instance.LoseHealth(200);
            GetComponent<CharacterController>().enabled=false;
            this.gameObject.transform.position = starPosition.position;
            GetComponent<CharacterController>().enabled=true;
        }
        
    }

    //Si recive un disparo
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet")){
            GameManager.Instance.LoseHealth(25);
        }
    }
}
