using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSystemEquipament : MonoBehaviour
{
    [Header("UI")]
    public Canvas menu;
    public Image imagenEquipamiento;
    public Image imagenCharacter;
    
    [Header("SaveLobby")]
    public SaveLobby saveLobby;//se llama al sistema de salvado

    [Header("Other")]
    public bool readingbool=false;
    public int level;//en que nivel del menu se esta
    
    void Update()
    {
        readingbool = saveLobby.leyendo;//sacando el leyendo del save para abrir el menu

        MenuSystem();
        Debug.Log(level);

    }

     void MenuSystem(){
        
        if(readingbool){
            Cursor.lockState = CursorLockMode.None;//deja el cursor libre
            menu.GetComponent<Canvas>().enabled = true;
            imagenEquipamiento.GetComponent<RectTransform>().localPosition = new Vector3(-250,0,0);
            level=1;
        }
        if(!readingbool){
            Cursor.lockState = CursorLockMode.Locked;
            menu.GetComponent<Canvas>().enabled = false;
            imagenEquipamiento.GetComponent<RectTransform>().localPosition = new Vector3(-550,0,0);
        }
    }

    //esta funcion se activa con el boton
    public void CharacterMenu(){
        Debug.Log("HOLA :D");
        imagenEquipamiento.GetComponent<RectTransform>().localPosition = new Vector3(-550,0,0);
        imagenCharacter.GetComponent<RectTransform>().localPosition = new Vector3(-250,0,0);
        //no suma pero deberia
        level+=1;
    }

    public void WeaponPrimaryMenu(){

    }
    public void WeaponSecondaryMenu(){
        
    }
}
