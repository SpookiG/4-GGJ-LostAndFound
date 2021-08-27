using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    public float thrustPower;
    public float rotationSpeed;

    public float maxSpeed;

    private Vector2 _storedVelocity;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        _storedVelocity = Vector2.zero;
    }

    void FixedUpdate()
    {
        //Inputs: A/D or cursors = rotate left/right. Thrust forward = space
        if (Input.GetKey(KeyCode.Space))
        {
            body.AddForce(transform.up * thrustPower, ForceMode2D.Force);    
        }

        float rotationInput = Input.GetAxisRaw("Horizontal");
        body.angularVelocity = -rotationInput * rotationSpeed;    


        // Limit maximum speed
        if (body.velocity.x > maxSpeed)
        {
            body.velocity = new Vector2(maxSpeed, body.velocity.y);
        }
        else if (body.velocity.x < -maxSpeed)
        {     
            body.velocity = new Vector2(-maxSpeed, body.velocity.y);
        }

        if (body.velocity.y > maxSpeed)
        {
            body.velocity = new Vector2(body.velocity.x, maxSpeed);
        }
        else if (body.velocity.y < -maxSpeed)
        {     
            body.velocity = new Vector2(body.velocity.x, -maxSpeed);
        }
    }


    private void OnEnable()
    {
        if (body != null)
        {
            body.velocity = _storedVelocity;
        }
    }

    private void OnDisable()
    {
        _storedVelocity = body.velocity;
        body.velocity = Vector2.zero;
    }
}
