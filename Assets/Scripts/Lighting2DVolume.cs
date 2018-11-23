using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
public class Lighting2DVolume : MonoBehaviour {
    private Material circleMaterial;

    private Lighting2D setting;
    private RenderTexture rt;
    private RenderTexture rtt;
    private Texture2D black;

    [HideInInspector] public List<Lighting2DLightSource> sources = new List<Lighting2DLightSource>();
    public static Lighting2DVolume instance;
    private Camera cam;
    private float ratio;

    //caching the ids for more performance
    private int _ColorID = Shader.PropertyToID("_Color");
    private int _IntensityID = Shader.PropertyToID("_Intensity");
    private int _ExpID = Shader.PropertyToID("_Exp");
    private int _CoordsID = Shader.PropertyToID("_Coords");

    private void Awake() {
        instance = this;
        cam = Camera.main;
    }

    private void OnEnable() {
        instance = this;
        sources.Clear();
        circleMaterial = new Material(Shader.Find("Hidden/PointLight2D"));
    }

    private void Start() {
        instance = this;
        cam = Camera.main;
        var volume = GetComponent<PostProcessVolume>();
        setting = volume.profile.GetSetting<Lighting2D>();

        black = new Texture2D(1, 1);
        black.SetPixel(0, 0, Color.black);
        black.Apply();
        rt = new RenderTexture(Screen.width, Screen.height, 0);
        rtt = new RenderTexture(Screen.width, Screen.height, 0);

        setting.lightingTexture.value = rt;
        ratio = Screen.width / (float)Screen.height;
    }

    private void Update() {
        Graphics.Blit(black, rt);

        foreach(var s in sources) {
            var p = cam.WorldToViewportPoint(s.transform.position);
            circleMaterial.SetColor(_ColorID, s.color);
            circleMaterial.SetFloat(_IntensityID, s.intensity);
            circleMaterial.SetFloat(_ExpID, s.exponent);
            circleMaterial.SetVector(_CoordsID, new Vector4(p.x, p.y, s.range, ratio));

            Graphics.Blit(rt, rtt, circleMaterial);
            Graphics.Blit(rtt, rt);
        }


        setting.lightingTexture.value = rt;
    }

    public void Register(Lighting2DLightSource source) {
        sources.Add(source);
    }

    public void Remove(Lighting2DLightSource source) {
        sources.Remove(source);
    }
}
