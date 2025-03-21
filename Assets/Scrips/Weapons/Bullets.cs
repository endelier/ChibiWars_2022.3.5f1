using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Building")){
            //Destroy(gameObject);
        }
    }
}
