using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cronometer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textCrono;
    [SerializeField] private float tempo;

    private int tempoMin, tempoSecon, tempoDeci;
    private bool timeStop;

    // Update is called once per frame
    void Update()
    {
        Cronometro();
    }

    void Cronometro(){

        if(!timeStop){
            //el cronometro va de mayor a menor, si quiero que aumente solo se cambia el simbolo - por +
            tempo -= Time.deltaTime;
        }

        //saca el entero 
        //Minutos es igual a tiempo entre 60
        tempoMin = Mathf.FloorToInt(tempo/60);
        //modulo de 60
        tempoSecon = Mathf.FloorToInt(tempo%60);
        //modulo de 1 x 100
        tempoDeci = Mathf.FloorToInt((tempo%1)*100);

        //-----se le da un formato con la que en la posicion  deja dos espacio 
        textCrono.text = string.Format("{0:00}:{1:00}:{2:00}", tempoMin, tempoSecon, tempoDeci);

        if(tempo <= 0){
            timeStop=true;
            tempo=0;

        }
    }
}
