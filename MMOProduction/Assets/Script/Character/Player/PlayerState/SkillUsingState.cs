//
// SkillUsingState.cs
//
// Author : Tama
//
// プレイヤーのスキル使用状態を処理
//

public class SkillUsingState : BaseState
{


    public override void Start()
    {
        
    }

    public override void Execute()
    {
        
    }

    public override void End()
    {
        
    }




    // シングルトン化 --------------------------------------------
    private static BaseState _instance;
    public static BaseState Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SkillUsingState();
            return _instance;
        }
    }
}
