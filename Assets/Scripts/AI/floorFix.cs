using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorFix : MonoBehaviour
{
    private void Start()
    {
        Destroy(GetComponent<MeshRenderer>()); // object is only used for navmesh baking, this ensures it isn't included when playing the game
    }
    
}
