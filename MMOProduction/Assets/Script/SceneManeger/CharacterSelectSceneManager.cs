using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectSceneManager : SceneManagerBase
{
    WS.WsPlay ws = null;

    //---モデル・ラベル---//
    [SerializeField]
    private GameObject parentAttacker_=null;
    [SerializeField]
    private GameObject parentDefender_ = null;
    [SerializeField]
    private GameObject parentWitch_ = null;
    [SerializeField]
    private GameObject parentHealer_ = null;


    //---テキスト---//
    [SerializeField]
    private Text jobText_ = null;
    [SerializeField, MultilineAttribute(10)]
    string attackerText_ = null;
    [SerializeField, MultilineAttribute(10)]
    string defenderText_ = null;
    [SerializeField, MultilineAttribute(10)]
    string witchText_ = null;
    [SerializeField, MultilineAttribute(10)]
    string healerText_ = null;

    //---回転させるモデル---//
    public GameObject target_ = null;
    public GameObject Target
    {
        get { return target_; }

        set
        {
            target_ = value;
            rotating_ = false;
        }
    }

    //---回転---//
    private bool rotating_;
    private float rot_;

    //---ID---//
    public int modelID_ = 0;

    

    void Start()
    {
        rotating_ = false;
        ws = WS.WsPlay.Instance; ;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50.0f))
            {
                if(hit.collider.tag == "Player") AnimationPause(hit.collider.gameObject);
            }
        }

        if (target_ == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            rot_ = target_.transform.localEulerAngles.y - GetAngle(Input.mousePosition);
            rotating_ = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rotating_ = false;
        }

        if (!rotating_)
        {
            return;
        }

        target_.transform.localRotation = Quaternion.Euler(0f, rot_ + GetAngle(Input.mousePosition), 0f);
        
    }

    //---ボタン処理---//

    public void TypeButtonClick(int _id) {
        GameObject parent = FindModel(_id);
        
        if (!parent) {
            Debug.LogError("not model type");
            return;
        }
        ModelActiveAllOff();

        //---ラベル・モデル表示---//
        parent.SetActive(true);
        target_ = parent.transform.GetChild(0).gameObject;

        //---テキスト表示---//
        string text = FindComment(_id);
        jobText_.text = text;

        //---正面を向かせる---//
        target_.transform.rotation = Quaternion.Euler(0f, 183f, 0f);

        //---IDを割り振る---//
        modelID_ = _id;

        //---アニメーションを行う---//
        AnimationPause(parent.transform.GetChild(0).gameObject);         
    }

    //---決定ボタン処理---//
    public void clickDecision()
    {
        if(modelID_ != 0)
        {
            ws.Send(new Packes.SaveModelType(UserRecord.ID, modelID_).ToJson());
            ChangeScene("LoadingScene");
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
    
    public void AnimationPause(GameObject _obj) {
        try { _obj.GetComponent<Animator>().SetBool("pause", true); }
        catch (ArithmeticException _error) { Debug.LogError(_error); }
    }

    private GameObject FindModel(int _id) {
        if (_id == 101) return parentAttacker_;
        else if (_id == 102) return parentDefender_;
        else if (_id == 103) return parentWitch_;
        else if (_id == 104) return parentHealer_;
        return null;
    }

    private string FindComment(int _id) {
        if (_id == 101) return attackerText_;
        else if (_id == 102) return defenderText_;
        else if (_id == 103) return witchText_;
        else if (_id == 104) return healerText_;
        return null;
    }

    private void ModelActiveAllOff() {
        parentAttacker_.SetActive(false);
        parentDefender_.SetActive(false);
        parentWitch_.SetActive(false);
        parentHealer_.SetActive(false);
    }
}