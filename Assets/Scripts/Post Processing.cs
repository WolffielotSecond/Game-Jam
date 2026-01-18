using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessing : MonoBehaviour
{
    public Volume _globalVolume;
    private DepthOfField _depthOfField;
    public float focusDistance;
    public GameObject _player;
    private void Awake()
    {
        var profile = _globalVolume.profile != null ? _globalVolume.profile : _globalVolume.sharedProfile;

        if (!profile.TryGet(out _depthOfField))
        {
            Debug.LogError("Profile 里没有 Depth Of Field。请在 Volume Profile 里 Add Override -> Depth Of Field");
            enabled = false;
            return;
        }

        _depthOfField.focusDistance.overrideState = true;
    }
    private void Update()
    {


        if (_player.GetComponent<Player>()._cooldown <= 15)
        {
            _depthOfField.active = true;
            _depthOfField.focusDistance.value = _player.GetComponent<Player>()._cooldown / 5;
            
        }
        else
        {
            _depthOfField.active = false;
            _depthOfField.focusDistance.value = 3;
        }
    }

}
