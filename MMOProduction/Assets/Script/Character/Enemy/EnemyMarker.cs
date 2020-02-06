using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMarker : MonoBehaviour
{
    [SerializeField]
    private GameObject marker = null;

    public GameObject MARKER
    {
        set { marker = value; }
        get { return marker; }
    }
}
