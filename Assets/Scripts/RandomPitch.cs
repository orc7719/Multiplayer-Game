using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitch : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    [SerializeField] float minPitch = 1.2f;
    [SerializeField] float maxPitch = 2f;

    private void Start()
    {
        audio.pitch = Random.Range(minPitch, maxPitch);
        audio.Play();
    }
}
