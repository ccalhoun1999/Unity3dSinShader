using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody body;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float speed;

    private void Start()
    {
        Func<KeyCode, IObservable<Unit>> onGetKey = (keyCode) => this
            .UpdateAsObservable().Where(_ => Input.GetKey(keyCode));

        var forward = onGetKey(KeyCode.W);
        var backward = onGetKey(KeyCode.S);
        var left = onGetKey(KeyCode.A);
        var right = onGetKey(KeyCode.D);

        forward.Subscribe(_ =>
            {
                Vector3 direction = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
                Vector3.Normalize(direction);
                transform.position += direction * speed * Time.deltaTime;
                // body.velocity -= direction;
                // body.velocity = body.velocity.normalized * speed;
                // body.AddForce(direction * speed, ForceMode.Acceleration);
            });
        backward.Subscribe(_ =>
            {
                Vector3 direction = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
                Vector3.Normalize(direction);
                transform.position -= direction * speed * Time.deltaTime;
                // body.velocity -= direction;
                // body.velocity = body.velocity.normalized * speed;
                // body.AddForce(-direction * speed, ForceMode.Acceleration);
            });
        left.Subscribe(_ =>
            {
                Vector3 direction = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z);
                Vector3.Normalize(direction);
                transform.position -= direction * speed * Time.deltaTime;
                // body.velocity -= direction;
                // body.velocity = body.velocity.normalized * speed;
                // body.AddForce(-direction * speed, ForceMode.Acceleration);
            });
        right.Subscribe(_ =>
            {
                Vector3 direction = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z);
                Vector3.Normalize(direction);
                transform.position += direction * speed * Time.deltaTime;
                // body.velocity += direction;
                // body.velocity = body.velocity.normalized * speed;
                // body.AddForce(direction * speed, ForceMode.Acceleration);
            });
    }
}
