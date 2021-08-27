using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioFX : MonoBehaviour
{

    public AudioSource thrusterLoop;
    bool fadeOut;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fadeOut = false;
            thrusterLoop.volume = .7f;
            thrusterLoop.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            fadeOut = true;  
        }

        if (fadeOut)
        {
            thrusterLoop.volume -= 0.03f;

            if (thrusterLoop.volume <= 0)
            {
                thrusterLoop.Stop();
                fadeOut = false;
            }
        }
    }

    private void OnDisable()
    {
        thrusterLoop.Stop();
    }
}
