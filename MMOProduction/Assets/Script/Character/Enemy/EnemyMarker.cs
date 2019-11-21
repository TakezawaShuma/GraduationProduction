using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMarker : MonoBehaviour
{
    [SerializeField]
    private Marker marker;

    public Marker Marker
    {
        get { return marker; }
    }
}
