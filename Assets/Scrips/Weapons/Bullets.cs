using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public int damageBullet = 25;

    //Cuando se activa la bala
    private void OnEnable()
    {
        // Invoka la funcion para que se desactive en 20 segundos
        Invoke(nameof(DisableBullet), 20f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Aquí puedes poner lógica de daño al enemigo, partículas, etc.
        
        //Cuando choca con el enemigo 
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DisableBullet();
        }
    }

    //Funcion de desactivar la bala para que vuelva al pool llamada desde el Invoke por si no choco con nada
    private void DisableBullet()
    {
        gameObject.SetActive(false);
    }

    //funcion que se activa cuando SetActive(false);
    private void OnDisable()
    {
        // Cancela el Invoke por si la bala se desactiva antes (por colisión)
        CancelInvoke();
    }
}
