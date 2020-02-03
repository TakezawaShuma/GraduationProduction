using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField, Header("キャラクターモデルリスト")]
    private character_table characterModel = null;
    [Header("スキルの全データ"), SerializeField]
    private skill_table skillTabe = null;

    List<OtherPlayers> characters = new List<OtherPlayers>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            int count = 0;
            foreach (var tmp in characterModel.tables)
            {
                CreateOtherPlayers(new Packes.OtherPlayersData(tmp.modelId, count*2, 0, 0, tmp.modelId, "私"));

            }
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            OthersUseSkills(null);
        }
    }

    private void CreateOtherPlayers(Packes.OtherPlayersData _packet)
    {
        if (this == null) return;
        GameObject avatar = characterModel.FindModel(CheckModel(_packet.model_id));

        var otherPlayer = Instantiate<GameObject>
                          (avatar,
                          new Vector3(_packet.x, _packet.y, _packet.z),
                          Quaternion.Euler(0, 0, 0)
                          );                                  // 本体生成

        var other = otherPlayer.AddComponent<OtherPlayers>();               // OtherPlayerの追加
        other.Weapon = otherPlayer.gameObject.FindDeep("sword", true);

        otherPlayer.tag = "OtherPlayer";                                    // タグ
        //otherPlayer.transform.localScale = new Vector3(2, 2, 2);
        other.Init(_packet.x, _packet.y, _packet.z, 0, _packet.user_id, skillTabe);
        other.Type = CharacterType.Other;
        characters.Add(other);
        Debug.Log("他キャラ生成" + _packet.user_id);
    }
    private int CheckModel(int _id)
    {
        return (_id == 0) ? 101 : _id;
    }

    /// <summary>
    /// 他プレイヤーのスキルを再生 → ???
    /// </summary>
    /// <param name="_paket"></param>
    private void OthersUseSkills(Packes.OtherPlayerUseSkill _packet)
    {
        // todo
        // 他プレイヤーがスキルを使ったときの処理
        foreach (var tmp in characters)
        {
            tmp.Weapon.SetActive(true);
            tmp.animationType = PlayerAnim.PARAMETER_ID.ATTACK;
        }
    }
}
