using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    //Nesh navegation
    [Header("Navegation Mesh")]
    public NavMeshAgent navMeshAgent;/// agente del enemigo

    public Transform[] destinations;//Destinos
    public float distanceToFollowPath = 2;//Distancia para cambiar de destino

    [Header("Jugador")]
    [SerializeField] private GameObject player;//Referencia del jugador
    public bool followplayer = true;//si sigue al jugador

    public float distanceToPlayer;//distancia que hay entre el enemigo y el jugador
    private float distanceToFollowPlayer = 10; //Distancia para comenzar a seguir al jugador


    //cambiar color al recibir el disparo
    private float cronometer = 2.5f;//temporizador de cronometro
    private bool accion = false;//booleano de si resive la bala

    [Header("Stadistics")]
    public int healt = 3;
    public bool life = true;


    void Start()
    {
        player = FindAnyObjectByType<PlayerMove>().gameObject;
        //navMeshAgent.destination = destinations[0].transform.position;
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log(distanceToFollowPlayer);

        if (distanceToPlayer <= distanceToFollowPlayer && followplayer)
        {
            Debug.Log("Siguindo");
            FollowPlyer();
        }
        else
        {
            //EnemyPath();
        }

        if (healt <= 0)
        {
            life = false;
        }

        Cronometer();

    }


    /*public void EnemyPath(){

        navMeshAgent.destination = destinations[i].position;

        if(Vector3.Distance(transform.position,destinations[i].position) <= distanceToFollowPath){

            if(destinations[i] != destinations[destinations.Length - 1]){
                i++;

            }
            else{
                i=0;
            }
        }
    }*/

    public void FollowPlyer()
    {

        navMeshAgent.destination = player.transform.position;
    }

    //funcion de cronometro para regresarle el color
    private void Cronometer()
    {

        //si recivio la bala
        if (accion)
        {

            //cronometro empieza a subir
            cronometer += Time.deltaTime;

            //limitador para que el cronometro no suba mas
            if (cronometer > 2.5f)
            {
                cronometer = 2.5f;
            }

            //si el cronometro es 2.5f cambia de color a rojo
            if (cronometer == 2.5f)
            {
                GetComponentInChildren<Renderer>().material.color = new Color(255, 0, 0);

                //cambia accion a falso de que no a recivido bala
                accion = false;
            }
        }

    }

    //funcion de rescibir un disparo
    private void OnCollisionEnter(Collision collision)
    {
        //di es impactado por una bala
        if (collision.gameObject.CompareTag("Bullet"))
        {

            //cambia de color a verde
            GetComponentInChildren<Renderer>().material.color = new Color(0, 255, 0);

            //destruye la bala
            collision.gameObject.SetActive(false);

            healt--;

            //inicia cronometro y acciona que si recivio la bala
            //followplayer = true;
            cronometer = 0;
            //accion = true;
        }
    }

    public void Revivir()
    {
        life = true;
        healt = 3;
    }
}
