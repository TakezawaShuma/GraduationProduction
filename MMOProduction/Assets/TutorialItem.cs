using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tutorialの描画変更
/// </summary>
public class TutorialItem : MonoBehaviour
{
    [SerializeField]
    private GameObject image_;
    // Start is called before the first frame update
    void Start() => image_.SetActive(false);

    private void OnTriggerEnter(Collider _other) {
        if (_other.gameObject.tag == "Player") { 
            image_.SetActive(true);
            //InputManager
        }
    }
}
