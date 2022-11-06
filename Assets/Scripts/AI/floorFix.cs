using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorFix : MonoBehaviour
{
    private void Start()
    {
        Destroy(GetComponent<MeshRenderer>());
    }
    
}
