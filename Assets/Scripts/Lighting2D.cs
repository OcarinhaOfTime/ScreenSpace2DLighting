using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(Lighting2DRenderer), PostProcessEvent.AfterStack, "Custom/Lighting2D")]
public sealed class Lighting2D : PostProcessEffectSettings {
    [Range(0f, 1f)]
    public FloatParameter brightness = new FloatParameter { value = 0.5f };
    public TextureParameter lightingTexture = new TextureParameter();
}

public sealed class Lighting2DRenderer : PostProcessEffectRenderer<Lighting2D> {
    public override void Render(PostProcessRenderContext context) {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Lighting2D"));
        sheet.properties.SetFloat("_Brightness", settings.brightness);
        if(settings.lightingTexture.value == null) {
            var t = new Texture2D(1, 1);
            t.SetPixel(0, 0, Color.black);
            t.Apply();
            settings.lightingTexture.value = t;
        }

        sheet.properties.SetTexture("_LightTex", settings.lightingTexture);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}