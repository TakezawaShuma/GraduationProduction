//////////////////////////////////////////////
// プレイヤーに追従するカメラを制御用クラス //
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, DisallowMultipleComponent]
public class FollowingCamera : MonoBehaviour
{
    [SerializeField, Header("写したいターゲット")]
    private GameObject target; // an object to follow

    /// <summary> ターゲットの設定 </summary>
    public GameObject Target { set { target = value; } }

    [SerializeField, Header("オフセット")]
    private Vector3 offset = new Vector3(0f, 1.5f, 0f); // offset form the target object

    [SerializeField, Header("ターゲットとの距離")]
    private float distance = 2.0f; // distance from following object
    [SerializeField, Header("X軸の回転")]
    private float polarAngle = 75.0f; // angle with y-axis
    [SerializeField, Header("Y軸の回転")]
    private float azimuthalAngle = 270.0f; // angle with x-axis

    public Quaternion Angle
    {
        get { return Quaternion.Euler(0, -azimuthalAngle, 0); }
    }

    [Space()]

    [SerializeField]
    private float minDistance = 0.7f;
    [SerializeField]
    private float maxDistance = 10.0f;
    [SerializeField]
    private float minPolarAngle = 45.0f;
    [SerializeField]
    private float maxPolarAngle = 120.0f;
    [SerializeField]
    private float mouseXSensitivity = 5.0f;
    [SerializeField]
    private float mouseYSensitivity = 5.0f;
    [SerializeField]
    private float scrollSensitivity = 5.0f;

    [SerializeField, Header("障害物がターゲットとカメラの間にあったら近づくか")]
    private bool isApproachObstacle = false;

    [SerializeField, Header("障害物とするレイヤー")]
    private LayerMask obstacleLayer = default(LayerMask);

    [SerializeField, Header("地面に当たった時に近づくか")]
    private bool isApproachGround = false;

    [SerializeField, Header("近づく速度")]
    private float approachSpeed = 0.0f;

    [SerializeField, Header("ロックオン時カメラが移動する角度")]
    private float moveAngle = 25f;

    //[SerializeField]
    //private BoxCollider boxCollider = null;

    private GameObject lockOnTarget;
    public GameObject LOCK
    {
        set { lockOnTarget = value; }
    }

    private float collisionDistance;

    private bool collisionObstacle;

    private bool stop = false;

    private Vector3 hitPos;

    private Vector3 lastPos;
    private float lastDistance;
    private float dist = 10f;
    private bool distFlag;

    


    void LateUpdate()
    {
        if (InputManager.InputMouseCheck(1) == INPUT_MODE.PLAY)
        {
            updateAngle(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        else
        {
            if (Input.GetKey(KeyCode.Q))
            {
                azimuthalAngle += 1;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                azimuthalAngle -= 1;
            }

            if(InputManager.InputMouseCheck(2) == INPUT_MODE.PLAY)
            {
                float y = Input.GetAxis("Mouse Y");

                y = polarAngle + y * mouseYSensitivity;
                polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);
            }
        }

        Vector3 lookAtPos;
        updateDistance(Input.GetAxis("Mouse ScrollWheel"));

        if (lockOnTarget)
        {
            if (lockOnTarget.GetComponent<Marker>().LOCK_OBSERVE)
            {
                Vector3 v = lockOnTarget.transform.position - target.transform.position;

                if (distFlag)
                {
                    dist = v.magnitude + lastDistance;
                    distFlag = true;
                }
                //float a = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg % 360 + 90;

                //azimuthalAngle = -a;
                updateAngle(0, 0);
                lookAtPos = lockOnTarget.transform.position;
                updatePosition(lookAtPos);
            }
            else
            {
                lookAtPos = target.transform.position + offset;
                updatePosition(lookAtPos);
                lastDistance = distance;
                lastPos = transform.position;
                distFlag = false;
                dist = 10f;
            }
        }
        else
        {
            lookAtPos = target.transform.position + offset;
            updatePosition(lookAtPos);
            lastDistance = distance;
            lastPos = transform.position;
            distFlag = false;
            dist = 10f;
        }

        transform.LookAt(lookAtPos);

        //　レイを視覚的に確認
        //Debug.DrawLine(target.transform.position + offset, transform.position, Color.red, 0f, false);
        //Vector3 b = target.transform.position + offset - transform.position;
        //Debug.DrawLine(transform.position, transform.position - b, Color.red, 0f, false);
        CantPenetrateObstacle();
    }

    void updateAngle(float x, float y)
    {
        if (!lockOnTarget || !lockOnTarget.GetComponent<Marker>().LOCK_OBSERVE)
        {
            x = azimuthalAngle - x * mouseXSensitivity;
            azimuthalAngle = Mathf.Repeat(x, 360);
        }
        else
        {
            Vector3 v = target.transform.position - transform.position;
            float a = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;

            Vector3 v2 = lockOnTarget.transform.position - transform.position;
            float a2 = Mathf.Atan2(v2.x, v2.z) * Mathf.Rad2Deg;

            float nas = Mathf.Acos(Vector2.Dot(new Vector2(v.x, v.z), new Vector2(v2.x, v2.z)) / (new Vector2(v.x,v.z).magnitude * new Vector2(v2.x,v2.z).magnitude)) * Mathf.Rad2Deg;
            float gai = Vector3.Cross(v, v2).y;

            if (nas > moveAngle)
            {
                if (gai < 0)
                {
                    azimuthalAngle += Mathf.Clamp((nas - moveAngle), 0, moveAngle);
                }
                else if (gai > 0)
                {
                    azimuthalAngle -= Mathf.Clamp((nas - moveAngle), 0, moveAngle);
                }
            }
        }

        y = polarAngle + y * mouseYSensitivity;
        polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);
    }

    void updateDistance(float scroll)
    {
        if (lockOnTarget && lockOnTarget.GetComponent<Marker>().LOCK_OBSERVE)
        {
            scroll = dist - scroll * scrollSensitivity;
            dist = Mathf.Clamp(scroll, minDistance + 10f, maxDistance + 30f);
        }
        else
        {
            if (!collisionObstacle && !stop)
            {
                scroll = distance - scroll * scrollSensitivity;
                distance = Mathf.Clamp(scroll, minDistance, maxDistance);
            }
        }
    }

    void updatePosition(Vector3 lookAtPos)
    {
        var da = azimuthalAngle * Mathf.Deg2Rad;
        var dp = polarAngle * Mathf.Deg2Rad;
        if (lockOnTarget && lockOnTarget.GetComponent<Marker>().LOCK_OBSERVE)
        {
            transform.position = new Vector3(
                lookAtPos.x + dist * Mathf.Sin(dp) * Mathf.Cos(da),
                lookAtPos.y + dist * Mathf.Cos(dp),
                lookAtPos.z + dist * Mathf.Sin(dp) * Mathf.Sin(da));
        }
        else
        {
            transform.position = new Vector3(
                lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
                lookAtPos.y + distance * Mathf.Cos(dp),
                lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
        }
    }

    void CantPenetrateObstacle()
    {
        if (isApproachObstacle)
        {
            RaycastHit hit;
            //　キャラクターとカメラの間に障害物があったら障害物の位置にカメラを移動させる
            if (Physics.Linecast(target.transform.position + offset, transform.position, out hit, obstacleLayer))
            {
                if(!collisionObstacle)
                {
                    collisionObstacle = true;
                    collisionDistance = distance;
                }
                //transform.position = hit.point;
                hitPos = hit.point;
                distance -= approachSpeed * Time.deltaTime;
            }
            else if(collisionObstacle)
            {
                Vector3 v = target.transform.position + offset - transform.position;

                if (!Physics.Linecast(transform.position, transform.position - v, out hit, obstacleLayer))
                {
                    distance += approachSpeed * Time.deltaTime;

                    if (distance >= collisionDistance)
                    {
                        distance = collisionDistance;
                        collisionObstacle = false;
                    }
                }
            }
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isApproachGround)
        {
            if (collision.gameObject.tag == "Ground")
            {
                distance -= approachSpeed * Time.deltaTime;
            }
        }
    }
}
