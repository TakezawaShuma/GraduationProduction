using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;


public class NodeJsClient : MonoBehaviour
{
    [SerializeField]
    string serverIp;
    [SerializeField]
    int port;

    WebSocket ws;
    private void Start()
    {
        ws = new WebSocket("ws://" + serverIp + ":" + port.ToString());

        ws.OnOpen += (sender, e) =>
          {
              Debug.Log("WebSocket Open");
          };
        ws.OnMessage += (sender, e) =>
          {
              Debug.Log("WebSocket Message Type: " + e.GetType() + ", Data : " + e.Data);

          };

        ws.OnError += (sender, e) =>
        {
            Debug.Log("WebSocket Error Message: " + e.Message);
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket Close");
        };

        ws.Connect();
    }

    private void Update()
    {
        if(Input.GetKeyUp("s"))
        {
            ws.Send("Test Message");
        }

    }

    private void OnDestroy()
    {
        ws.Close();
        ws = null;
    }


    //[SerializeField]
    //private string serverIp;
    //[SerializeField]
    //private int port;
    //
    //// Share tran sform
    //[SerializeField]
    //private Transform syncObjTransform;
    //
    //[SerializeField]
    //private SyncPhase nowPhase;
    //
    //private WebSocket ws;
    //
    //public enum SyncPhase
    //{
    //    Idling,
    //    Syncing
    //}
    //private void Awake()
    //{
    //    nowPhase = SyncPhase.Idling;
    //    var cTransformValue = gameObject.ObserveEveryValueChanged(_ => _syncObjTransform.position);
    //}
    //
    //public void OnSyncStartButtonDown()
    //{
    //    var ca = "ws://" + serverIp + ":" + port.ToString();
    //    Debug.Log("Connect to" + ca);
    //    ws = new WebSocket(ca);
    //
    //
    //    // Add Events
    //    // On catch message evect
    //    ws.OnMassage += (object sender, MessageEventArgs e) => { print(e.Data); };
    //
    //    // On error event
    //    ws.OnError += (sender, e) =>
    //    {
    //        Debug.Log("WebSocket Error Message: " + e.Message);
    //        nowPhase = SyncPhase.Idling;
    //    };
    //
    //    //On WebSocket close event
    //    ws.OnClose += (sender, e) =>
    //    {
    //        Debug.Log("Disconnected Server");
    //    };
    //
    //    ws.Connect();
    //
    //    nowPhase = SyncPhase.Syncing;
    //}
    //
    ///// <summary>
    ///// Get Down Stop Sync Button
    ///// </summary>
    //public void OnSyncStopButtonDown()
    //{
    //    ws.Close(); //Disconnect
    //}
    //
    //public void OnChangedTargetTransformValue(Vector3 pos)
    //{
    //    if (nowPhase == SyncPhase.Syncing)
    //    {
    //        Debug.Log(pos);
    //        ws.Send(pos.ToString());
    //    }
    //}

}


