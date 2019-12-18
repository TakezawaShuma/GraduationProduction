using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    WS.WsPlay ws = null;

    // ---モデル---//
    public GameObject attackerModel_;
    public GameObject defenderModel_;
    public GameObject witchModel_;
    public GameObject healerModel_;

    //---ジョブ説明テキスト---//
    public GameObject attackerInfoText_;
    public GameObject defenderInfoText_;
    public GameObject witchInfoText_;
    public GameObject healerInfoText_;
    
    //---現在表示されているモデルとテキスト---//
    public GameObject nowActiveModel_ = null;
    public GameObject nowActiveText_ = null;

    public bool Enabled;

    //---回転させるモデル---//
    [SerializeField]
    private GameObject target_ = null;

    //---回転---//
    private bool rotating_;
    private float rot_;

    //---ID---//
    public int modelID_ = 0;

    //---アタッカーボタン処理---//
    public void clickAttacker()
    {
        if(nowActiveModel_ != null && nowActiveText_ != null)
        {
            nowActiveModel_.SetActive(false);
            nowActiveText_.SetActive(false);
            target_ = null;
        }

        nowActiveModel_ = attackerModel_;
        nowActiveText_ = attackerInfoText_;

        attackerModel_.SetActive(true);
        attackerInfoText_.SetActive(true);
        target_ = attackerModel_;

        modelID_ = 101;
    }

    //---ディフェンダーボタン処理---//
    public void clickDefender()
    {
        if (nowActiveModel_ != null && nowActiveText_ != null)
        {
            nowActiveModel_.SetActive(false);
            nowActiveText_.SetActive(false);
            target_ = null;
        }

        nowActiveModel_ = defenderModel_;
        nowActiveText_ = defenderInfoText_;

        defenderModel_.SetActive(true);
        defenderInfoText_.SetActive(true);
        target_ = defenderModel_;

        modelID_ =  102;
    }

    //---メイジボタン処理---//
    public void clickWitch()
    {
        if (nowActiveModel_ != null && nowActiveText_ != null)
        {
            nowActiveModel_.SetActive(false);
            nowActiveText_.SetActive(false);
            target_ = null;
        }

        nowActiveModel_ = witchModel_;
        nowActiveText_ = witchInfoText_;

        witchModel_.SetActive(true);
        witchInfoText_.SetActive(true);
        target_ = witchModel_;

        modelID_ = 103;
    }

    //---ヒーラーボタン処理---//
    public void clickHealer()
    {
        if (nowActiveModel_ != null && nowActiveText_ != null)
        {
            nowActiveModel_.SetActive(false);
            nowActiveText_.SetActive(false);
            target_ = null;
        }

        nowActiveModel_ = healerModel_;
        nowActiveText_ = healerInfoText_;

        healerModel_.SetActive(true);
        healerInfoText_.SetActive(true);
        target_ = healerModel_;

        modelID_ = 104;
    }

    //---決定ボタン処理---//
    public void clickDecision()
    {
        if(modelID_ != 0)
        {
            ws.Send(new Packes.SaveModelType(UserRecord.ID, modelID_).ToJson());
            SceneManager.LoadScene("LoadingScene");
            //Debug.Log(new Packes.SaveModelType(UserRecord.ID, modelID_).ToJson());
        }
    }

    public GameObject Target
    {
        get { return target_; }

        set
        {
            target_ = value;
            rotating_ = false; 
        }
    }

    private float GetAngle(Vector3 pos)
    {
        var camera = GameObject.FindObjectOfType<Camera>();
        var origin = camera.WorldToScreenPoint(target_.transform.position);

        Vector3 diff = pos - origin;

        var angle = diff.magnitude < Vector3.kEpsilon
                                    ? 0.0f
                                    : Mathf.Atan2(diff.y, diff.x);

        return angle * Mathf.Rad2Deg;
    }

    void Start()
    {
        rotating_ = false;
        Enabled = true;

        ws = WS.WsPlay.Instance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50.0f)){
                try { hit.collider.gameObject.GetComponent<Animator>().SetBool("pause", true); }
                catch (ArithmeticException _error) { Debug.LogError(_error); }
            }
        }

        if(Enabled == false || target_ == null)
        {
            return;
        }

        if(Input.GetMouseButtonDown(1))
        {
            rot_ = target_.transform.eulerAngles.y - GetAngle(Input.mousePosition);
            rotating_ = true;
        }
        else if(Input.GetMouseButtonUp(1))
        {
            rotating_ = false;
        }
        
        if(!rotating_)
        {
            return;
        }

        target_.transform.rotation = Quaternion.Euler(0f, rot_ + GetAngle(Input.mousePosition), 0f);
    }
         
}
