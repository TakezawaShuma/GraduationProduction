//////////////////////////////////////////////
// プレイヤーに追従するカメラを制御用クラス //
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, DisallowMultipleComponent]
public class FollowingCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject target; // an object to follow

    public GameObject Target
    {
        set { target = value; }
    }

    [SerializeField]
    private Vector3 offset = new Vector3(0f, 1.5f, 0f); // offset form the target object

    [SerializeField]
    private float distance = 2.0f; // distance from following object
    [SerializeField]
    private float polarAngle = 75.0f; // angle with y-axis
    [SerializeField]
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

    private GameObject lockOnTarget;
    public GameObject LOCK
    {
        set { lockOnTarget = value; }
    }

    /// <summary>
    /// ターゲットの設定
    /// </summary>
    /// <param name="_target"></param>
    public void SetTarget(GameObject _target) {
        target = _target;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            updateAngle(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        updateDistance(Input.GetAxis("Mouse ScrollWheel"));

        Vector3 lookAtPos;

        if (lockOnTarget)
        {
            // ここでazimuthalAngleをいい感じにする
            Vector3 v = lockOnTarget.transform.position - target.transform.position;

            float a = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg % 360 + 90;

            Debug.Log(azimuthalAngle + ":" + a);

            azimuthalAngle = -a;

            lookAtPos = target.transform.position + offset;
            updatePosition(lookAtPos);
        }
        else
        {
            lookAtPos = target.transform.position + offset;
            updatePosition(lookAtPos);
        }

        transform.LookAt(lookAtPos);
    }

    void updateAngle(float x, float y)
    {

        if (!lockOnTarget)
        {
            x = azimuthalAngle - x * mouseXSensitivity;
            azimuthalAngle = Mathf.Repeat(x, 360);
        }

        y = polarAngle + y * mouseYSensitivity;
        polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);
    }

    void updateDistance(float scroll)
    {
        scroll = distance - scroll * scrollSensitivity;
        distance = Mathf.Clamp(scroll, minDistance, maxDistance);
    }

    void updatePosition(Vector3 lookAtPos)
    {
        var da = azimuthalAngle * Mathf.Deg2Rad;
        var dp = polarAngle * Mathf.Deg2Rad;
        transform.position = new Vector3(
            lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(da),
            lookAtPos.y + distance * Mathf.Cos(dp),
            lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(da));
    }
}
