using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Game Data/Resources")]
public class ResourceManager : ScriptableSingleton<ResourceManager>
{
    [Serializable]
    public class ObjectReferences
    {
        [Serializable]
        public class ObjectPrefabs
        {
            public Managers manager;
        }

        [SerializeField] private ObjectPrefabs prefabs = new ObjectPrefabs();
        public ObjectPrefabs Prefabs { get { return prefabs; } }
    }

    [SerializeField] private ObjectReferences references = new ObjectReferences();
    public ObjectReferences References { get { return references; } }
}
