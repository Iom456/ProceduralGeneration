using UnityEngine;

public class TestSimTexture : MonoBehaviour
{
    [SerializeField] private int _size = 512;
    [SerializeField] private float _waterLevel = 2f;
    [SerializeField] private Material[] _materials;   // terrain + eau

    private void Start()
    {
        var tex = new Texture2D(_size, _size, TextureFormat.RGBAFloat, false, true)
            { filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };

        var pixels = new Color[_size * _size];
        for (int y = 0; y < _size; y++)
        for (int x = 0; x < _size; x++)
        {
            float b = 3f + 3f * Mathf.Sin(x * 0.03f) * Mathf.Cos(y * 0.03f); // sol
            float d = Mathf.Max(0f, _waterLevel - b);                          // eau
            pixels[y * _size + x] = new Color(b, d, 0f, 1f);                   // R=b, G=d, B=s
        }
        tex.SetPixels(pixels);
        tex.Apply(false);

        foreach (var m in _materials) m.SetTexture("_SimTex", tex);
    }
}