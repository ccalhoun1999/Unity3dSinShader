using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField]
    private Rigidbody body;
    [SerializeField]
    private float drag;

    private void FixedUpdate()
    {
        var dragForce = -drag * (body.velocity * body.velocity.magnitude) / 2 * Time.fixedDeltaTime;
        dragForce.y = 0f;
        body.AddForce(dragForce, ForceMode.VelocityChange);
    }
}
