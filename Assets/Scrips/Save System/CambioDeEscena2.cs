using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena2 : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            
            //Si esta en lobby carga nivel debajo del agua
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                SceneManager.LoadScene(1);
            }
            //sino carga el lobby
            else
            {
                SceneManager.LoadScene(0);
            }   
        }
    }
}
