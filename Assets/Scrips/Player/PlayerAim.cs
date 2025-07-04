using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerAim : MonoBehaviour
{
        //Referencias de otros objetos
    [Header("Objects Reference")]
    public CinemachineVirtualCamera cameraVirtual;//Camara Virtual
    public PlayerCamera centerPlayer;//centerPlayer
    private Animator animator;//animador del personaje-se referencia en el Start
    private PlayerMove mover;//Codigo de mover del personaje-se referencia en el Start

    public RECrosshairImage crosshair;

    //aim
    [Header("Aim")]
    public Transform aimpos;//posicion de la bolita donde llegan los disparos
    [HideInInspector]public float aimshootspeed = 40f;//velocidad de recorrido de la bolita a la pocicionda
    public LayerMask aimask;

    //Zoom
    [HideInInspector]public float currentFOV = 60f;//Enfoque de la camara
    [HideInInspector]public float zoomFOV = 40f;//Zoom
    private float smoothSpeed = 6f;//Velocidad del zoom
    
    //Datos para el animator
    private float verRotation;//variable privada que recibira la informacion de la rotacion Vertical del centro del jugador
    private float horRotation;//variable privada que recibira la informacion de la rotacion Horizontal del centro del jugador
    private float time = 4f;//Temporizador

    private bool cronometro;
    private bool disparo = false;

    void Awake()
    {
        cameraVirtual = FindObjectOfType<CinemachineVirtualCamera>();
        centerPlayer = FindObjectOfType<PlayerCamera>();
        aimpos = GameObject.FindGameObjectWithTag("AimSphere").transform;
    }

    void Start()
    {

        crosshair = FindObjectOfType<RECrosshairImage>();
        animator = GetComponent<Animator>();
        mover = GetComponent<PlayerMove>();
    }


    void Update()
    {

        verRotation = centerPlayer.verRotation;
        //horRotation = playerCamera.horRotation;

        Aim();
        Zoom();
        Animation_Shooting_standing();
        if(cronometro){
            Cronometer();
        }


    }

    private void Aim(){

        //en un vector2 se especifica donde esta el centro de la camara dividiendolo en dos la anchura y altura
        Vector2 centerScreen = new Vector2 (Screen.width/2, Screen.height/2);

        //se coloca el origen de donde saldra el rayo
        Ray ray= Camera.main.ScreenPointToRay(centerScreen);

        //si se lanza el rayo
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimask)){

            //se le da posicion de la bolita donde esta el aim, posicion, hit, velocidad de pocicion
            aimpos.position = Vector3.Lerp(aimpos.position, hit.point, aimshootspeed * Time.deltaTime);

            //La mi cambia de color a rojo si se posiciona sobre el enemigo 
            if(hit.collider.CompareTag("Enemy")){
                crosshair.GetComponent<Image>().color = new Color(255,0,0);
            }
            else{
                //Sino se queda en blanco
                crosshair.GetComponent<Image>().color = new Color(255,255,255);
            }
        }

    }

    private void Zoom(){

        //si se aplasta click derecho
        if(Input.GetMouseButton(1) && Time.timeScale != 0){
            //se le suma a la camara en el lens en su fov la velocidad
            cameraVirtual.m_Lens.FieldOfView -=smoothSpeed; 
        }
        else{
            //se le resta a la camara en el lens en su fov la velocidad
            cameraVirtual.m_Lens.FieldOfView +=smoothSpeed;
        }
        //limita cua es el maximo y el minimo de zoom
        cameraVirtual.m_Lens.FieldOfView = Mathf.Clamp(cameraVirtual.m_Lens.FieldOfView, zoomFOV, currentFOV);

    }

    //funcion de si se dispara y esta parado sin moverse
    private void Animation_Shooting_standing(){;

        // si se dispara o el temporizador es mayor a 4
        if(Input.GetMouseButtonDown(0)){

            disparo = true;
            if(time >= 4f && disparo || time <=3.9f && time >=0.2f && disparo){

                time = 4;
                //se aplica la mascara de disparo
                animator.SetLayerWeight(1,1f);

                //se le pasa los valores  verticales al animator
                animator.SetFloat("Aim", verRotation);

                cronometro = true;
                disparo = false;
            }
        }

        //el quita la mascara, detiene el cronometro y resetea el valor de time
        if(time < 0.1f){
            cronometro = false;
            animator.SetFloat("Aim",0);
            animator.SetLayerWeight(1,0);
            time = 4f;
        }
    }

    //Cronometro para quitar la mascara del disparo
    private void Cronometer(){

        //se resta a cada frame
        time -= Time.deltaTime;

    }
}
