using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Underwater : MonoBehaviour
{
    public bool door_enable=true;
    public Animator puerta_anima;
    public GameObject puerta1;
    public GameObject puerta2;
    public GameObject puerta3;

    private void OnTriggerEnter(Collider other)
    {
        if(door_enable){
            if(other.gameObject.CompareTag("Player")||other.gameObject.CompareTag("Enemy")){
                puerta_anima.Play("Armature_Door|Open");
                puerta1.GetComponent<MeshCollider>().enabled = false;
                puerta2.GetComponent<MeshCollider>().enabled = false;
                puerta3.GetComponent<MeshCollider>().enabled = false;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
         if(door_enable){
            if(other.gameObject.CompareTag("Player")||other.gameObject.CompareTag("Enemy")){
                puerta_anima.Play("Armature_Door|Close");
                puerta1.GetComponent<MeshCollider>().enabled = true;
                puerta2.GetComponent<MeshCollider>().enabled = true;
                puerta3.GetComponent<MeshCollider>().enabled = true;
            }
        }
    }
}
