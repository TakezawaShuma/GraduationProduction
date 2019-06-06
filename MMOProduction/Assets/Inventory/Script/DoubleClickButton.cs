using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor( typeof( DoubleClickButton ) )]
public class DoubleClickButtonInspector : Editor
{
}
#endif

public class DoubleClickButton : Button
{
    public float doubleClickTime = 0.3f;
    public UnityEvent OnDoubleClick;

    private float lastClickTime;

    private void DoubleClicked()
    {
        if (Time.time - lastClickTime < doubleClickTime)
        {
            OnDoubleClick.Invoke();
            return;
        }
        lastClickTime = Time.time;
    }

    protected override void OnEnable()
    {
        onClick.AddListener(DoubleClicked);
    }

    protected override void OnDisable()
    {
        onClick.RemoveListener(DoubleClicked);
    }
}