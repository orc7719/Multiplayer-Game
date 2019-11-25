using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    public static PlayerCanvas canvas;

    private void Start()
    {
        if (canvas == null)
            canvas = this;
    }
}
