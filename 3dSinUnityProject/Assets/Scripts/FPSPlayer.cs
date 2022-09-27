using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FPSPlayer : MonoBehaviour
{
    [SerializeField]
    private Rigidbody body = null;
    [SerializeField]
    private MouseLook mouseLook = null;
    [SerializeField]
    private float _hp;
    public float HealthPoints
    {
        get
        {
            return _hp;
        }
        private set
        {
            _hp = value;
        }
    }

    private bool grappling = false;

    private void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Mouse0))
            .Subscribe(_ => HitScan())
            .AddTo(this);
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Mouse1))
            .Subscribe(_ => Grapple())
            .AddTo(this);
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyUp(KeyCode.Mouse1))
            .Subscribe(_ => grappling = false)
            .AddTo(this);
    }

    public void TakeDamage(float damage)
    {
        HealthPoints -= damage;
        if (HealthPoints < 0)
        {
            HealthPoints = 0;
        }
        Debug.Log(HealthPoints);
    }

    private void HitScan()
    {
        RaycastHit hitInfo;

        Vector3 hitScanOrigin = mouseLook.transform.position;
        hitScanOrigin.y -= 0.05f;
        hitScanOrigin.x += 0.05f;
        
        Physics.Raycast(hitScanOrigin, mouseLook.transform.forward * 20, out hitInfo);
        if(hitInfo.collider != null)
        {
            Debug.DrawLine(hitScanOrigin, hitInfo.point, Color.white, 0.5f);
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                Destroy(hitInfo.transform.gameObject);
            }
        }
        else
        {
            Debug.DrawRay(hitScanOrigin, mouseLook.transform.forward * 50f, Color.white, 0.5f);            
        }
    }

    private void Grapple()
    {
        RaycastHit hitInfo;

        Vector3 hitScanOrigin = mouseLook.transform.position;
        hitScanOrigin.y -= 0.05f;
        hitScanOrigin.x += 0.05f;
        
        Physics.Raycast(hitScanOrigin, mouseLook.transform.forward * 20, out hitInfo);

        if(hitInfo.collider != null)
        {
            grappling = true;
            StartCoroutine(GrappleRoutine(hitInfo));
        }
    }

    private IEnumerator GrappleRoutine(RaycastHit hitInfo)
    {
        WaitForFixedUpdate UpdateRate = new WaitForFixedUpdate();
        float startTime = Time.time;
        float duration = 3f;
        while(Time.time < startTime + duration && grappling == true)
        {
            Debug.DrawLine(gameObject.transform.position, hitInfo.point, Color.white);
            Vector3 force = Vector3.Normalize(hitInfo.point - transform.position) * 10f;
            Vector3 scaledForce = new Vector3(force.x * 3f, force.y * 3f, force.z * 3f);
            body.AddForce(scaledForce);
            Debug.Log(force);
            yield return UpdateRate;
        }
    }
}
