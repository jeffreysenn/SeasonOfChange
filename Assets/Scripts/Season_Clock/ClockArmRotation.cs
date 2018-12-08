using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockArmRotation : MonoBehaviour {

    const float timeToDegrees = 360 / 80;
    float timer;
    const float offset = 90;

    void Start()
    {
    }

    void Update()
    {
        timer += Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0.0f, 0.0f, (timer * -timeToDegrees) + offset);
    }

}
