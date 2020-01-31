using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelect : MonoBehaviour
{
    //アニメーションするための変数
    Animator anime;

    // 自身が選択されたときに呼ばれる
    [SerializeField]
    private UnityEvent _onSelect = new UnityEvent();
    

    // Start is called before the first frame update
    void Start()
    {
        //animatorを所得
        anime = GetComponent<Animator>();
   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50.0f))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    //AnimationPause(hit.collider.gameObject);
                    //_selectLookCamera.EntryTarget(hit.collider.transform);
                    _onSelect.Invoke();
                }
            }
        }

        if (anime.GetCurrentAnimatorStateInfo(0).IsName("pause") &&
            anime.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) {
            anime.SetBool("pause", false);
        }
    }
}
