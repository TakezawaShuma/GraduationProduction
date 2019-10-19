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

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private Color[] color = new Color[(int)State.Num];

    private UnityEvent unityEvent;

    private State state = State.None;

    public State STATE
    {
        get { return state; }
        set { state = value; }
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

    public void Execute()
    {
        unityEvent.Invoke();
    }
}
