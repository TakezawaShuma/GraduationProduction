using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;

public class TitleEnemy : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> point = new List<GameObject>();

    [SerializeField]
    private NavMeshAgent nav = null;

    [SerializeField]
    private float waitTime = 0f;

    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private float distance = 0f;

    private float currentTime = 0f;

    private int currentPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        point = point.OrderBy(a => Guid.NewGuid()).ToList();
        nav.SetDestination(point[currentPoint].transform.position);
        Walk();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 v = new Vector2(point[currentPoint].transform.position.x, point[currentPoint].transform.position.z) - new Vector2(transform.position.x, transform.position.z);

        if(v.magnitude <= distance)
        {
            currentTime += Time.deltaTime;
            nav.isStopped = true;
            Wait();

            if (currentTime >= waitTime)
            {
                nav.isStopped = false;

                currentPoint++;

                if (currentPoint >= point.Count)
                {
                    currentPoint = 0;
                }

                nav.SetDestination(point[currentPoint].transform.position);

                Walk();

                currentTime = 0f;
            }
        }
    }

    private void Wait()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);
    }

    private void Walk()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", true);
    }
}
