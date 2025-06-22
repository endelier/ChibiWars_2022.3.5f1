using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CI.QuickSave;
using UnityEditor;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    //variable del propio game manager
    public static GameManager Instance { get; private set; }

    [Header("Game level")]
    public int level;

    [SerializeField]private GameManagerData GMS;

    [Header("Canvas")]//variable del canvas texs
    public Text ammonText;
    public Text healthText;
    public Slider healthBar;
    //public Text missionObjetiveText;

    [Header("Objectos")]//Instancias
    public Transform starposition;

    //Cosas del jugador y armas
    private string nameCharacter;
    private string namePrimary;
    private string nameSecondary;
    private Object characterObject;//object hace referencia a cualquier objeto de los assets pero no de la escena
    private Object primaryWeapon;
    private Object secondaryWeapon;

    [Header("Estadisticas del Jugador")]
    //Estadisticas jugador
    public float healthPlayer;
    public float armorPlayer;
    public int healthPlayerUI;


    [Header("Mision")]
    public int missionWin = 0;// 0=null, 1=win,2=lose

    //otros
    private float contador;//Contador vidas jugador

    private void Awake()
    {
        // Asegurarse de que solo haya una instancia
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Elimina duplicados
        }

        Instance = this;

        GMS.Level(level);

        //variable lecto leera el archivo equipamento
        var lector = QuickSaveReader.Create("Equipament");

        // en el string se guardara la variable de lector el nombre del personaje
        nameCharacter = lector.Read<string>("Character");
        namePrimary = lector.Read<string>("WeaponPrimary");
        nameSecondary = lector.Read<string>("WeaponSecondary");

        //busca el objeto en la direccion dada del proyecto
        characterObject = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Character/" + nameCharacter + ".prefab", typeof(Object)) as GameObject;
        primaryWeapon = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Weapons/" + namePrimary + "/" + namePrimary + ".prefab", typeof(Object)) as GameObject;
        secondaryWeapon = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Weapons/" + nameSecondary + "/" + nameSecondary + ".prefab", typeof(Object)) as GameObject;

        //instancia los objetos
        Object characterInstatiate = Instantiate(characterObject, starposition.position, Quaternion.identity);
        Object weaponPri = Instantiate(primaryWeapon, starposition.position, Quaternion.identity);
        Object weaponSeco = Instantiate(secondaryWeapon, starposition.position, Quaternion.identity);

        //Activa o desactiva componentes segun el tipo de nivel que sea: 0-neutral, 1-ataque
        if (level == 0)
        {

            characterInstatiate.GetComponent<PlayerAim>().enabled = false;
            characterInstatiate.GetComponent<PlayerWeaponSwich>().enableswich = false;
            characterInstatiate.GetComponent<PlayerStatistics>().enabled = false;

            weaponPri.GetComponent<Weapon>().enabled = false;
            weaponSeco.GetComponent<Weapon>().enabled = false;

            //se pasa los objectos :D
            /*Cosa(characterInstatiate);
            Cosa(weaponPri);*/

        }
        if (level == 1)
        {
            characterInstatiate.GetComponent<PlayerAim>().enabled = true;
            characterInstatiate.GetComponent<PlayerWeaponSwich>().enableswich = true;
            characterInstatiate.GetComponent<PlayerStatistics>().enabled = true;

            weaponPri.GetComponent<Weapon>().enabled = true;
            weaponSeco.GetComponent<Weapon>().enabled = false;

            //pasa la informacion de las estadisticas del jugador al UI
            healthPlayer = characterInstatiate.GetComponent<PlayerStatistics>().healt;
            healthBar.maxValue = healthPlayer;
            healthBar.value = healthPlayer;
            healthText.text = healthPlayer.ToString();
        }

    }

    void Start()
    {
    }

    void Update()
    {
        if (level == 1)
        {
            SaveHealth();
            Perder();

            //Debug.Log(contador);
        }
    }

    //--------------------------------------------------Player Functions---------------------------
    public void HealthPlayer(int bulletdamage)
    {
        //vida = bala x armadura
        healthPlayer -= bulletdamage * armorPlayer;
        //de float se convierte en int
        healthPlayerUI = (int)healthPlayer;
        //healBar.value = healtPlayer;
        healthBar.value = healthPlayer;
        if (healthPlayerUI < 0)
        {
            healthPlayerUI = 0;
        }
        //convierte la informacion de int a string y las envia al canvas
        healthText.text = healthPlayerUI.ToString();
    }

    //Si la vida es 0, se reinicia el nivel
    public void SaveHealth()
    {
        if (healthPlayer <= 0)
        {
            Debug.Log("Has muerto");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //--------------------------------------------------Missions Functions---------------------------
    //Defensa
    /*//se Recive la informacion del daño de la bala y se le reduce a la vida del objetivo
            public void HealthObjective(int bulletDamage)
    {
        //vida del objetivo -= bala x armadura
        objectiveLife -= bulletDamage * objectiveArmor;
        //de float se convierte en int
        objectiveLifeUI = (int)objectiveLife;
        //convierte la informacion de int a string y las envia al canvas
        //missionObjetiveText.text = objectiveLifeUI.ToString();
    }*/

    private void Perder()
    {
        if (missionWin == 2)
        {
            Debug.Log("Has perdido");
            Tiempo();
            if (contador >= 10)
            {
                SceneManager.LoadScene(0);
            }

        }
    }

    void Tiempo()
    {
        contador = contador + 1 * Time.deltaTime;
    }
}
