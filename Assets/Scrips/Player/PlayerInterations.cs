using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInterations : MonoBehaviour
{

    //Este scrip es el que se cominica con el GameManager, envia y recibe informacion
    [Header("Star position")]
    public Transform starPosition;

    public GameObject weaponPrimary;
    public GameObject weaponSecondary;

    [HideInInspector]public int bulletDamage;

    void Start()
    {
        starPosition = GameObject.FindGameObjectWithTag("PuntoInicial").transform;
        weaponPrimary = GameObject.FindGameObjectWithTag("PrimaryWeapon");
        weaponSecondary = GameObject.FindGameObjectWithTag("SecondaryWeapon");
    }

    void OnTriggerEnter(Collider other)
    {

        //compara que el objeto con el que coliciono tenga el tag de municion
        //Si la municion en recerva es menor a la maxima municion recoge la municion
        if(other.gameObject.CompareTag("PrimaryAmmon")){
            if(weaponPrimary.GetComponent<Weapon>().ammonInGun < weaponPrimary.GetComponent<Weapon>().maxAmmonReserve){
                weaponPrimary.GetComponent<Weapon>().ammonInGun += other.GetComponent<AmmoBox>().ammo;
                Destroy(other.gameObject);
            }
        }
        if(other.gameObject.CompareTag("SecondaryAmmon")){
            if(weaponSecondary.GetComponent<Weapon>().ammonInGun < weaponSecondary.GetComponent<Weapon>().maxAmmonReserve){
                weaponSecondary.GetComponent<Weapon>().ammonInGun += other.GetComponent<AmmoBox>().ammo;
                Destroy(other.gameObject);
            }
        }


        //Si entra en la zona de muerte, pierde vida o respawnea
        if(other.gameObject.CompareTag("DeathZone")){

            //GameManager.Instance.LoseHealth(200);
            GetComponent<CharacterController>().enabled=false;
            this.gameObject.transform.position = starPosition.position;
            GetComponent<CharacterController>().enabled=true;
        }
        
    }

    //Si recive un disparo
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet")){
            bulletDamage = collision.gameObject.GetComponent<EnemyBullet>().damageBullet;
            GameManager.Instance.HealthPlayer(bulletDamage);
        }
    }
}
