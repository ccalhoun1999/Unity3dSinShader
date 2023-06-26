using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody body = null;
    [SerializeField]
    private Camera cam = null;
    [SerializeField]
    private float groundSpeed = 0f;
    [SerializeField]
    private float grappleSpeed = 0f;
    [SerializeField]
    private float airSpeed = 0f;
    [SerializeField]
    private float jumpStrength = 0f;

    // public float gravity = -9.81f;

    private Vector3 moveVec = Vector3.zero;

    private readonly Vector3 JUMP_VEC = new Vector3(0f, 1f, 0f);

    public enum InputModeEnum
    {
        Keyboard,
        Controller,
    }
    public InputModeEnum InputMode = InputModeEnum.Keyboard;

    public enum MoveModeEnum
    {
        Ground,
        Grapple,
        Air,
        Reset,
    }
    public MoveModeEnum MoveMode = MoveModeEnum.Ground;

    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ => Jump())
            .AddTo(this);
    }

    private void Update()
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0f;
        Vector3 right = cam.transform.right;
        right.y = 0f;
        if (InputMode == InputModeEnum.Keyboard)
        {
            moveDirection = 
                forward.normalized * Input.GetAxisRaw("Vertical")
                + right.normalized * Input.GetAxisRaw("Horizontal");
            moveDirection = moveDirection.normalized;
        }
        else if (InputMode == InputModeEnum.Controller)
        {
            moveDirection = 
                forward.normalized * Input.GetAxis("Vertical")
                + right.normalized * Input.GetAxis("Horizontal");
            moveDirection = moveDirection.normalized;
        }
        MovementModeUpdate();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        // Debug.Log(body.velocity.magnitude);
    }

    private void MovePlayer()
    {
        if (MoveMode == MoveModeEnum.Ground)
        {
            body.velocity = new Vector3(moveDirection.x * groundSpeed, body.velocity.y, moveDirection.z * groundSpeed);
        }
        else if (MoveMode == MoveModeEnum.Grapple)
        {
            body.AddForce(moveDirection * grappleSpeed, ForceMode.Acceleration);
        }
        else if (MoveMode == MoveModeEnum.Air)
        {
            body.AddForce(moveDirection * airSpeed, ForceMode.Acceleration);
        }
    }

    public void MovementModeUpdate(bool force = false)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hitInfo, 1.05f))
        {
            MoveMode = MoveModeEnum.Ground;
        }
        else if (MoveMode != MoveModeEnum.Grapple || force == true)
        {
            MoveMode = MoveModeEnum.Air;
        }
    }

    private bool Jump()
    {
        if (MoveMode == MoveModeEnum.Ground)
        {
            // Debug.Log("Distance from ground: " + hitInfo.distance);
            body.AddForce(JUMP_VEC * jumpStrength, ForceMode.Impulse);
            return true;
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
            vec = new Vector3(vec.x / mag * groundSpeed, vec.y, vec.z / mag * groundSpeed);
        }
        return vec;
    }
}
