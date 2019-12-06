using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "game_table/character_model", fileName = "model_data")]
public class ModelTable : ScriptableObject
{
    // データ一覧
    [SerializeField]
    public List<CharacterModel> tables_ = new List<CharacterModel>();

    [System.SerializableAttribute]
    public class CharacterModel
    {
        //モデルID
        public int id_;
        // モデル
        public GameObject modelPre_;
    }
}
