using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using Unity.VisualScripting;
using Cinemachine;

public class SaveLobby : MonoBehaviour
{
    [Header("Camara")]
    public CinemachineVirtualCamera cameraVirtual;
    private Vector3 cameraPosicion = new Vector3 (1.3f,0.31f,-0.7f);//configuracion actial de la posicion de la camara

    [Header("Player Things")]
    public Transform equipamentZone;//zona del arcenal y donde mira la camara
    public Transform equipamentPosition;//punto se repociciona y creal al personaje
    public Transform follorPlayer;
    public GameObject characterPlayer;

    [Header("MenuSystem")]
    public MenuSystemEquipament menuSysEqui;//se llama al UI del arcenal

    //otros
    private bool zone = false;
    public bool leyendo = false;

    string characterName;
    string stadistics;

    void Start()
    {
        follorPlayer = FindObjectOfType<PlayerCamera>().transform;
        characterPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {

        //posicion en 0 -550, posicion en 150 -250
        //si esta en la zona y aplasta c leera las estadisticas del personaje
        if(zone && Input.GetKeyUp(KeyCode.X)){
            //Lectura();
            ReposicionActivdo();
            leyendo = true;
            characterName = characterPlayer.GetComponent<AutoReference>().nombre;
        }
        if(zone && Input.GetKeyUp(KeyCode.Escape)){
            Escritura();
            ReposicionDesactivado();
            leyendo = false;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if(other.gameObject.CompareTag("Player")){
            zone = true;
        }
        else{
            zone = false;
        }
    }
    
    private void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            zone = false;
        }
        else{
            zone = false;
        }
    }

    private void ReposicionActivdo(){
        //mira al arcenal la camara
        cameraVirtual.m_Follow = equipamentZone.transform;
        //Repociciona el hombro
        cameraVirtual.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset = new Vector3 (0,-0.5f,-1.5f);
        //Desactiva el controlador del personaje, movimiento, y animaciones
        characterPlayer.GetComponent<CharacterController>().enabled=false;
        characterPlayer.GetComponent<PlayerMove>().enabled=false;
        characterPlayer.GetComponent<Animator>().SetBool("Walk", false);
        characterPlayer.GetComponent<Animator>().SetBool("Jump", false);
        characterPlayer.GetComponent<Animator>().SetBool("onAir", false);
        characterPlayer.GetComponent<Animator>().SetBool("Fall", true);
        //repociciona al personaje
        characterPlayer.transform.position = equipamentPosition.transform.position;
        characterPlayer.transform.rotation = equipamentPosition.transform.rotation;
    }

    private void ReposicionDesactivado(){
        //camara vuelve a mirar el centro del personjae
        cameraVirtual.m_Follow = follorPlayer;
        //busca al nuevo personaje jugable
        characterPlayer = GameObject.FindGameObjectWithTag("Player");
        //regresa la pocicion original del homnbro
        cameraVirtual.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset = cameraPosicion;
        //reactiva el controlador de personajes y el movimiento
        characterPlayer.GetComponent<CharacterController>().enabled = true;
        characterPlayer.GetComponent<PlayerMove>().enabled = true;
    }



    //lecura solo debe leer la lista de personajes CSV
    /*private void Lectura(){

        Debug.Log("leyendo");

        var lector = QuickSaveReader.Create(stadistics);

        characterName = lector.Read<string>("Name");
        //falta leer y guardar estadistics aplastando una tecla, que el gameManager lea las estadisticas y las aplique
    }*/

    //escritura guarda la informacion del personaje seleecionado en equipamiento
    private void Escritura(){

        var escritor = QuickSaveWriter.Create("Equipament");

        escritor.Write("Character", characterName);

        escritor.Commit();

    }


    /*private void EscrituraCSV(){
        var csv = new QuickSaveCsv();

        csv.SetCell(0,0, "Rhan.prefab");
        csv.SetCell(1,0, "EvilRhan.prefab");

        csv.Save($"{Application.persistentDataPath}/Characters.csv");
    }*/
}
