using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemies : MonoBehaviour
{
    NavMeshHit hit;
    public GameObject objeto;

    public GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        RandomPositionNavMesh();
        SpawnEnemy();
    }

    Vector3 RandomPositionNavMesh()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
        if (NavMesh.SamplePosition(randomPosition, out hit, 100, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(objeto, RandomPositionNavMesh(), Quaternion.identity);
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        agent.SetDestination(player.transform.position);
    }

}
