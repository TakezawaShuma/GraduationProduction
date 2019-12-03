using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, 50.0f))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.gameObject.tag == "attacker")
                    Debug.Log("1");
                if (hit.transform.gameObject.tag == "defense")
                    Debug.Log("2");
                if (hit.transform.gameObject.tag == "Healer")
                    Debug.Log("3");
                if (hit.transform.gameObject.tag == "Witch")
                    Debug.Log("4");
            }
        }

    }
}
