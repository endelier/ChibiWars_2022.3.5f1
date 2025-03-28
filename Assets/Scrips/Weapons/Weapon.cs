using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Shot Speed")]
    public float fireRate = 1f;//velocidad de cada disparo
    public float fireRateTimer = 4f;//cronometro para disparar

    [Header("Charge")]
    //public int bulletCharge = 20;//cargador

    [Header("MultiShot")]
    public int multiShot = 1;//multidisparo

    [Header("References")]
    private Animator weaponAnimator;//animador del arma-se referencia en el Start
    private AudioSource audioSource;//complemento que reproduce el sonido al dispara-se referencia en el Start

    public AudioClip soundSource;
    [SerializeField]private GameObject bullet;//llama al objeto Arked bullet, o bala de Arked
    [SerializeField]private Transform barrelPos;//posicion del barril
    public float bulletSpeed = 500f;//velocidad de la bala
    public PlayerAim aim;

    public int municion = 20;


    void Start()
    {
        weaponAnimator = GetComponent<Animator>();//se referencia que el animator es de la misma arma
        audioSource = GetComponent<AudioSource>();//se referencia que el audiosource es de la misma arma
        GameManager.Instance.gunAmmo = municion;
    }
    void Update()
    {
        
        SpeedOfEachShot();
        Fire();
        WaponAnimation();
    }


    //Cronometro de cada cuanto se puede disparar
    private void SpeedOfEachShot(){

        //se suma a cada frame
        fireRateTimer += Time.deltaTime;

        //no deja que el cronometro sea mayor a 4
        if(fireRateTimer > 4f){
            fireRateTimer = 4f;
        }

    }

    //funcion que se ejecuta cuando el cronometro es mas alto y dispara
    private void Fire(){
        
        barrelPos.LookAt(aim.aimpos);//el barril mira hacia donde esta el puntero

        if(Input.GetMouseButton(0) && fireRateTimer > fireRate && GameManager.Instance.gunAmmo > 0 && Time.timeScale != 0){
                
            fireRateTimer = 0;//reinica el cronometro

            audioSource.PlayOneShot(soundSource);

            //Instancia en ciclo balas, oesa el multidisparo
            for(int i =0; i<multiShot; i++){
                
                //crea la bala
                GameObject newBullet = Instantiate(bullet,barrelPos.position, barrelPos.rotation);
                
                //busca el componente rigidbody
                Rigidbody rb = newBullet.GetComponent<Rigidbody>();

                //se le da el impulso, dede el frente del barril X velocidad de la bala
                rb.AddForce(barrelPos.forward * bulletSpeed, ForceMode.Impulse);

                Destroy(newBullet, 5);
            }
            
            //le resta una bala a la municion;
            GameManager.Instance.gunAmmo--;
        }

    }

//Seccion de animacion
    void WaponAnimation(){

        //si no tiene balas se abre el arma
        if(GameManager.Instance.gunAmmo <= 0){
            weaponAnimator.Play("OpenShell");
        }
        //Si tiene balas se cierra
        else{
            weaponAnimator.Play("CloseShell");
        }
    }
}
