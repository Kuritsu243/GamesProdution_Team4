using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingObject : MonoBehaviour
{

    [SerializeField] private Vector3 destinationPos;
    public bool haveConditionsBeenMet;
    
    [SerializeField] private float timeToTake;

    private Vector3 _startPos;
    private bool _hasMoved;

    // Start is called before the first frame update
    private void Start()
    {
        _startPos = transform.localPosition;
        if (destinationPos.x == 0)
        {
            _startPos.x = transform.localPosition.x;
        }
        else if (destinationPos.y == 0)
        {
            _startPos.y = transform.localPosition.y;
        }
        else if (destinationPos.z == 0)
        {
            _startPos.z = transform.localPosition.z;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!haveConditionsBeenMet || !(Vector3.Distance(transform.localPosition, destinationPos) > 0.01) ||
            _hasMoved) return;
        transform.Translate(destinationPos * (Time.deltaTime / timeToTake), Space.World);
        StartCoroutine(Moved());
    }

    private IEnumerator Moved()
    {
        yield return new WaitForSeconds(timeToTake);
        _hasMoved = true;
    }
}
