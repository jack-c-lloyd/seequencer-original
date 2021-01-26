using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class MaterialRelay : MonoBehaviour
{
    public Renderer target = null;

    public string[] colorNames;
    public string[] colorArrayNames;
    public string[] floatNames;
    public string[] floatArrayNames;
    public string[] intNames;
    public string[] matrixNames;
    public string[] matrixArrayNames;
    public string[] shaderPassEnabledNames;
    public string[] textureNames;
    public string[] textureOffsetNames;
    public string[] textureScaleNames;
    public string[] vectorNames;
    public string[] vectorArrayNames;

    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        var src = target.material;
        var dst = renderer.material;

        foreach (var name in colorNames)
        {
            dst.SetColor(name, src.GetColor(name));
        }

        foreach (var name in colorArrayNames)
        {
            dst.SetColorArray(name, src.GetColorArray(name));
        }

        foreach (var name in floatNames)
        {
            dst.SetFloat(name, src.GetFloat(name));
        }

        foreach (var name in floatArrayNames)
        {
            dst.SetFloatArray(name, src.GetFloatArray(name));
        }

        foreach (var name in intNames)
        {
            dst.SetInt(name, src.GetInt(name));
        }

        foreach (var name in matrixNames)
        {
            dst.SetMatrix(name, src.GetMatrix(name));
        }

        foreach (var name in matrixArrayNames)
        {
            dst.SetMatrixArray(name, src.GetMatrixArray(name));
        }

        foreach (var name in shaderPassEnabledNames)
        {
            dst.SetShaderPassEnabled(name, src.GetShaderPassEnabled(name));
        }

        foreach (var name in textureNames)
        {
            dst.SetTexture(name, src.GetTexture(name));
        }

        foreach (var name in textureOffsetNames)
        {
            dst.SetTextureOffset(name, src.GetTextureOffset(name));
        }

        foreach (var name in textureScaleNames)
        {
            dst.SetTextureScale(name, src.GetTextureScale(name));
        }

        foreach (var name in vectorNames)
        {
            dst.SetVector(name, src.GetVector(name));
        }

        foreach (var name in vectorArrayNames)
        {
            dst.SetVectorArray(name, src.GetVectorArray(name));
        }
    }
}