using UnityEngine;

public class UIController_WorldSpace : MonoBehaviour
{
    private RectTransform myRectTfm;

    void Start()
    {
        myRectTfm = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 自身の向きをカメラに向ける
        myRectTfm.LookAt(Camera.main.transform);
        myRectTfm.Rotate(0, 180, 0);
    }
}