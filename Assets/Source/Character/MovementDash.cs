using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDash : MonoBehaviour
{
    public float dashVelocity = 50;
    public const float maxDashTime = 1.0f;
    public float dashStoppingSpeed = 0.1f;
    float currentDashTime = 0;
    public void Dash()
    {
        if (Input.GetKey(KeyCode.Y) == true)
        {
            while (currentDashTime <= dashStoppingSpeed)
            {
                GetComponent<Rigidbody>().velocity = transform.forward * dashVelocity;
                currentDashTime = dashStoppingSpeed * Time.deltaTime;
            }
        }
    }
}
