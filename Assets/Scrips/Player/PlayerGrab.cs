using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{

    //Arma
    public GameObject weapon;
    //hueso mano
    public Transform hand;
    //si se activa o no
    private bool activo;


    void Update()
    {
        if(activo == true)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                //emparenta el arma a la mano e iguala las pocisiones y rotaciones
                weapon.transform.SetParent(hand);
                weapon.transform.position = hand.position;
                weapon.transform.rotation = hand.transform.rotation;
            }
        }
    }

    //funcionde colicion
    private void OnTriggerEnter(Collider other)
    {
        //si es el jugador bool es verdadero
        if (other.tag == "Player")
        {
            activo = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            activo = false;
        }
    }
}
