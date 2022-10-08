using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;


public class gameSaturationModifier : MonoBehaviour
{
    private float _enemiesInSceneCount;
    private float _enemiesCurrentlyInScene;
    private GameObject _postProcessController;
    private PostProcessVolume _postProcessVolume;
    private ColorGrading _colorGrading;

    public float EnemiesCurrentlyInScene
    {
        get => _enemiesCurrentlyInScene;
        set => _enemiesCurrentlyInScene = value;
    }
    private float _saturation;
    // Start is called before the first frame update
    
    
    void Start()
    {

        _postProcessController = GameObject.FindGameObjectWithTag("postProcessingController");
        _postProcessVolume = _postProcessController.GetComponent<PostProcessVolume>();
        _postProcessVolume.profile.TryGetSettings(out _colorGrading);
        GetAllEnemiesInScene();
        SetSaturationLevel();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CalculateSaturationLevel();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SetSaturationLevel();
        }
        Debug.Log(_saturation);
    }

    void GetAllEnemiesInScene()
    {
        _enemiesInSceneCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        _enemiesCurrentlyInScene = _enemiesInSceneCount;
    }

    public void CalculateSaturationLevel()
    {
        _saturation = (_enemiesCurrentlyInScene / _enemiesInSceneCount) * -100;
        SetSaturationLevel();
    }

    void SetSaturationLevel()
    {
        _colorGrading.saturation.value = (int)(_saturation);
        _postProcessVolume.profile.TryGetSettings(out _colorGrading);
    }


}
