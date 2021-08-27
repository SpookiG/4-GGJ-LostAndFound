using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// grab each child object
// for each child, if out of viewzone (x or y) then move to other side of viewzone (x or y)


public class BackgroundFollow : MonoBehaviour
{
    public Vector2 ViewZoneSize;
    public Transform CamTransform;

    private Vector2 _lastCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        _lastCameraPosition = CamTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform)
        {
            // parallax movement based on size
            // add 
            //Debug.Log(_lastCameraPosition == (Vector2)CamTransform.position);
            //Debug.Log(_lastCameraPosition - (Vector2)CamTransform.position);

            float scaleFactor = Mathf.Log((child.localScale.x + child.localScale.y) / 2, 3);
            float distanceCheck = scaleFactor < 0 ? 1f : 10f;


            child.position = (Vector2) child.position + ((_lastCameraPosition - (Vector2)CamTransform.position) * distanceCheck * scaleFactor);


            // reposition if out of frame
            if (child.position.x < CamTransform.position.x - ViewZoneSize.x)
            {
                child.position = new Vector2(child.position.x + (ViewZoneSize.x * 2), child.position.y);
            }
            if (child.position.x > CamTransform.position.x + ViewZoneSize.x)
            {
                child.position = new Vector2(child.position.x - (ViewZoneSize.x * 2), child.position.y);
            }

            if (child.position.y < CamTransform.position.y - ViewZoneSize.y)
            {
                child.position = new Vector2(child.position.x, child.position.y + (ViewZoneSize.y * 2));
            }
            if (child.position.y > CamTransform.position.y + ViewZoneSize.y)
            {
                child.position = new Vector2(child.position.x, child.position.y - (ViewZoneSize.y * 2));
            }
        }

        _lastCameraPosition = CamTransform.position;
    }
}
