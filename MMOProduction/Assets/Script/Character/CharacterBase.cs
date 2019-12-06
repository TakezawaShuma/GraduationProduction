using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    protected int maxHp;
    protected int maxMp;
    protected int hp = 0;
    protected int mp = 0;
    protected int status = 0;
    protected string characterName = "";
    public string Name { get { return characterName; } set { characterName = value; } }

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

