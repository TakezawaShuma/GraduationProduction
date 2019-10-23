using UnityEngine;

public class UIController_WorldSpace : MonoBehaviour
{
    [SerializeField, Header("カメラ")]
    private Camera camera;
    private RectTransform myRectTfm;

    void Start()
    {
        myRectTfm = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 自身の向きをカメラに向ける
        myRectTfm.LookAt(camera.transform);
    }
}