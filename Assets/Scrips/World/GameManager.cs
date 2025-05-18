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
    public Text missionObjetiveText;

    [Header("Objectos")]//Instancias
    public Transform starposition;
    private string nameCharacter;
    private string namePrimary;
    private string nameSecondary;
    private Object characterObject;//object hace referencia a cualquier objeto de los assets pero no de la escena
    private Object primaryWeapon;
    private Object secondaryWeapon;

    [Header("Estadisticas del Jugador")]
    //Estadisticas jugador
    public float healtPlayer;
    public float armorPlayer;

    public int healtPlayerUI;


    [Header("Mision")]
    public float objectiveLife;
    public float objectiveArmor;

    public int objectiveLifeUI;

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
        if (level == 0)
        {

            characterInstatiate.GetComponent<PlayerAim>().enabled = false;
            characterInstatiate.GetComponent<PlayerWeaponSwich>().enableswich = false;
            characterInstatiate.GetComponent<PlayerStatistics>().enabled = false;

            weaponPri.GetComponent<Weapon>().enabled = false;
            weaponSeco.GetComponent<Weapon>().enabled = false;

        }
        if (level == 1)
        {
            characterInstatiate.GetComponent<PlayerAim>().enabled = true;
            characterInstatiate.GetComponent<PlayerWeaponSwich>().enableswich = true;
            characterInstatiate.GetComponent<PlayerStatistics>().enabled = true;

            weaponPri.GetComponent<Weapon>().enabled = true;
            weaponSeco.GetComponent<Weapon>().enabled = false;
            
            //pasa la informacion de las estadisticas del jugador al UI
            healtPlayer = characterInstatiate.GetComponent<PlayerStatistics>().healt;
            healBar.maxValue = healtPlayer;
            healBar.value = healtPlayer;
            healtText.text = healtPlayer.ToString();
        }

    }
    void Start()
    {
        if(level == 0)
        {
        }
        if (level == 1)
        {
            //le envia la vida al text del objetivo
            missionObjetiveText.text = objectiveLife.ToString();
        }

    }

    void Update()
    {
        if (level == 1)
        {
            SaveHealt();
        }
    }

    public void HealtPlayer(int bulletdamage)
    {
        //vida = bala x armadura
        healtPlayer -= bulletdamage * armorPlayer;
        //de float se convierte en int
        healtPlayerUI = (int)healtPlayer;
        healBar.value = healtPlayer;
        if (healtPlayerUI < 0)
        {
            healtPlayerUI = 0;
        }
        //convierte la informacion de int a string y las envia al canvas
        healtText.text = healtPlayerUI.ToString();
    }

    //Si la vida es 0, se reinicia el nivel
    public void SaveHealt()
    {
        if (healtPlayer <= 0)
        {
            Debug.Log("Has muerto");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //se Recive la informacion del daño de la bala y se le reduce a la vida del objetivo
    public void LifeObjective(int bulletDamage)
    {
        //vida del objetivo -= bala x armadura
        objectiveLife -= bulletDamage * objectiveArmor;
        //de float se convierte en int
        objectiveLifeUI = (int)objectiveLife;
        //convierte la informacion de int a string y las envia al canvas
        missionObjetiveText.text = objectiveLifeUI.ToString();
    }
}
