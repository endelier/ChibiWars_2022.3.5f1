using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Level_1 : MonoBehaviour
{

    [Header("Pool Enemies")]
    public GameObject enemyPrefab;//Referencia del prefab del enemigo
    public int cantidadEnemigos = 6;//Cantidad de enemigos a instanciar

    //Lista de enemigos vivos y muertos
    private List<GameObject> enemigosVivos = new List<GameObject>();
    private List<GameObject> enemigosMuertos = new List<GameObject>();

    //Matriz de puntos de spawn para los enemigos
    private Transform[] spawnPoints;
    private int spawnIndexPos = 0;//index de posicion del spawn
    private int spawnIndexRota = 0;//index de rotacion del spawn

    public MissionObjective objective;

    void OnEnable()
    {
        //Referencia del enemigo escrito por script
        enemyPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemies/Enemigo_Capsule.prefab", typeof(object)) as GameObject;
    }

    void Start()
    {
        //Busca en la escena el objeto con el componente MissionObjetive
        //objective = FindAnyObjectByType<MissionObjective>();

        // Obtener los 3 puntos de spawn con el tag "EnemySpawn"
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("EnemySpawn");

        //lo mete a un Array de transforms
        spawnPoints = new Transform[spawns.Length];
        for (int i = 0; i < spawns.Length; i++)
        {
            spawnPoints[i] = spawns[i].transform;
        }

        // Instanciar enemigos inicialmente y agregarlos a la lista de vivos
        /*for (int i = 0; i < cantidadEnemigos; i++)
        {
            //instancea es igual a (enemigo, posicion retornada de la funcion, rotacion)
            GameObject instancia = Instantiate(enemyPrefab, GetNextSpawnPosition(), GetNextSpawnRotation());

            enemigosVivos.Add(instancia);
        }---------------------------------*/
    }

    void Update()
    {
        /*
        // Comprobar enemigos muertos; i = lista de enemios vivos, i sea mayor o igual a 0, i-1
        for (int i = enemigosVivos.Count - 1; i >= 0; i--)
        {
            Enemy script = enemigosVivos[i].GetComponent<Enemy>();

            //si enemigo no es null y vida es falsa, lo pasa a muerto
            if (script != null && !script.life)
            {
                //Pasa el indice
                MoverAMuertos(i);
            }
        }

        //Busca en la escena el objeto con el componente MissionObjetive
        objective = FindAnyObjectByType<MissionObjective>();
        Debug.Log(objective.objectiveLife);
        */
    }
/*
    void MoverAMuertos(int index)
    {
        //iguala un objeto al objeto enemigo indice
        GameObject enemigo = enemigosVivos[index];

        //remueve de la lista de vivos y lo pasa a la lista de muertos
        enemigosVivos.RemoveAt(index);
        enemigosMuertos.Add(enemigo);

        //le desactiva
        enemigo.SetActive(false);

        //inicia corrutina para revivirlo
        StartCoroutine(RevivirEnemigoDespuesDeTiempo(enemigo, 10f));
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

        //si enemigo no es nulo accede a su funcion de revivir y la vida pasa a verdadero y le da 3 vidas
        Enemy script = enemigo.GetComponent<Enemy>();
        if (script != null)
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
*/
}
