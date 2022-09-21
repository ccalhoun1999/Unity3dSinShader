using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentTest : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent = null;

    GameObject player = null;

    void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine(GoToPlayer());
        StartCoroutine(AttackPlayer());
    }

    private IEnumerator GoToPlayer()
    {
        WaitForSeconds waiter = new WaitForSeconds(0.1f);
        while (true)
        {
            yield return waiter;
            if (Vector3.Distance(transform.position, player.transform.position) > 5f)
            {
                navMeshAgent.destination = player.transform.position;
            }
            else
            {
                navMeshAgent.destination = transform.position;
            }
        }
    }

    private IEnumerator AttackPlayer()
    {
        WaitForSeconds wait1 = new WaitForSeconds(Random.Range(0.05f, 0.4f));
        WaitForSeconds wait2 = new WaitForSeconds(Random.Range(0.15f, 0.6f));
        while (true)
        {
            yield return wait1;
            
            RaycastHit hitInfo = new RaycastHit();
            Vector3 target = (player.transform.position - transform.position) * 20;

            yield return wait2;
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, target, Color.red, 0.1f);
#endif
            Physics.Raycast(transform.position, target, out hitInfo);
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.CompareTag("Player"))
                {
                    Debug.Log("Hit player");
                }
            }
        }
    }
}
