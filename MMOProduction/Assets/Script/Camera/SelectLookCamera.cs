using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLookCamera : MonoBehaviour
{

    [SerializeField]
    private Transform _target = null;

    private Vector3 _originPos;
    private Vector3 _originRot;

    System.Action _onUpdate;


    private void Start()
    {
        _originPos = this.transform.position;
        _originRot = this.transform.eulerAngles;

        _onUpdate = (() => { });
    }

    private void Update()
    {
        _onUpdate.Invoke();
    }

    public void EntryTarget(Transform target)
    {
        _target = target;
        _onUpdate = LookAtTarget;
    }

    public void RemoveTarget()
    {
        _target = null;
        _onUpdate = ReturnDefaultPoint;
    }

    private void LookAtTarget()
    {
        this.transform.eulerAngles = Vector3.Lerp(
                this.transform.eulerAngles,
                new Vector3(_target.eulerAngles.x, Mathf.Abs(_target.eulerAngles.y - 180), _target.eulerAngles.z),
                Time.deltaTime * 2
                );

        Vector3 targetFront = _target.transform.position + _target.forward * 8;
        this.transform.position = Vector3.Lerp(
        this.transform.position,
        new Vector3(targetFront.x, targetFront.y + 3, targetFront.z),
        Time.deltaTime * 2
        );
    }

    private void ReturnDefaultPoint()
    {
        this.transform.eulerAngles = Vector3.Lerp(
                this.transform.eulerAngles,
                _originRot,
                Time.deltaTime * 2
                );

        this.transform.position = Vector3.Lerp(
        this.transform.position,
        _originPos,
        Time.deltaTime * 2
        );
    }
}
