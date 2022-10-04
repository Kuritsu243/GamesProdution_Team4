using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roof_hiding : MonoBehaviour
{
    private Camera _camera;
    private GameObject _player;
    private GameObject _hitObject;
    private GameObject _previouslyDisabledObject;
    private GameObject _roof;
    private bool _isRoofActive = true;
    [SerializeField] private float distanceBetweenRoof;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfTouchingRoof();
        updateDistanceBetweenPlayerAndRoof();
        if (distanceBetweenRoof > 15f)
        {
            ToggleRoof(_previouslyDisabledObject);
        }
    }

    void ToggleRoof(GameObject roof)
    {
        _isRoofActive = !_isRoofActive;
        roof.SetActive(_isRoofActive);
        _previouslyDisabledObject = _hitObject;
    }

    void CheckIfTouchingRoof()
    {
        RaycastHit hit;

        if (Physics.Raycast(_camera.transform.position, (_player.transform.position - _camera.transform.position), out hit))
        {
            _hitObject = hit.collider.gameObject;
            if (hit.collider.gameObject.CompareTag("roof"))
            {
                Debug.Log("<b> Hit Object: </b> " + _hitObject);
                _roof = _hitObject;
                ToggleRoof(_roof);
            }
            else if (hit.collider.gameObject.tag == "")
                Debug.Log("no tag, ignoring error");
        }
    }

    void updateDistanceBetweenPlayerAndRoof()
    {
        distanceBetweenRoof = Vector3.Distance(_player.transform.position, _previouslyDisabledObject.transform.position);
    }
}
