using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    //アニメーションするための変数
    Animator anime;

    // Start is called before the first frame update
    void Start()
    {
        //animatorを所得
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //マウスの位置調整
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, 50.0f))
        {
            //各キャラクターに左クリックしたら番号が出る
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.gameObject.tag == "attacker")
                    anime.SetBool("AttackerPause", true);
                    anime.SetBool("AuraAnimation", true);
                if (hit.transform.gameObject.tag == "defense")
                    anime.SetBool("DefensePause", true);
                if (hit.transform.gameObject.tag == "Healer")
                    anime.SetBool("HealerPause", true);
                if (hit.transform.gameObject.tag == "Witch")
                    anime.SetBool("WitchPause", true);
            }
        }
       

    }
}
