using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMissionObjective : MonoBehaviour
{
    public bool activation = false;

    private float seconds = 3f;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            activation = true;
            GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(DisableArea());
        }
    }

    IEnumerator DisableArea()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(seconds);
    }
}
