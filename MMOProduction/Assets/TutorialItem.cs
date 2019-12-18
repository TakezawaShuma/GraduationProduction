using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tutorialの描画変更
/// </summary>
public class TutorialItem : MonoBehaviour
{
    [SerializeField]
    private GameObject image_ = null;

    private bool flag = true;

    void Start() => image_.SetActive(false);

    private void Update() {
        if((InputManager.InputMouseCheckDown(0) == INPUT_MODE.KEY_STOP) &&
           image_.activeSelf) {
            image_.SetActive(false);
            InputManager.Release();
        }
    }

    private void OnTriggerEnter(Collider _other) {
        if (_other.gameObject.tag == "Player" && flag) { 
            image_.SetActive(true);
            flag = false;
            InputManager.FriezeKey();
        }
    }
}
