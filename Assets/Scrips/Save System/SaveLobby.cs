using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using Unity.VisualScripting;
using Cinemachine;

public class SaveLobby : MonoBehaviour
{
    public CinemachineVirtualCamera cameraVirtual;
    public CinemachineVirtualCamera cameraVirtual2;
    public Transform equipamentZone;
    public Transform follorPlayer;

    private Vector3 posicion = new Vector3 (1.3f,0.31f,-0.7f);
    string stadistics;
    public bool zone = false;
    public bool leyendo = false;

    string characterName;

    void Start()
    {
        follorPlayer = FindObjectOfType<PlayerCamera>().transform;
    }

    private void Update()
    {
        //si esta en la zona y aplasta c leera las estadisticas del personaje
        if(zone && Input.GetKeyUp(KeyCode.X)){
            //Lectura();
            Debug.Log("En zona");
            cameraVirtual.m_Follow = equipamentZone.transform;
            cameraVirtual.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset = new Vector3 (0,0,0);
            leyendo = true;
        }
        if(Input.GetKeyUp(KeyCode.Escape)){
            //Escritura();
            cameraVirtual.m_Follow = follorPlayer;
            cameraVirtual.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset = posicion;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Player")){
            zone = true;
            //sacando la referencia del objeto a su estadisticas guardadas en un json donde esta el nombre del personaje
            //stadistics = other.gameObject.GetComponent<AutoReference>().characterStadistics;
        }
        else{
            zone = false;
        }

    }
    /*private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && leyendo){
            zone = false;
            Escritura();
        }
        else{
            zone = false;
        }
        Debug.Log("salio");
    }

    private void Lectura(){

        Debug.Log("leyendo");

        var lector = QuickSaveReader.Create(stadistics);

        characterName = lector.Read<string>("Name");
        //falta leer y guardar estadistics aplastando una tecla, que el gameManager lea las estadisticas y las aplique
    }

    private void Escritura(){

        var escritor = QuickSaveWriter.Create("Equipament");

        escritor.Write("Character", characterName);

        escritor.Commit();

    }*/
}
