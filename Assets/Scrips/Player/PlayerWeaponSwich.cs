using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSwich : MonoBehaviour
{

    public GameObject[] weapons;

    private int selectedWeapon=0;

    //hueso mano
    public Transform hand;

    public Transform back;



    void Start()
    { 
        
        weapons[0].transform.SetParent(hand);
        weapons[0].transform.position = hand.position;
        weapons[0].transform.rotation = hand.transform.rotation;
        weapons[0].GetComponent<Weapon>().enabled =true;

        weapons[1].transform.SetParent(back);
        weapons[1].transform.position = back.position;
        weapons[1].transform.rotation = back.transform.rotation;
        weapons[1].GetComponent<Weapon>().enabled =false;
        
    }

    void Update()
    {
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

    private void SelecWeapon(){

        int i = 0;

        foreach(GameObject weapon in weapons){
            if(i == selectedWeapon){
                weapon.transform.SetParent(hand);
                weapon.transform.position = hand.position;
                weapon.transform.rotation = hand.transform.rotation;
                weapon.GetComponent<Weapon>().enabled =true;
            }
            else{
                weapon.transform.SetParent(back);
                weapon.transform.position = back.position;
                weapon.transform.rotation = back.transform.rotation;
                weapon.GetComponent<Weapon>().enabled =false;
            }
            i++;
        }
    }
}
