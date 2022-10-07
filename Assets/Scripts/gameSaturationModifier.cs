using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;


public class gameSaturationModifier : MonoBehaviour
{

    private GameObject _postProcessController;
    private PostProcessVolume _postProcessVolume;
    private ColorGrading _colorGrading;
    private int _levelProgression;

    public int LevelProgression
    {
        get => _levelProgression;
        set => _levelProgression = value;
    }
    [Range(-100, 100f)]
    [SerializeField] private int saturation;
    // Start is called before the first frame update
    void Start()
    {
        _postProcessController = GameObject.FindGameObjectWithTag("postProcessingController");
        _postProcessVolume = _postProcessController.GetComponent<PostProcessVolume>();
        _postProcessVolume.profile.TryGetSettings(out _colorGrading);
        
    }
    // Update is called once per frame
    void Update()
    {
        _colorGrading.saturation.value = saturation;
    }
}
