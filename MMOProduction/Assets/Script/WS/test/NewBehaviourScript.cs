using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField, Header("キャラクターモデルリスト")]
    private character_table characterModel = null;
    [Header("敵のマスターデータ"), SerializeField]
    private enemy_table enemyTable = null;
    [Header("スキルの全データ"), SerializeField]
    private skill_table skillTabe = null;

    public GameObject enemy;

    List<OtherPlayers> others = new List<OtherPlayers>();
    List<Enemy> enemys = new List<Enemy>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            int count = 0;
            foreach (var tmp in characterModel.tables)
            {
                CreateOtherPlayers(new Packes.OtherPlayersData(tmp.modelId, count * 2, 0, 0, tmp.modelId, "私"));
                count++;
            }
            count = 0;
            foreach (var tmp in enemyTable.tables)
            {
                CreateEnemys(new Packes.EnemyReceiveData(tmp.modelId, tmp.id, count * 2, 4, 0, 0, 0, 0));
                count++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            foreach(var ene in enemys)
            {
                ene.enemyAnimType = EnemyAnim.PARAMETER_ID.IDLE;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            foreach (var ene in enemys)
            {
                ene.enemyAnimType = EnemyAnim.PARAMETER_ID.WALK;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            foreach (var ene in enemys)
            {
                ene.enemyAnimType = EnemyAnim.PARAMETER_ID.ATTACK;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            foreach (var ene in enemys)
            {
                ene.enemyAnimType = EnemyAnim.PARAMETER_ID.DAMAGE;
            }
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            foreach (var ene in enemys)
            {
                ene.enemyAnimType = EnemyAnim.PARAMETER_ID.DIE;
            }
        }

        else if (Input.GetKeyDown(KeyCode.F12))
        {
            OthersUseSkills(null);
        }
    }

    void PlayAnimation(EnemyAnim.PARAMETER_ID _eneType)
    {
        enemy.GetComponent<Enemy>().enemyAnimType = _eneType;
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
        others.Add(other);
        Debug.Log("他キャラ生成" + _packet.user_id);
    }
    private int CheckModel(int _id)
    {
        return (_id == 0) ? 101 : _id;
    }


    /// <summary>
    /// エネミー作成
    /// </summary>
    /// <param name="_ene">作成に必要なデータ</param>
    private void CreateEnemys(Packes.EnemyReceiveData _ene)
    {
        GameObject enemyModel = enemyTable.FindModel(_ene.master_id);
        if (enemyModel == null) return;

        GameObject newEnemy = Instantiate<GameObject>
                              (enemyModel,
                              new Vector3(_ene.x, _ene.y, _ene.z),
                              Quaternion.Euler(0, 0, 0));

        Enemy enemy = newEnemy.AddComponent<Enemy>();
        newEnemy.transform.localScale = new Vector3(1, 1, 1);

        newEnemy.name = "Enemy:" + _ene.master_id + "->" + _ene.unique_id;

        enemy.Init(_ene.x, _ene.y, _ene.z, _ene.dir, _ene.unique_id, skillTabe);
        enemy.Type = CharacterType.Enemy;
        enemys.Add(enemy);
    }

    /// <summary>
    /// 他プレイヤーのスキルを再生 → ???
    /// </summary>
    /// <param name="_paket"></param>
    private void OthersUseSkills(Packes.OtherPlayerUseSkill _packet)
    {
        // todo
        // 他プレイヤーがスキルを使ったときの処理
        foreach (var tmp in others)
        {
            tmp.Weapon.SetActive(true);
            tmp.animationType = PlayerAnim.PARAMETER_ID.ATTACK;
        }
    }
}
