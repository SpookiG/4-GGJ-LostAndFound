using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPick : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip[] PickFrom;


    // Start is called before the first frame update
    void Start()
    {
        AudioSource.clip = PickFrom[UnityEngine.Random.Range(0, PickFrom.Length)];
    }
}
