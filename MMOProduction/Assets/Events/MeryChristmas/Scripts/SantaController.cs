//
// SantaController.cs
//
// Author : Tama
//

using UnityEngine;

public class SantaController : MonoBehaviour
{

    [SerializeField]
    private float _speed = 10;


    private void Start()
    {
        
    }

    private void Update()
    {
        transform.Rotate(0, Time.deltaTime * _speed, 0);
    }
}
