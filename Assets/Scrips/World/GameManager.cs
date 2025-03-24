using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //variable del propio game manager
    public static GameManager Instance{get;private set;}

    //variable del canvas texs
    public Text ammonText;
    public Text healtText;
    public Slider healBar;

    //Municion
    public int gunAmmo;

    //Estadisticas jugador
    public int healt;

    private void Awake()
    {
        Instance = this;

    }
    void Start()
    {
        //Llama la vida y se lo envia al slider
        healBar.maxValue=healt;
        healBar.value=healt;

        /*Inicia la corrutina para enviarlos datos,  porque los starts inicial al mismo tiempo y los valores
        se pasa a 0*/
        StartCoroutine("ValuesData");

        //Si la vida maxima es mayor a 0 detiene la corrutina
        if(healBar.maxValue > 0){
            StopCoroutine("ValuesData");
        }

    }

    IEnumerator ValuesData(){

        //Se detiene la corrutina por 0.1
        yield return new WaitForSeconds(0.1f);


        //El slider recive la informacion
        healBar.maxValue = healt;
        healBar.value = healt;

    }

    void Update()
    {
        //convierte la informacion de int a string y las envia al canvas
        ammonText.text = gunAmmo.ToString();
        healtText.text = healt.ToString();

        SaveHealt();
        DrawHealt();
    }

    //Funcion para perder vida
    public void LoseHealth(int healtToReduce){

        healt -= healtToReduce;
        healBar.value=healt;
    }

    //Si la vida es 0, se reinicia el nivel
    public void SaveHealt(){
        if(healt<=0){
            Debug.Log("Has muerto");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //Dibuja en el slider;
    public void DrawHealt(){
        healBar.value=healt;
    }

}
