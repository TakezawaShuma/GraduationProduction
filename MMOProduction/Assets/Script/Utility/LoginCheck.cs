using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class LoginCheck
{
    public const int MIN_ID_WORD = 4;
    public const int MIN_PW_WORD = 4;
    public enum CHECKRESULT
    {
        OK,
        ERROR,

        MINSTRING,
        INVALID,

        NONE,
    }

    public CHECKRESULT CheckIdAndPassword(string _id, string _pass)
    {
        CHECKRESULT result = CHECKRESULT.OK;
        if ((result = CheackWord(_id, MIN_ID_WORD)) != CHECKRESULT.OK) {
            return result;
        }
        if ((result = CheackWord(_pass, MIN_PW_WORD)) != CHECKRESULT.OK) {
            return result;
        }
        return CHECKRESULT.OK;        
    }

    private CHECKRESULT CheackWord(string _str, int _min)
    {
        //文字が入っていない場合
        if (_str == "") return CHECKRESULT.MINSTRING;

        // 4文字以下なら弾く
        if (_str.Length < _min) return CHECKRESULT.MINSTRING;
        //英数字と_のみか判定
        if (!Regex.IsMatch(_str, @"^[0-9a-zA-Z-_]+$")) return CHECKRESULT.INVALID;

        return CHECKRESULT.OK;

    }
}