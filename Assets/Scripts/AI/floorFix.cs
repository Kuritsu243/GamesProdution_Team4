using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorFix : MonoBehaviour
{
    void Start()
    {
        Destroy(GetComponent<MeshRenderer>());
    }
    
}
