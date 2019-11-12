﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase
{

    public int HP { get { return hp; } set { hp = value; } }
    public int MP { get { return mp; } set { mp = value; } }

    
    
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        lastDir = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        LerpMove();
    }


    // アニメーション関係

    /// <summary>
    /// ワンプレイ系のアニメーションを再生する
    /// </summary>
    /// <param name="_animetionName">アニメーション遷移名</param>
    public void PlayTriggerAnimetion(string _animetionName)
    {
        gameObject.GetComponent<Animator>().SetTrigger(_animetionName);

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Attack 01"
            && collision.gameObject.tag == "Player")
        {

            Debug.Log("攻撃がヒットしたよ");

        }
    }

    void DestroyMe()
    {
        Destroy(this.gameObject);
    }
    
}