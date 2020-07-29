using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public bool touchStart = false;
    private Vector3 pointA;
    private Vector3 pointB;

    public Transform circle;
    public Transform outerCircle;

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<PlayerControls>().nextLevel)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    pointA = hit.point;

                    circle.transform.position = new Vector3(pointA.x, 0, pointA.z);
                    outerCircle.transform.position = new Vector3(pointA.x, 0, pointA.z);
                    circle.GetComponent<SpriteRenderer>().enabled = true;
                    outerCircle.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
            if (Input.GetMouseButton(0))
            {
                touchStart = true;
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    pointB = hit.point;
                }
            }
            else
            {
                touchStart = false;
            }
        }
        else
        {
            touchStart = false;
        }

    }
    private void FixedUpdate()
    {
        if (touchStart)
        {
            Vector3 offset = pointB - pointA;
            Vector3 direction = Vector3.ClampMagnitude(offset, 1.0f);
            if (!GetComponent<PlayerControls>().levelStart)
            GetComponent<PlayerControls>().moveTo = new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z + direction.z);

            circle.transform.position = new Vector3(pointA.x + direction.x, 0, pointA.z + direction.z);
        }
        else
        {
            circle.GetComponent<SpriteRenderer>().enabled = false;
            outerCircle.GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}
