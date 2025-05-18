using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Shot Speed")]
    public float fireRate = 1f;//velocidad de cada disparo
    public float fireRateTimer = 4f;//cronometro para disparar

    [Header("MultiShot")]
    public int multiShot = 1;//multidisparo

    [Header("References")]
    private Animator weaponAnimator;//animador del arma-se referencia en el Start
    private AudioSource audioSource;//complemento que reproduce el sonido al dispara-se referencia en el Start
    private AmmoCurrent municionCurrent;//texto que marca las balas que se pueden disparar
    private AmmoMax municionMax;//texto que marca las balas en recerva

    public AudioClip soundSource;//sonido al disparar
    [SerializeField]private GameObject bullet;//llama al objeto Arked bullet, o bala de Arked
    [SerializeField]private Transform barrelPos;//posicion del barril
    public float bulletSpeed = 500f;//velocidad de la bala
    public PlayerAim aim;//componente PlayerAim del player


    [Header("Bullets")]
    public int bulletInGun=20;
    public int ammonInGun=20;

    [HideInInspector]public int maxBulletInGun = 20;
    [HideInInspector]public int maxAmmonReserve=20;

    [Header("Array Bullet")]
    public GameObject[] bullets;

    [Header("Active")]
    public bool activo;



    void Start()
    {
        aim = FindObjectOfType<PlayerAim>();//busca el comppnente PlayerAim del jugador

        municionCurrent = FindAnyObjectByType<AmmoCurrent>();
        municionMax = FindAnyObjectByType<AmmoMax>();

        weaponAnimator = GetComponent<Animator>();//se referencia que el animator es de la misma arma
        audioSource = GetComponent<AudioSource>();//se referencia que el audiosource es de la misma arma
        //GameManager.Instance.gunAmmo = municion;
        
        municionCurrent.GetComponent<Text>().text = bulletInGun.ToString();
        municionMax.GetComponent<Text>().text = ammonInGun.ToString();
    }

    void Update()
    {
        if(activo){
            DrawAmmo();   
        }
        SpeedOfEachShot();
        Fire();
        ReloadAmmon();
        WaponAnimation();
    }

    private void DrawAmmo(){

        municionCurrent.GetComponent<Text>().text = bulletInGun.ToString();
        municionMax.GetComponent<Text>().text = ammonInGun.ToString();

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

        if(Input.GetMouseButton(0) && fireRateTimer > fireRate && /*GameManager.Instance.gunAmmo*/ bulletInGun > 0 && Time.timeScale != 0){
                
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
            bulletInGun--;
        }

    }

    private void ReloadAmmon(){

        if(bulletInGun == 0){
            if(Input.GetKeyUp(KeyCode.R)){
                bulletInGun+=ammonInGun;
                ammonInGun-=maxAmmonReserve;
            }
        }
    }

//Seccion de animacion
    void WaponAnimation(){

        //si no tiene balas se abre el arma
        if(bulletInGun <= 0){
            weaponAnimator.Play("OpenShell");
        }
        //Si tiene balas se cierra
        else{
            weaponAnimator.Play("CloseShell");
        }
    }
}
