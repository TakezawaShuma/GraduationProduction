using System;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50.0f)){
                try { hit.collider.gameObject.GetComponent<Animator>().SetBool("pause", true); }
                catch (ArithmeticException _error) { Debug.LogError(_error); }
            }
        }
    }
}
