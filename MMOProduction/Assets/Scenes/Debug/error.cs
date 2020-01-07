using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class error : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExternalFileAccess.Overwrite("../ip.txt", "172.24.52.250");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            ExternalFileAccess.Additional("../test.txt", "明日は曇天だ。\n");
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            string file= ExternalFileAccess.ReadFile("../test.txt");
            Debug.Log(file);
        }
        
    }
}
