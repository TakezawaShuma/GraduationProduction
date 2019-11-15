using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build {
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad() {
        Screen.SetResolution(1920, 1080, true);
    }
}

