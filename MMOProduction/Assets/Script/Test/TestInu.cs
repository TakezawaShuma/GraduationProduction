using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestInu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] point = null;

    [SerializeField]
    private NavMeshAgent nma = null;

    private int nowPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        nma.SetDestination(point[nowPoint].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if((point[nowPoint].transform.position - transform.position).magnitude < 2f)
        {
            nowPoint++;

            if(nowPoint >= point.Length)
            {
                nowPoint = 0;
            }

            nma.SetDestination(point[nowPoint].transform.position);
        }
    }
}
