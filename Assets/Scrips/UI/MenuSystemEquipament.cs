using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuSystemEquipament : MonoBehaviour
{
    [Header("UI")]
    public Canvas menu;
    public Image imagenEquipamiento;
    public Image imagenCharacter;
    public Image imagenWeaponPrimary;
    public Image imagenWeaponSecondary;

    [Header("SaveLobby")]
    public SaveLobby saveLobby;//se llama al sistema de salvado

    [Header("Other")]
    public bool readingbool = false;
    public int level = 0;//en que nivel del menu se esta

    private float position1 =-550;//posicion de vector cerrado
    private float position2 =-550;//posicion de vector abierto
    private float position3 =-550;//posicion de vector abierto

    //Boleanos de Menu Personaje, Arma Primaria, Arma Secundaria
    public bool charMe = false;
    public bool wePriMe = false;
    public bool weSeMe = false;

    void Update()
    {
        readingbool = saveLobby.leyendo;//sacando el leyendo del save para abrir el menu

        if (readingbool)
        {
            MenuSystem();
            CharacterMenu();
            WeaponPrimaryMenu();
        }
        else
        {
            imagenEquipamiento.GetComponent<RectTransform>().localPosition = new Vector3(-550, 0, 0);
            Cursor.lockState = CursorLockMode.Locked;
            RestandoOpen();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            MenuEscape();
        }

        //Tope para que level no baje a -1
        if (level <= 0)
        {
            level = 0;
        }
    }

    private void MenuSystem()
    {
        //si esta leyendo despliega el menu de equipamiento
        if (level == 0)
        {
            Cursor.lockState = CursorLockMode.None;//deja el cursor libre
            menu.GetComponent<Canvas>().enabled = true;
            //SumandoOpen();
            //imagenEquipamiento.GetComponent<RectTransform>().localPosition = new Vector3(position2, 0, 0);
        }
    }

    //esta funcion se activa con el boton
    private void CharacterMenu()
    {
        if (level == 1 && charMe && !wePriMe && !weSeMe)
        {
            SumandoClose();
            RestandoOpen();
            imagenEquipamiento.GetComponent<RectTransform>().localPosition = new Vector3(position2, 0, 0);
            imagenCharacter.GetComponent<RectTransform>().localPosition = new Vector3(position1, 0, 0);
        }
        else
        {
            RestandoClose();
            SumandoOpen();
            imagenEquipamiento.GetComponent<RectTransform>().localPosition = new Vector3(position2, 0, 0);
            imagenCharacter.GetComponent<RectTransform>().localPosition = new Vector3(position1, 0, 0);
        }
    }

    public void WeaponPrimaryMenu()
    {
        if (level == 1 && !charMe && wePriMe && !weSeMe)
        {
            SumandoClose();
            RestandoOpen();
            imagenEquipamiento.GetComponent<RectTransform>().localPosition = new Vector3(position2, 0, 0);
            imagenWeaponPrimary.GetComponent<RectTransform>().localPosition = new Vector3(position1, 0, 0);
        }
        else
        {
            RestandoClose();
            SumandoOpen();
            imagenEquipamiento.GetComponent<RectTransform>().localPosition = new Vector3(position2, 0, 0);
            imagenWeaponPrimary.GetComponent<RectTransform>().localPosition = new Vector3(position1, 0, 0);
        }
    }
    public void WeaponSecondaryMenu()
    {

    }
    //Suma a la posicion del vector de la imagen
    void SumandoClose()
    {
        position1 += 750 * Time.deltaTime;
        position1 = Mathf.Clamp(position1, -550, -250);
    }
    //Resta a la posicion del vector de la imagen
    void RestandoClose()
    {
        position1 -= 750*Time.deltaTime;
        position1 = Mathf.Clamp(position1, -550, -250);
    }

    void SumandoOpen()
    {
        position2 += 750 * Time.deltaTime;
        position2 = Mathf.Clamp(position2, -550, -250);
        Debug.Log(position2);

    }
    //Resta a la posicion del vector de la imagen
    void RestandoOpen()
    {
        position2 -= 750*Time.deltaTime;
        position2 = Mathf.Clamp(position2, -550, -250);
    }

    //Contadores------------------------
    private void MenuEscape()
    {
        level--;
        charMe = false;
        wePriMe = false;
        weSeMe = false;
    }

    public void MenuLevelChar()
    {
        level++;
        charMe = true;

    }
    public void MenuLevelWePri()
    {
        level++;
        wePriMe = true;
    }
    public void MenuLevelWeSe()
    {
        level++;
        weSeMe = true;
    }
}
