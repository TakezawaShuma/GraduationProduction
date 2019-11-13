using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag=="Weapon" && other.gameObject.tag=="Enemy")
        {
            WS.WsPlay.Instance.Send(new Packes.Attack(other.gameObject.GetComponent<Enemy>().ID, UserRecord.ID, 0, 0).ToJson());
        }
    }
}
