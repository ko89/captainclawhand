using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderOrder : MonoBehaviour
{
    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null)
            return;

        renderer.sortingOrder = 10;
    }
}
