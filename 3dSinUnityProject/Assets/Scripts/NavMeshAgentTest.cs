using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentTest : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent = null;

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
        WaitForSeconds attackDelay = new WaitForSeconds(Random.Range(0.05f, 0.4f));
        WaitForSeconds aimDelay = new WaitForSeconds(Random.Range(0.1f, 0.25f));
        while (true)
        {
            yield return attackDelay;
            
            RaycastHit hitInfo = new RaycastHit();
            Vector3 target = (player.transform.position - transform.position) * 20;

            yield return aimDelay;
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, target, Color.red, 0.1f);
#endif
            Physics.Raycast(transform.position, target, out hitInfo);
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.CompareTag("Player"))
                {
                    hitInfo.collider.GetComponent<FPSPlayer>().TakeDamage(10);
                }
            }
        }
    }
}
