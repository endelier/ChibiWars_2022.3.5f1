using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    //Nesh navegation    
    public NavMeshAgent navMeshAgent;

    public Transform[] destinations;
    public float distanceToFollowPath = 2;

    private int i = 0;

    [Header("Jugador")]
    public bool followplayer;
    private GameObject player;

    private float distanceToPlayer;
    private float distanceToFollowPlayer = 10;


    //cambiar color
    [SerializeField]private float cronometer = 2.5f;
    [SerializeField]private bool accion=false;


    void Start()
    {
        navMeshAgent.destination = destinations[0].transform.position;
        player = FindAnyObjectByType<PlayerMove>().gameObject;
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToPlayer <= distanceToFollowPlayer && followplayer){
            FollowPlyer();
        }
        else{
            EnemyPath();
        }

        Cronometer();
    }


    public void EnemyPath(){

        navMeshAgent.destination = destinations[i].position;

        if(Vector3.Distance(transform.position,destinations[i].position) <= distanceToFollowPath){

            if(destinations[i] != destinations[destinations.Length - 1]){
                i++;

            }
            else{
                i=0;
            }
        }
    }

    public void FollowPlyer(){

        navMeshAgent.destination = player.transform.position;
    }


    //funcion cromonetrada
    private void Cronometer(){

        //si recivio la bala
        if(accion){

            //cronometro empieza a subir
            cronometer+=Time.deltaTime;

            //limitador para que el cronometro no suba mas
            if(cronometer > 2.5f){
                cronometer = 2.5f;
            }

            //si el cronometro es 2.5f cambia de color a rojo
            if(cronometer == 2.5f){
                GetComponent<Renderer>().material.color = new Color(255,0,0);

                //cambia accion a falso de que no a recivido bala
                accion=false;
            }
        }

    }

    //funcion de rescibir un disparo
    private void OnCollisionEnter(Collision collision)
    {
        //di es impactado por una bala
        if(collision.gameObject.CompareTag("Bullet")){

            //cambia de color a verde
            GetComponent<Renderer>().material.color = new Color(0,255,0);

            //destruye la bala
            Destroy(collision.gameObject);

            //inicia cronometro y acciona que si recivio la bala
            cronometer = 0;
            accion = true;
        }
    }
}
