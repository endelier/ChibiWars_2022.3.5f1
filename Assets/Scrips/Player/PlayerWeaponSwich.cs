using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerWeaponSwich : MonoBehaviour
{
    [Header("Enable Swich")]
    public bool enableswich = true;

    [Header("Array Weapons")]
    public GameObject[] weapons = new GameObject[2];//Matriz de las armas

    private GameObject weaponPrimary;//arma primaria
    private GameObject weaponSecondary;//arma secundaria

    private int selectedWeapon=0;//numero del arma seleccionada

    [Header("Bones")]
    public Transform hand;//hueso de la mano
    public Transform back;//hueso de la espalda


    void Start()
    {

        weaponPrimary = GameObject.FindGameObjectWithTag("PrimaryWeapon");
        weaponSecondary = GameObject.FindGameObjectWithTag("SecondaryWeapon");

        //reposiciona al inicio del juego en que mano esta cada arma
        weapons[0] = weaponPrimary;
        weapons[0].transform.SetParent(hand);
        weapons[0].transform.position = hand.position;
        weapons[0].transform.rotation = hand.transform.rotation;
        weapons[0].GetComponent<Weapon>().enabled =true;

        weapons[1] = weaponSecondary;
        weapons[1].transform.SetParent(back);
        weapons[1].transform.position = back.position;
        weapons[1].transform.rotation = back.transform.rotation;
        weapons[1].GetComponent<Weapon>().enabled =false;
        
    }

    void Update()
    {
        if(enableswich){
            int previuseapon = selectedWeapon;

            if(Input.GetKeyUp(KeyCode.F)){
                if(selectedWeapon >= weapons.Length-1){
                    selectedWeapon = 0;
                }
                else{
                    selectedWeapon++;
                }
            }
            if(previuseapon != selectedWeapon){
                SelecWeapon();
            }
        }
    }

    private void SelecWeapon(){

        int i = 0;

        foreach(GameObject weapon in weapons){
            if(i == selectedWeapon){
                weapon.transform.SetParent(hand);
                weapon.transform.position = hand.position;
                weapon.transform.rotation = hand.transform.rotation;
                weapon.GetComponent<Weapon>().activo = true;
                weapon.GetComponent<Weapon>().enabled =true;
            }
            else{
                weapon.transform.SetParent(back);
                weapon.transform.position = back.position;
                weapon.transform.rotation = back.transform.rotation;
                weapon.GetComponent<Weapon>().activo = false;
                weapon.GetComponent<Weapon>().enabled =false;
            }
            i++;
        }
    }
}
