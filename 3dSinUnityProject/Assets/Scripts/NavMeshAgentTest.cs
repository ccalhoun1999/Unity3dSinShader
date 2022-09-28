using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentTest : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent = null;
    [SerializeField]
    private TrailRenderer bulletTrail = null;
    [SerializeField]
    private LayerMask hitScanLayerMask;

    [SerializeField]
    private float maxHitScanDistance;
    [SerializeField]
    private float idleBorder;
    [SerializeField]
    private float huntingBorder;
    [SerializeField]
    private float attackingBorder;


    GameObject player = null;

    private enum aiState
    {
        idle = 0,
        hunting = 1,
        attacking = 2,
        chasing = 3
    }

    private aiState _state;
    private aiState state
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state != value)
            {
                _state = value;
                TriggerNewState(_state);
            }
        }
    }

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        BehaviourTree();
    }

    private void BehaviourTree()
    {
        float distanceFromPlayer = 
            Vector3.Distance(transform.position, player.transform.position);

        if (distanceFromPlayer > idleBorder)
        {
            state = aiState.idle;
        }
        else if (distanceFromPlayer > huntingBorder)
        {
            state = aiState.hunting;
        }
        else if (distanceFromPlayer > attackingBorder)
        {
            state = aiState.attacking;
        }
        else
        {
            state = aiState.chasing;
        }
    }

    private void TriggerNewState(aiState state)
    {
        if (state == aiState.idle)
        {
        }
        else if (state == aiState.hunting)
        {
            StopAllCoroutines();
            StartCoroutine(GoToPlayer());
        }
        else if (state == aiState.attacking)
        {
            StopAllCoroutines();
            StartCoroutine(ShootPlayer());
        }
        else if (state == aiState.chasing)
        {
            StopAllCoroutines();
            StartCoroutine(GoToPlayer());
        }
    }

    private IEnumerator GoToPlayer()
    {
        WaitForSeconds waiter = new WaitForSeconds(0.1f);
        while (true)
        {
            yield return waiter;
                navMeshAgent.destination = player.transform.position;
        }
    }

    private IEnumerator ShootPlayer()
    {
        navMeshAgent.destination = transform.position;
        WaitForSeconds attackDelay = new WaitForSeconds(Random.Range(0.05f, 0.4f));
        WaitForSeconds aimDelay = new WaitForSeconds(Random.Range(0.1f, 0.25f));
        while (true)
        {
            yield return attackDelay;
            
            RaycastHit hitInfo = new RaycastHit();
            Vector3 target = player.transform.position - transform.position;
            target = target.normalized;

            yield return aimDelay;
            
            Debug.DrawRay(transform.position, target * maxHitScanDistance, Color.red, 0.2f);
            
            Physics.Raycast(transform.position, target, out hitInfo, maxHitScanDistance, hitScanLayerMask);

            TrailRenderer trail = Instantiate(bulletTrail, transform.position, Quaternion.identity);

            if (hitInfo.collider != null)
            {
                StartCoroutine(bulletTrailRoutine(trail, hitInfo.point, transform.position));
                if (hitInfo.collider.CompareTag("Player"))
                {
                    hitInfo.collider.GetComponent<FPSPlayer>().TakeDamage(10);
                }
            }
            else
            {
                Debug.Log("miss");
                Vector3 raycastEnd = target.normalized * maxHitScanDistance + transform.position;
                StartCoroutine(bulletTrailRoutine(trail, raycastEnd, transform.position));
            }
        }
    }
    private IEnumerator bulletTrailRoutine(TrailRenderer trail, Vector3 endPoint, Vector3 startPoint)
    {
        float distance = Vector3.Distance(startPoint, endPoint);
        trail.time = (distance / maxHitScanDistance) * 0.1f;
        float time = 0f;
        trail.transform.position = startPoint;
        while(time < 1f)
        {
            trail.transform.position = Vector3.Lerp(startPoint, endPoint, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = endPoint;
        Destroy(trail.gameObject, 0f);
    }
}
