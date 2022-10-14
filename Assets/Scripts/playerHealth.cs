using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    // Start is called before the first frame update


    void damage(float damageamount) // damage function
    {
        _playerhealth -= damageamount; // reduces health by damage value passed through 
    }

    void die()
    { // destroys player gameobject
        destroy(this.gameobject);
    }
}
