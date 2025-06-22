using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionObjective : MonoBehaviour
{

    [Header("Stadistics")]
    public float objectiveLife;
    [Range(1.0f, 0.1f)]
    public float objectiveArmor;
    public int objectiveLifeUI;

    public Text missionObjetiveText;

    [HideInInspector] public int bulletDamage;

    void Start()
    {

        //le envia la vida al text del objetivo
        missionObjetiveText = FindAnyObjectByType<ObjectiveMission>().text;
        missionObjetiveText.text = objectiveLife.ToString();
    }

    //Colicion de la bala con el objetivo
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            bulletDamage = collision.gameObject.GetComponent<Bullets>().damageBullet;
            HealthObjective(bulletDamage);
        }
    }

        public void HealthObjective(int bulletDamage)
    {
        //vida del objetivo -= bala x armadura
        objectiveLife -= bulletDamage * objectiveArmor;
        //de float se convierte en int
        objectiveLifeUI = (int)objectiveLife;
        //convierte la informacion de int a string y las envia al canvas
        missionObjetiveText.text = objectiveLifeUI.ToString();
    }
}
