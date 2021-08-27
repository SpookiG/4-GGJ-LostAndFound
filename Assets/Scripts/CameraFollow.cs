using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
	//private Vector3 targetPos;
    public float smoothingValue;

    void Start()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
    }

    void FixedUpdate()
    {
		Vector3 targetPos = new Vector3(playerTransform.position.x, playerTransform.position.y,  -10);
		transform.position = Vector3.Lerp (transform.position, targetPos, smoothingValue * Time.deltaTime);
    }
}
