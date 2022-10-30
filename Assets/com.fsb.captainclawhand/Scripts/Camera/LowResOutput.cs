using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowResOutput : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private RawImage _rawImage;
    [SerializeField]
    private int _outputWidth = 320;

    private RenderTexture _rt;

    void Start()
    {
        float aspect = (float)Screen.width / (float)Screen.height;
        var width = Mathf.Max(_outputWidth, 32);
        var height = (int)(width / aspect);

        _rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);

        if (_camera != null)
            _camera.targetTexture = _rt;

        if (_rawImage != null)
        {
            _rawImage.texture = _rt;

            var canvas = _rawImage.GetComponentInParent<Canvas>(true);
            if (canvas != null)
                canvas.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (_camera != null)
            _camera.targetTexture = null;

        if (_rawImage != null)
            _rawImage.texture = null;

        RenderTexture.Destroy(_rt);
    }
}
