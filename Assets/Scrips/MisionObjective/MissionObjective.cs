using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionObjective : MonoBehaviour
{

    [Header("Stadistics")]
    public float objectiveLife;
    [Range(0.1f, 1.0f)]
    public float objectiveArmor = 0.7f;
    public int objectiveLifeUI;

    public Text missionObjetiveText;

    [HideInInspector] public int bulletDamage;

    void Start()
    {
        //le envia la vida al text del objetivo
        missionObjetiveText = FindAnyObjectByType<REObjectiveMissionText>().text;
        //manda la vida del objetivo al inicio
        missionObjetiveText.text = objectiveLife.ToString();
    }

    //Colicion de la bala con el objetivo
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            bulletDamage = collision.gameObject.GetComponent<EnemyBullet>().damageBullet;
            HealthObjective(bulletDamage);
        }
    }

    public void HealthObjective(int bulletDamage)
    {
        //vida del objetivo -= bala x armadura
        objectiveLife -= bulletDamage * objectiveArmor;
        //de float se convierte en int
        objectiveLifeUI = (int)objectiveLife;

        //tope para que la vida no baje mas que cero
        if (objectiveLifeUI <= 0)
        {
            objectiveLifeUI = 0;
            objectiveLife = 0;
        }

        //convierte la informacion de int a string y las envia al canvas
        missionObjetiveText.text = objectiveLifeUI.ToString();
    }
    
}
