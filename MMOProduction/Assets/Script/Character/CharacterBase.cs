using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public int maxHp { get; set; }
    public int maxMp { get; set; }
    public int hp { get; set; }
    public int mp { get; set; }
    public int status { get; set; }

    protected string characterName = "";
    public string Name { get { return characterName; } set { characterName = value; } }
    protected CharacterType characterType;     // キャラクタータイプ
    public CharacterType Type { get { return characterType; } set { characterType = value; } }

    protected int id = 0;   // キャラクターごとのID

    protected Animator animator_;


    /// <summary>
    /// キャラクターごとのID
    /// </summary>
    public int ID
    {
        get { return this.id; }
        set { this.id = value; }
    }
}

public enum CharacterType
{
    Player,
    Other,
    NPC,
    Enemy
}
