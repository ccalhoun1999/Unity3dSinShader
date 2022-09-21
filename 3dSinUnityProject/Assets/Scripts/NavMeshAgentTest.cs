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
            navMeshAgent.destination = player.transform.position;
        }
    }

    private IEnumerator AttackPlayer()
    {
        WaitForSeconds waiter = new WaitForSeconds(0.3f);
        while (true)
        {
            yield return waiter;
            
            RaycastHit hitInfo = new RaycastHit();
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, (player.transform.position - transform.position) * 20, Color.red, 0.1f);
#endif
            Physics.Raycast(transform.position, (player.transform.position - transform.position) * 20, out hitInfo);
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
