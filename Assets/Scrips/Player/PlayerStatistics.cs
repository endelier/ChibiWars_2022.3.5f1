using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{

    //Este scrip es el que se cominica con el GameManager al iniciar el juego, solo envia envia informacion
    //Estadisticas del jugador
    public int healt = 200;
    public int armor;
    public int sheld;

    //al iniciar el juego le pasa las estadisticas al GameManager y es todo lo que hace el codigo
    void Start()
    {
        GameManager.Instance.healt = healt;
    }

}
