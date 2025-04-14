using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena2 : MonoBehaviour
{


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
