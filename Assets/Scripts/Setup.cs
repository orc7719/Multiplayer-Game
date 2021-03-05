using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : Singleton<Setup>
{
    protected override void Start()
    {
        GameManager.Setup();
    }
}
