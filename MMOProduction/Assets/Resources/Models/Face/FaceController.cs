using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

public class FaceController : MonoBehaviour
{
    public float interval;

    private VRMBlendShapeProxy param;

    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        param = GetComponent<VRM.VRMBlendShapeProxy>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval) { param.SetValue(VRM.BlendShapePreset.Blink, 1); timer = 0; }
        else { param.SetValue(VRM.BlendShapePreset.Blink, 0); }
    }
}
