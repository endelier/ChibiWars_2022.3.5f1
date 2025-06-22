using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealt : MonoBehaviour
{
    [SerializeField]private int healt = 3;
    public bool life = true;

    public EnemyAIController enemy;

    // Update is called once per frame
    void Update()
    {
        if (healt <= 3)
        {
            life = false;
            enemy.AlertNearbyEnemies(transform.position, 10f);
            GetComponent<Renderer>().material.color = new Color(0, 0, 0);
            enemy.agent.isStopped = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //di es impactado por una bala
        if (collision.gameObject.CompareTag("Bullet"))
        {
            enemy.isAlerted = true;

            //cambia de color a verde
            GetComponent<Renderer>().material.color = new Color(0, 255, 0);

            //destruye la bala
            collision.gameObject.SetActive(false);

            //StartCoroutine(AlertDecisionRoutine());
            
            healt--;

        }
    }
}
