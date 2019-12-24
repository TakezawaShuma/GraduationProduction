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

        if (anime.GetCurrentAnimatorStateInfo(0).IsName("pause") &&
            anime.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) {
            Debug.Log("end anime");
            anime.SetBool("pause", false);
        }
    }
}
