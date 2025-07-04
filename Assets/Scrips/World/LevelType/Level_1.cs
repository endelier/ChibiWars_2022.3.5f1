using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Level_1 : MonoBehaviour
{

    [Header("Pool Enemies")]
    public GameObject enemyPrefab;//Referencia del prefab del enemigo
    public int cantidadEnemigos = 10;//Cantidad de enemigos a instanciar

    //Lista de enemigos vivos y muertos
    private List<GameObject> enemigosVivos = new List<GameObject>();
    private List<GameObject> enemigosMuertos = new List<GameObject>();

    //Matriz de puntos de spawn para los enemigos
    private Transform[] spawnPoints;
    private int spawnIndexPos = 0;//index de posicion del spawn
    private int spawnIndexRota = 0;//index de rotacion del spawn

    [Header("Scripts Refences")]
    public GameObject objetivoDefem;
    public MissionObjective objectiveLife;
    public TargetObject objectiveTargetData;
    public TriggerMissionObjective activation;
    public Text cronometerText;
    [SerializeField] private bool oneInstance = false;

    [Header("Cronometro")]

    private float tempo = 60f;
    private int cronoMin;
    private int cronoSeco;
    private int cronoDeci;

    private bool isTimeStopped;
    private bool gameReady = false;
    

    void OnEnable()
    {
        //Referencia del enemigo escrito por script
        enemyPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemies/EnemigoProvicional.prefab", typeof(object)) as GameObject;
    }

    void Start()
    {
        //Busca en la escena el objeto con el componente MissionObjetive
        //objective = FindAnyObjectByType<MissionObjective>();

        objetivoDefem = GameObject.FindGameObjectWithTag("DefenseObjective");

        objectiveLife = objetivoDefem.GetComponent<MissionObjective>();

        objectiveTargetData = objetivoDefem.GetComponent<TargetObject>();

        //se hace referencia al texto del cronometro
        cronometerText = FindAnyObjectByType<RECronometerText>().cronoText;

        //Busca el trigger del area para comenzar la mision
        activation = FindAnyObjectByType<TriggerMissionObjective>();

        // Obtener los 3 puntos de spawn con el tag "EnemySpawn"
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("EnemySpawn");

        //lo mete a un Array de transforms
        spawnPoints = new Transform[spawns.Length];
        for (int i = 0; i < spawns.Length; i++)
        {
            spawnPoints[i] = spawns[i].transform;
        }
    }

    void Update()
    {
        //cuando el jugador entra dentro del circulo instancia a los enemigos pero solo una vez
        if (activation.activation && !oneInstance)
        {
            // Instanciar enemigos inicialmente y agregarlos a la lista de vivos
            for (int i = 0; i < cantidadEnemigos; i++)
            {
                //instancea es igual a (enemigo, posicion retornada de la funcion, rotacion)
                GameObject instancia = Instantiate(enemyPrefab, GetNextSpawnPosition(), GetNextSpawnRotation());

                enemigosVivos.Add(instancia);
            }
            oneInstance = true;
            gameReady = true;
        }

        if (activation.activation && gameReady)
        {
            Cronometer();
        }

        Debug.Log("Vida UI " + objectiveLife.objectiveLifeUI);
        //si esta iniciado el juego, el tiempo esta detenido y la vida del objetivo es mayor o igual 0
        if (gameReady && isTimeStopped && objectiveLife.objectiveLife >= 0)
        {
            //le envia al GameManager que el missionWin = 1/ 0=null, 1=win, 2=lose
            GameManager.Instance.missionWin = 1;
            objectiveTargetData.SetPriority(0);
        }

        //si esta iniciado el juego, el tiempo no esta detenido y la vida del objetivo es menor o igual a 0
        if (gameReady && !isTimeStopped && objectiveLife.objectiveLife <= 0)
        {
            //le envia al GameManager que el missionWin = 1/ 0=null, 1=win, 2=lose
            GameManager.Instance.missionWin = 2;
            Debug.Log("PERDER");
            objectiveTargetData.SetPriority(0);
        }

        // Comprobar enemigos muertos; i = lista de enemios vivos, i sea mayor o igual a 0, i-1
        for (int i = enemigosVivos.Count - 1; i >= 0; i--)
        {
            EnemyHealt script = enemigosVivos[i].GetComponent<EnemyHealt>();

            //si enemigo no es null y vida es falsa, lo pasa a muerto
            if (script != null && !script.life)
            {
                //Pasa el indice
                MoverAMuertos(i);
            }
        }
    }

    void Cronometer()
    {
        if(!isTimeStopped){
            //el cronometro va de mayor a menor, si quiero que aumente solo se cambia el simbolo - por +
            tempo -= Time.deltaTime;
        }

        //saca el entero 
        //Minutos es igual a tiempo entre 60
        cronoMin = Mathf.FloorToInt(tempo/60);
        //modulo de 60
        cronoSeco = Mathf.FloorToInt(tempo%60);
        //modulo de 1 x 100
        cronoDeci = Mathf.FloorToInt((tempo%1)*100);

        //-----se le da un formato con la que en la posicion  deja dos espacio 
        cronometerText.text = string.Format("{0:00}:{1:00}:{2:00}", cronoMin, cronoSeco, cronoDeci);

        if(tempo <= 0){
            isTimeStopped=true;
            tempo=0;

        }
    }

    void MoverAMuertos(int index)
    {
        //iguala un objeto al objeto enemigo indice
        GameObject enemigo = enemigosVivos[index];

        enemigosVivos[index].GetComponent<BoxCollider>().enabled = false;

        //remueve de la lista de vivos y lo pasa a la lista de muertos
        enemigosVivos.RemoveAt(index);
        enemigosMuertos.Add(enemigo);

        StartCoroutine(DesactivarEnemigoDespuesDeTiempo(enemigo,3f));
        //le desactiva

        //inicia corrutina para revivirlo
        StartCoroutine(RevivirEnemigoDespuesDeTiempo(enemigo, 10f));
    }

    IEnumerator DesactivarEnemigoDespuesDeTiempo(GameObject enemigo, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        enemigo.SetActive(false);
    }

    //corrutina para revivirlos
    IEnumerator RevivirEnemigoDespuesDeTiempo(GameObject enemigo, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        //lo quita de la lista muertos y lo pasa a vivos
        enemigosMuertos.Remove(enemigo);
        enemigosVivos.Add(enemigo);

        //le da posicion y rotacion y lo activa
        enemigo.transform.position = GetNextSpawnPosition();
        enemigo.transform.rotation = GetNextSpawnRotation();
        enemigo.SetActive(true);
        enemigo.GetComponent<BoxCollider>().enabled = true;
        
        EnemyAIController eneAI = enemigo.GetComponent<EnemyAIController>();
        eneAI.isAlerted = false;
        eneAI.tope = false;
        eneAI.hasMadeCoverDecision=false;

        //cuando revive entra en algun estado, si hay objetivo prioritario ataca, sino patrulla
        if(eneAI.currentTarget !=null && eneAI.currentTarget.priority >= 4){
            eneAI.TransitionToState(eneAI.attackObjectiveState);
        }
        else{
            eneAI.agent.speed = eneAI.patrolDefaultSpeed;
            eneAI.TransitionToState(eneAI.patrolState);
        }

        //si enemigo no es nulo accede a su funcion de revivir y la vida pasa a verdadero y le da 3 vidas
        EnemyHealt script = enemigo.GetComponent<EnemyHealt>();
        if (script != null && !script.life)
        {
            script.Revivir();
        }
    }

    // Devuelve el siguiente posicion de punto de spawn (rotando)
    Vector3 GetNextSpawnPosition()
    {
        if (spawnPoints.Length == 0)
        {
            return Vector3.zero;
        }

        //regresa la posicion del spawn en el indice
        Vector3 pos = spawnPoints[spawnIndexPos].position;
        //avanza al siguente indice(; indice+1 entre largo del array de spawns, si se pasa regresa a 0
        spawnIndexPos = (spawnIndexPos + 1) % spawnPoints.Length;
        return pos;
    }
    
    // Devuelve el siguiente rotacion de punto de spawn (rotando)
    Quaternion GetNextSpawnRotation()
    {
        if(spawnPoints.Length == 0)
        {
            return Quaternion.identity;   
        }

        //regresa la posicion del spawn en el indice
        Quaternion rota = spawnPoints[spawnIndexRota].rotation;
        //avanza al siguente indice(; indice+1 entre largo del array de spawns, si se pasa regresa a 0
        spawnIndexRota = (spawnIndexRota + 1) % spawnPoints.Length;

        return rota;
    }

}
