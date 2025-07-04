using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealt : MonoBehaviour
{
    [SerializeField]private int baseHealt;
    [SerializeField]private int healt;
    
    public bool life = true;

    public EnemyAIController enemy;

    void Start(){
        healt=baseHealt;
    }

    // Update is called once per frame
    void Update()
    {
        if (healt <= 0)
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
            
            healt--;

        }
    }

    public void Revivir()
    {
        life = true;
        healt = baseHealt;
        enemy.agent.isStopped = false;
        GetComponent<Renderer>().material.color = new Color(255, 255, 255);
    }
}
