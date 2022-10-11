using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class renderWithinRadius : MonoBehaviour
{
    [SerializeField] private int postProcessingLayer;
    [SerializeField] private int noPostProcessingLayer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Floor"))
        {
            other.gameObject.layer = noPostProcessingLayer;
        }

        Debug.Log("changed obj " + other.gameObject + "to no post process");
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.layer = postProcessingLayer;
    }
}
