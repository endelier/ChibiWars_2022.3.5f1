using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthbar;

    public float maxHealth = 200;

    public float currentHealth;

    private float regenerateHealth;

    void Start()
    {
        //healthbar.maxValue=maxHealth;
        currentHealth = GameManager.Instance.healt;
        Debug.Log(currentHealth);
        Debug.Log(GameManager.Instance.healt);
    }

    void Update()
    {
        Debug.Log(currentHealth);

    }

    public void HealthBarvalue(float healtvalue){

        currentHealth -= healtvalue;
        healthbar.value = currentHealth;

    }
}
