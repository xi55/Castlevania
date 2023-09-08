using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{

    private GameObject cam;

    [SerializeField] private float speed;

    private float xPosition;
    private float length;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {

        float distanceMoved = cam.transform.position.x * (1 - speed);
        float distanceToMove = cam.transform.position.x * speed;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y, transform.position.z);

        if (distanceMoved > xPosition + length)
            xPosition += length;
        else if(distanceMoved < xPosition - length)
            xPosition -= length;
    }
}
