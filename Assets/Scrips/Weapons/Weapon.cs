using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Shot Speed")]
    public float fireRate = 1f;//Cadencia de fuego
    public float fireRateTimer = 4f;//cronometro para disparar

    [Header("MultiShot")]
    public int multiShot = 1;//multidisparo

    [Header("References")]
    private Animator weaponAnimator;//animador del arma-se referencia en el Start
    private AudioSource audioSource;//complemento que reproduce el sonido al dispara-se referencia en el Start
    private REAmmonCurrentText municionCurrent;//texto que marca las balas que se pueden disparar
    private REAmmonMaxText municionMax;//texto que marca las balas en recerva

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

    [Header("Pool Bullet")]
    [SerializeField]private List<GameObject> bulletPool = new List<GameObject>();//lista de balas
    private int poolSize = 50; // Tamaño de la lista de las balas

    [Header("Active")]
    public bool activo;



    void Start()
    {
        aim = FindObjectOfType<PlayerAim>();//busca el comppnente PlayerAim del jugador

        municionCurrent = FindAnyObjectByType<REAmmonCurrentText>();
        municionMax = FindAnyObjectByType<REAmmonMaxText>();

        weaponAnimator = GetComponent<Animator>();//se referencia que el animator es de la misma arma
        audioSource = GetComponent<AudioSource>();//se referencia que el audiosource es de la misma arma

        //For que instancia balas y las mete en la lista
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bullet);
            obj.SetActive(false);
            bulletPool.Add(obj);
        }
        
        //Textos de la municion
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

    //Funcion para dibujar las balas en el arma y la municion
    private void DrawAmmo()
    {

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
    private void Fire() {
        //el barril mira a la posicion del aim
        barrelPos.LookAt(aim.aimpos);

        //if para disparar, si click y cronometro es menor a cadencia y balas son mayores a cero y el tiempo no esta pausado
        if (Input.GetMouseButton(0) && fireRateTimer > fireRate && bulletInGun > 0 && Time.timeScale != 0)
        {
            //reinicia el cronometro a 0
            fireRateTimer = 0;
            //reproduce el sonido de disparo
            audioSource.PlayOneShot(soundSource);

            //por cada multidisparo saca una bala de la lista de balas, 1 = 1 bala disparada 2 = 2 etc.
            for (int i = 0; i < multiShot; i++)
            {
                //Llama la funcion tomar bala de la picina
                GameObject newBullet = GetPooledBullet();

                //si la bala no es nula
                if (newBullet != null)
                {
                    //a la bala le da posicion y rotacion del barrir
                    newBullet.transform.position = barrelPos.position;
                    newBullet.transform.rotation = barrelPos.rotation;
                    //activa la bala
                    newBullet.SetActive(true);
                    
                    //iguala una variable Rigidbody para solo hacer una llamada
                    Rigidbody rb = newBullet.GetComponent<Rigidbody>();
                    rb.velocity = Vector3.zero; // reset para evitar arrastre de fuerza previa
                    rb.angularVelocity = Vector3.zero;
                    rb.AddForce(barrelPos.forward * bulletSpeed, ForceMode.Impulse);
                }
            }

            bulletInGun--;
        }
    }

    //Funcion para recargar arma
    private void ReloadAmmon()
    {
        //Si balas en arma son igual a 0
        if (bulletInGun == 0)
        {
            //Si aplasta la letra R
            if (Input.GetKeyUp(KeyCode.R))
            {
                bulletInGun += ammonInGun;
                ammonInGun -= maxAmmonReserve;
            }
        }
    }

    //Funcion para obtener una bala disponible del pool
    private GameObject GetPooledBullet()
    {
        //por cada bala en bulletPool
        foreach (GameObject bullet in bulletPool)
        {
            //si bullet no esta activo en la jerarquia
            if (!bullet.activeInHierarchy)
                //regresa bullet o da una bala
                return bullet;
        }
        return null;
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
