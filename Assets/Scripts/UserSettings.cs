using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Game Data/Config")]
public class UserSettings : ScriptableSingleton<UserSettings>
{
    [Serializable]
    public class AudioSettings
    {
        [Range(0, 100)] public float masterVolume = 100f;
        [Range(0, 100)] public float ambientVolume = 100f;
        [Range(0, 100)] public float sfxVolume = 100f;
        [Range(0, 100)] public float musicVolume = 100f;
    }

    [SerializeField] private AudioSettings audio = new AudioSettings();
    public AudioSettings Audio { get { return audio; } }
}
