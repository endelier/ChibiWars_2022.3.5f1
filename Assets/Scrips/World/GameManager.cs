using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CI.QuickSave;
using UnityEditor;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    //variable del propio game manager
    public static GameManager Instance{get;private set;}

    [Header("Game level")]
    public int level;

    [Header("Canvas")]//variable del canvas texs
    public Text ammonText;
    public Text healtText;
    public Slider healBar;

    //Municion
    public int gunAmmo;

    [Header("Objectos")]//Instancias
    public Transform starposition;
    private string nameCharacter;
    private string namePrimary;
    private string nameSecondary;
    private Object characterObject;//object hace referencia a cualquier objeto de los assets pero no de la escena
    private Object primaryWeapon;
    private Object secondaryWeapon;
    
    //Estadisticas jugador
    public int healt;

    private void Awake()
    {
        Instance = this;

        //variable lecto leera el archivo equipamento
        var lector = QuickSaveReader.Create("Equipament");

        // en el string se guardara la variable de lector el nombre del personaje
        nameCharacter = lector.Read<string>("Character");
        namePrimary = lector.Read<string>("WeaponPrimary");
        nameSecondary = lector.Read<string>("WeaponSecondary");

        //busca el objeto en la direccion dada del proyecto
        characterObject = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Character/"+nameCharacter+".prefab", typeof(Object)) as GameObject;
        primaryWeapon = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Weapons/"+namePrimary+"/"+namePrimary+".prefab", typeof(Object)) as GameObject;
        secondaryWeapon = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Weapons/"+nameSecondary+"/"+nameSecondary+".prefab", typeof(Object)) as GameObject;

        //instancia los objetos
        Object characterInstatiate = Instantiate (characterObject, starposition.position,Quaternion.identity);
        Object weaponPri = Instantiate (primaryWeapon, starposition.position,Quaternion.identity);
        Object weaponSeco = Instantiate (secondaryWeapon, starposition.position,Quaternion.identity);

        //Activa o desactiva componentes segun el tipo de nivel que sea: 0-neutral, 1-ataque
        if(level == 0){

            characterInstatiate.GetComponent<PlayerAim>().enabled = false;
            characterInstatiate.GetComponent<PlayerWeaponSwich>().enableswich = false;
            characterInstatiate.GetComponent<PlayerStatistics>().enabled = false;

            weaponPri.GetComponent<Weapon>().enabled = false;
            weaponSeco.GetComponent<Weapon>().enabled = false;

        }
        if(level == 1){

            characterInstatiate.GetComponent<PlayerAim>().enabled = true;
            characterInstatiate.GetComponent<PlayerWeaponSwich>().enableswich = true;
            characterInstatiate.GetComponent<PlayerStatistics>().enabled = true;

            weaponPri.GetComponent<Weapon>().enabled = true;
            weaponSeco.GetComponent<Weapon>().enabled = false;
        }

    }
    void Start()
    {
        if(level == 0)
        {
        }
        if(level == 1){

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
        if(level==1){
            //convierte la informacion de int a string y las envia al canvas
            /*ammonText.text = gunAmmo.ToString();*/
            healtText.text = healt.ToString();

            SaveHealt();
            DrawHealt();
        }
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
