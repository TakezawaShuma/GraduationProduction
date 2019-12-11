using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcWalk : MonoBehaviour
{
    enum State { 
        WALK,
        WAIT,
        RUN,

        NONE
    }
    private State state_ = State.WAIT;

    // 移動地点
    public List<Transform> point_ = new List<Transform>();
    NavMeshAgent agent_ = null;

    // 追従地点
    public Transform one_point_;

    public float range_;

    // 開始地点
    private Vector3 startPosition_;
    int point_index_ = -1;

    private Animator animeter_ = null;

    // Start is called before the first frame update
    void Start()
    {
        startPosition_ = transform.position;
        agent_ = GetComponent<NavMeshAgent>();

        animeter_ = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state_ == State.RUN) {
            PointSetting();
            if (agent_.remainingDistance < range_) {
                SetState(State.WAIT);
            }
        } else if(state_ == State.WAIT) {
            var dis = (Vector3.Distance(one_point_.position, transform.position));
            Debug.Log(dis);
            if (dis > range_) { 
                if (PointSetting()) SetState(State.RUN);
            }

        }
        Animation();
    }

    void Animation() {
        switch (state_) {
            case State.WAIT: AnimationWalk(); break;
            case State.RUN:  AnimationRun(); break;
        }
    }

    void AnimationWalk() {
        AnimationAllF();
        animeter_.SetBool("Idle", true);
    }

    void AnimationRun() {
        AnimationAllF();
        animeter_.SetBool("Run", true);
    }

    void AnimationAllF() {
        animeter_.SetBool("Idle", false);
        animeter_.SetBool("Run", false);
    }

    /// <summary>
    /// 状態変化
    /// </summary>
    /// <param name="_state"></param>
    private void SetState(State _state) => state_ = _state;

    /// <summary>
    /// 次の地点に移動する
    /// </summary>
    private void NextPointSetting() => PointSetting((point_index_ != point_.Count - 1) ? point_index_ += 1 : point_index_ = 0);

    /// <summary>
    /// 前の地点に移動する
    /// </summary>
    private void PrevPointSetting() => PointSetting((point_index_ != 0) ? point_index_ -= 1 : point_index_ = point_.Count - 1);

    /// <summary>
    /// 次の地点の設定
    /// </summary>
    /// <param name="_point_index"></param>
    private void PointSetting(int _point_index) => agent_.SetDestination(point_[_point_index].position);

    /// <summary>
    /// 追従地点の設定
    /// </summary>
    private bool PointSetting() => agent_.SetDestination(one_point_.position);
}
