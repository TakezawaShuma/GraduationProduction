using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject moveImage_ = null;
    [SerializeField]
    private GameObject battleImage_ = null;

    void Start(){
        AllActiveOff();
    }

    /// <summary>
    /// 移動の説明ON
    /// </summary>
    public void MoveActiveOn() {
        AllActiveOff();
        moveImage_.SetActive(true);
    }

    /// <summary>
    /// バトルの説明ON
    /// </summary>
    public void BattleActiveOn() {
        AllActiveOff();
        battleImage_.SetActive(true);
    }

    /// <summary>
    /// 全てのアクティブを切る
    /// </summary>
    private void AllActiveOff() {
        moveImage_.SetActive(false);
        battleImage_.SetActive(false);
    }
}
