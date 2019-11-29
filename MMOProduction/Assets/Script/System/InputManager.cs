using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 入力タイプ
/// </summary>
public enum INPUT_MODE { 
    ALL_STOP,
    KEY_STOP,
    MOUSE_STOP,
    
    PLAY,
    TUTORIAL,
    CHAT,
    UI,

    NOT,

    NONE,
}

/// <summary>
/// 入力
/// </summary>
public static class InputManager
{
    // 今の状態
    private static INPUT_MODE type_ = INPUT_MODE.NONE;

    /// <summary>
    /// 更新
    /// </summary>
    public static void Update(){
        // プレイとUIの時は何もしない
        if (type_ != INPUT_MODE.PLAY && type_ != INPUT_MODE.UI && type_ != INPUT_MODE.NONE) return;

        if (IsExist()) type_ = INPUT_MODE.UI;
        else type_ = INPUT_MODE.PLAY;
    }

    static bool IsExist()
    {
        var current = EventSystem.current;
        var eventData = new PointerEventData(current){
            position = Input.mousePosition
        };
        var raycastResults = new List<RaycastResult>();
        current.RaycastAll(eventData, raycastResults);
        var isExist = 0 < raycastResults.Count;
        return isExist;
    }

    /// <summary>
    /// 押されたとき
    /// </summary>
    public static bool InputKeyCheckDown(KeyCode _code) {
        if (type_ == INPUT_MODE.KEY_STOP || type_ == INPUT_MODE.ALL_STOP || 
            (type_ == INPUT_MODE.CHAT && _code != KeyCode.Return)) 
            return false;
        
        return Input.GetKeyDown(_code);
    }    
    
    /// <summary>
    /// 押されたとき
    /// </summary>
    public static bool InputKeyCheckDown(string _code) {
        if (type_ == INPUT_MODE.KEY_STOP || type_ == INPUT_MODE.ALL_STOP || 
            type_ == INPUT_MODE.CHAT) 
            return false;
        
        return Input.GetKeyDown(_code);
    }

    /// <summary>
    /// キー
    /// </summary>
    public static bool InputKeyCheck(KeyCode _code) {
        if (type_ == INPUT_MODE.KEY_STOP || type_ == INPUT_MODE.ALL_STOP || 
            (type_ == INPUT_MODE.CHAT &&  (_code != KeyCode.LeftControl || _code != KeyCode.RightControl))) 
            return false;

        return Input.GetKey(_code);
    }

    /// <summary>
    /// はなしたとき
    /// </summary>
    public static bool InputKeyCheckUp(KeyCode _code) {
        if (type_ == INPUT_MODE.KEY_STOP || type_ == INPUT_MODE.ALL_STOP || type_ == INPUT_MODE.CHAT) 
            return false;
        return Input.GetKeyUp(_code); 
    }

    /// <summary>
    /// マウス判定
    /// </summary>
    public static INPUT_MODE InputMouseCheck(int _dir) {
        if (type_ == INPUT_MODE.MOUSE_STOP || type_ == INPUT_MODE.ALL_STOP) 
            return INPUT_MODE.NOT;

        return (Input.GetMouseButton(_dir)) ? type_ : INPUT_MODE.NOT;
    }

    /// <summary>
    /// マウス判定
    /// </summary>
    public static INPUT_MODE InputMouseCheckDown(int _type) {
        if (type_ == INPUT_MODE.MOUSE_STOP || type_ == INPUT_MODE.ALL_STOP) 
            return INPUT_MODE.NOT;
        return (Input.GetMouseButtonDown(_type))? type_ : INPUT_MODE.NOT;
    }

    /// <summary>
    /// マウス判定
    /// </summary>
    public static INPUT_MODE InputMouseCheckUp(int _type){
        if (type_ == INPUT_MODE.MOUSE_STOP || type_ == INPUT_MODE.ALL_STOP) 
            return INPUT_MODE.NOT;
        return (Input.GetMouseButtonUp(_type)) ? type_ : INPUT_MODE.NOT;
    }


    public static void FriezeKey() => type_ = INPUT_MODE.KEY_STOP;
    public static void FriezeMouse() => type_ = INPUT_MODE.MOUSE_STOP;
    public static void FriezeAll() => type_ = INPUT_MODE.ALL_STOP;
    public static void ChatMode() => type_ = INPUT_MODE.CHAT;
    public static void Release() => type_ = INPUT_MODE.NONE;
}