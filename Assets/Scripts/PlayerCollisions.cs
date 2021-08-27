using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip SmallCrash;
    public AudioClip BigCrash;

    public float MinCrashCrashTime;
    public float BigCrashSpeed;

    private float _timeSinceCrash;

    // Start is called before the first frame update
    void Start()
    {
        _timeSinceCrash = MinCrashCrashTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceCrash += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_timeSinceCrash >= MinCrashCrashTime)
        {
            if (collision.relativeVelocity.magnitude > BigCrashSpeed)
            {
                AudioSource.clip = BigCrash;
            }
            else
            {
                AudioSource.clip = SmallCrash;
            }


            AudioSource.Play();
            _timeSinceCrash = 0;
        }
    }
}
