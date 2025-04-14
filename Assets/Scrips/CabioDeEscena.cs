using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    public Transform puntoinicial;
    public GameObject jugador;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");
        puntoinicial = GameObject.FindGameObjectWithTag("PuntoInicial").transform;
        MoverAPuntoInicial();
    }

    public void MoverAPuntoInicial(){
        jugador.transform.position = puntoinicial.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            if(SceneManager.GetActiveScene().buildIndex == 0){
                SceneManager.LoadScene(1);
            }
            else{
                SceneManager.LoadScene(0);
            }
        }
    }
}
