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
    [SerializeField]
    private float jumpStrength;

    private readonly Vector3 JUMP_VEC = new Vector3(0f, 1f, 0f);

    private void Start()
    {
        Func<KeyCode, IObservable<Unit>> onGetKey = (keyCode) => this
            .UpdateAsObservable().Where(_ => Input.GetKey(keyCode));
        Func<KeyCode, IObservable<Unit>> onGetKeyDown = (keyCode) => this
            .UpdateAsObservable().Where(_ => Input.GetKeyDown(keyCode));

        var forward = onGetKey(KeyCode.W);
        var backward = onGetKey(KeyCode.S);
        var left = onGetKey(KeyCode.A);
        var right = onGetKey(KeyCode.D);
        var jump = onGetKeyDown(KeyCode.Space);

        forward.Subscribe(_ => {
            Vector3 direction = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
            Vector3.Normalize(direction);
            // transform.position += direction * speed * Time.deltaTime;
            body.velocity += direction * speed;
            body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);
            // body.AddForce(direction * speed, ForceMode.Acceleration);
        });
        backward.Subscribe(_ => {
            Vector3 direction = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
            Vector3.Normalize(direction);
            // transform.position -= direction * speed * Time.deltaTime;
            body.velocity -= direction * speed;
            body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);
            //body.velocity = body.velocity.normalized * speed;
            // body.AddForce(-direction * speed, ForceMode.Acceleration);
        });
        left.Subscribe(_ => {
            Vector3 direction = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z);
            Vector3.Normalize(direction);
            // transform.position -= direction * speed * Time.deltaTime;
            body.velocity -= direction * speed;
            body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);
            // body.AddForce(-direction * speed, ForceMode.Acceleration);
        });
        right.Subscribe(_ => {
            Vector3 direction = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z);
            Vector3.Normalize(direction);
            // transform.position += direction * speed * Time.deltaTime;
            body.velocity += direction * speed;
            body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);
            // body.AddForce(direction * speed, ForceMode.Acceleration);
        });
        jump.Subscribe(_ => {
            if (Mathf.Approximately(body.velocity.y, 0f))
            {
                body.AddForce(JUMP_VEC * jumpStrength, ForceMode.Impulse);
            }
        });
    }

    private Vector3 GetNewVelVec(Vector3 vec, float mag)
    {
        vec = new Vector3(vec.x / mag * speed, vec.y, vec.z / mag * speed);
        return vec;
    }
}
