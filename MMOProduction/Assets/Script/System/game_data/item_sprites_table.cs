using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "game_table/item_sprite", fileName = "item_sprite_data")]
public class item_sprites_table : ScriptableObject
{

    [SerializeField]
    List<item_sprite_data> table;

    [System.SerializableAttribute]
    public class item_sprite_data
    {
        //アイテムの種別ID
        public int id;
        // 画像ID
        public int spriteID;
        //画像
        public Sprite sprite;
    }

    public Sprite FindOne(int _id)
    {
        Sprite ret = null;
        foreach (item_sprite_data data in table)
        {
            if (data.id == _id)
            {
                ret = data.sprite;
                break;
            }
        }
        return ret;
    }
}