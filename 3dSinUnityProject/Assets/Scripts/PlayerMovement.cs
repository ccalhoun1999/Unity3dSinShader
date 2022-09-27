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

    public float gravity = -9.81f;

    private Vector3 moveVec = Vector3.zero;

    private readonly Vector3 JUMP_VEC = new Vector3(0f, 1f, 0f);

    private enum MovementMode
    {
        Ground,
        Grapple,
    }

    //MovementMode movementMode = MovementMode.Ground;

    private void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.W))
            .Subscribe(_ => {
                Vector3 direction = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
                direction = direction.normalized;
                // body.velocity += direction * speed;
                body.AddForce(direction * speed, ForceMode.Acceleration);
                // body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);
            }).AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.S))
            .Subscribe(_ => {
                Vector3 direction = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
                direction = direction.normalized;
                // body.velocity -= direction * speed;
                body.AddForce(-direction * speed, ForceMode.Acceleration);
                // body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);

            }).AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.A))
            .Subscribe(_ => {
                Vector3 direction = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z);
                direction = direction.normalized;
                // body.velocity -= direction * speed;
                body.AddForce(-direction * speed, ForceMode.Acceleration);
                // body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);

            }).AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(KeyCode.D))
            .Subscribe(_ => {
                Vector3 direction = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z);
                direction = direction.normalized;
                // body.velocity += direction * speed;
                body.AddForce(direction * speed, ForceMode.Acceleration);
                // body.velocity = GetNewVelVec(body.velocity, body.velocity.magnitude);

            }).AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => Jump())
            .AddTo(this);
    }

    private bool Jump()
    {
        RaycastHit hitInfo;

        Physics.Raycast(gameObject.transform.position, Vector3.down, out hitInfo, 3f);
        if (hitInfo.collider != null)
        {
            // Debug.Log("Distance from ground: " + hitInfo.distance);
            if (hitInfo.distance < 1.05f)
            {
                body.AddForce(JUMP_VEC * jumpStrength, ForceMode.Impulse);
                return true;
            }
        }
        return false;
    }

    private Vector3 GetNewVelVec(Vector3 vec, float mag)
    {
        if (mag == 0)
        {
            vec = new Vector3(0, vec.y, 0);
        }
        else 
        {
            vec = new Vector3(vec.x / mag * speed, vec.y, vec.z / mag * speed);
        }
        return vec;
    }
}
