using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static string debugTag = "[GameManager]";

    public static GameSettings Settings { get { return GameSettings.Instance; } }
    public static ResourceManager Resources { get { return ResourceManager.Instance; } }
    public static UserSettings Config { get { return UserSettings.Instance; } }

    private static Managers _managers;
    private static Managers Managers
    {
        get
        {
            if (_managers == null)
            {
                if (_managers == null)
                {
                    _managers = Object.FindObjectOfType<Managers>();
                }

                if (_managers != null)
                {
                    Debug.LogFormat("{0} Managers found in the scene.", debugTag);
                }
                else
                {
                    _managers = Object.Instantiate(Resources.References.Prefabs.manager);
                }
            }

            return _managers;
        }
    }

    public static void Setup()
    {
        _managers = Managers;
    }
}
