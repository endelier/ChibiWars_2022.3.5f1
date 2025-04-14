using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CabioDeEscena : MonoBehaviour
{
    public Transform puntoinicial;
    public GameObject jugador;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        puntoinicial = GameObject.FindGameObjectWithTag("PuntoInicial").transform;
        MoverAPuntoInicial();
    }

    void Update()
    {
        ProbarCambioDeEscena();   
    }

    public void MoverAPuntoInicial(){
        jugador.transform.position = puntoinicial.position;
    }

    public void ProbarCambioDeEscena(){

        if(Input.GetKeyUp(KeyCode.L)){
            if(SceneManager.GetActiveScene().buildIndex == 0){
                SceneManager.LoadScene(1);
            }
            else{
                SceneManager.LoadScene(0);
            }
        }

    }
}
