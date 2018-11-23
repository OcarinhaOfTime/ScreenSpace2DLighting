using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Lighting2DLightSource : MonoBehaviour {
    public Color color = Color.white;
    [Range(0, 3)]
    public float range = .5f;
    [Range(0, 5)]
    public float intensity = 1;
    [Range(0.001f, 10)]
    public float exponent = 1;
    private bool added = false;

    private void Start() {
        Lighting2DVolume.instance.Register(this);
        added = true;
    }

    private void OnEnable() {
        print("OnEnable " + name);
        if(Lighting2DVolume.instance != null && !added)
            Lighting2DVolume.instance.Register(this);
    }

    private void OnDisable() {
        print("OnDisable " + name);
        Lighting2DVolume.instance.Remove(this);
        added = false;
    }
}
