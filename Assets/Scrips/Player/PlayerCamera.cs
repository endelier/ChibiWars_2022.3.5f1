using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivy = 80f;//velocidad de rotacion de la camara

    public Transform objectFollow;//Objeto al que  sigue
    
    [HideInInspector]public float verRotation = 0;//rotacion vertical
    [HideInInspector]public float horRotation = 0;//rotacion horizontal

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//el cursor se queda enmedio de la pantalla
    }

    // Update is called once per frame
    void Update()
    {
        Camera_move();
    }

    //se actualiza al final
    private void LateUpdate()
    {
        //sigue al centro del jugador
        transform.position = objectFollow.position;
    }

    private void Camera_move(){

        //optiene el valor de X y Y del movimiento del mouse
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivy * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivy * Time.deltaTime;

        verRotation -= mouseY;//rotacion vertical
        horRotation += mouseX;//rotacion horizontal
        
        verRotation = Mathf.Clamp(verRotation , - 90f, 90f);

        //gira el centro del jugador
        transform.localRotation = Quaternion.Euler(verRotation, horRotation, 0);

    }

}
