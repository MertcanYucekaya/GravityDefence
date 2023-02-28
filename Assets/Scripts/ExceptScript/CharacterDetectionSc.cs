using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDetectionSc : MonoBehaviour
{
    [SerializeField] private CharactersSc charactersSc;
    [SerializeField] private Collider col;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Killer"))
        {
            col.enabled = false;
            charactersSc.onDetection();
        }
    }
}
