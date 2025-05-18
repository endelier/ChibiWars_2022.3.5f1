using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionObjective : MonoBehaviour
{

    [Header("Stadistics")]
    public int objectiveLife;
    [Range(1.0f, 0.1f)]
    public float objectiveArmor;

    [HideInInspector] public int bulletDamage;

    void Start()
    {
        //le dice que el game manager su valor es igual al valor de las estadisticas
        GameManager.Instance.objectiveLife = objectiveLife;
        GameManager.Instance.objectiveArmor = objectiveArmor;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            bulletDamage = collision.gameObject.GetComponent<Bullets>().damageBullet;
            GameManager.Instance.LifeObjective(bulletDamage);
        }
    }
}
