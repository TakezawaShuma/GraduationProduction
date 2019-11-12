using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Marker : MonoBehaviour
{
    public enum State
    {
        None,
        Choice,

        Num,
    }

    public enum Type
    {
        NPC,
        Enemy,
    }

    [SerializeField]
    private SpriteRenderer sprite = null;

    [SerializeField]
    private Color[] color = new Color[(int)State.Num];

    [SerializeField, Header("距離")]
    private float executeRange = 5;

    [SerializeField]
    private Type type = Type.NPC;

    public Type TYPE
    {
        get { return type; }
    }

    private UnityEvent unityEvent;

    private State state = State.None;

    public State STATE
    {
        get { return state; }
        set { state = value; }
    }

    [SerializeField, Header("ロックした時に注目するか")]
    private bool lockObserve = false;

    public bool LOCK_OBSERVE
    {
        get { return lockObserve; }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        unityEvent = new UnityEvent();

        color[0] = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        sprite.color = color[(int)state];
    }

    public void SetFunction(UnityAction unityAction)
    {
        unityEvent.RemoveAllListeners();
        unityEvent.AddListener(unityAction);
    }

    public void Execute(Vector3 pos)
    {
        Vector3 v = pos - transform.position;
        float distance = v.magnitude;
        if (distance <= executeRange)
        {
            unityEvent.Invoke();
        }
    }
}
