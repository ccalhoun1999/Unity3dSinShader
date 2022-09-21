using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float mouseSensitivity = 0.1f;
    [SerializeField]
    private float clampAngle = 80.0f;

    private float rotY = 0.0f;
    private float rotX = 0.0f;

    private void Start()
    {
        Application.targetFrameRate = -1;
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;

#if UNITY_EDITOR
        KeyCode pauseKey = KeyCode.P;
#else
        KeyCode pauseKey = KeyCode.Escape;
#endif

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(pauseKey))
            .Subscribe(_ =>
            {
                if (Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Time.timeScale = 0f;
                    Cursor.lockState = CursorLockMode.None;
                }
            })
            .AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Mouse0))
            .Subscribe(_ => HitScan())
            .AddTo(this);
    }

    private void HitScan()
    {
        RaycastHit hitInfo;

#if UNITY_EDITOR
        Vector3 debugRayOrigin = gameObject.transform.position;
        debugRayOrigin.y -= 0.5f;
        Debug.DrawRay(debugRayOrigin, gameObject.transform.forward * 20, Color.white, 0.5f);
#endif

        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward * 20, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                Destroy(hitInfo.transform.gameObject);
            }
        }
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.timeScale;
        rotX += mouseY * mouseSensitivity * Time.timeScale;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }
}