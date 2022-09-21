using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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
        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.W))
            .Subscribe(_ => {
                Vector3 direction = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
                Vector3.Normalize(direction);
                // transform.position += direction * speed * Time.deltaTime;
                body.velocity += direction * speed;
                body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);
                // body.AddForce(direction * speed, ForceMode.Acceleration);
            }).AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.S))
            .Subscribe(_ => {
                Vector3 direction = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
                Vector3.Normalize(direction);
                // transform.position -= direction * speed * Time.deltaTime;
                body.velocity -= direction * speed;
                body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);
                // body.AddForce(-direction * speed, ForceMode.Acceleration);
        });

        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.A))
            .Subscribe(_ => {
                Vector3 direction = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z);
                Vector3.Normalize(direction);
                // transform.position -= direction * speed * Time.deltaTime;
                body.velocity -= direction * speed;
                body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);
                // body.AddForce(-direction * speed, ForceMode.Acceleration);
        });

        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.D))
            .Subscribe(_ => {
                Vector3 direction = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z);
                Vector3.Normalize(direction);
                // transform.position += direction * speed * Time.deltaTime;
                body.velocity += direction * speed;
                body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);
                // body.AddForce(direction * speed, ForceMode.Acceleration);
        });

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => {
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
