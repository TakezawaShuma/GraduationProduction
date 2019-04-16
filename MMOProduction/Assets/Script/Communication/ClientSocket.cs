using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

public class ClientSocket
{
    // ソケット
    private Socket m_socket = null;

    // 受信データ保存用
    private MemoryStream m_ms;

    // エンコード用変数
    private Encoding m_enc = Encoding.UTF8;

    /// <summary>
    /// イベントハンドラ
    /// </summary>

    // データ受信イベント
    public delegate void ReceiveEventHandler(object sender, string e);
    public event ReceiveEventHandler OnReceiveData;

    // 接続切断イベント
    public delegate void DisconnectedEventHandler(object sender, EventArgs e);
    public event DisconnectedEventHandler OnDisconnected;

    //接続OKイベント
    public delegate void ConnectedEventHandler(EventArgs e);

    Action<byte[]> onRecieveCallback;
    Action onConnectCallback;

    /** プロパティ **/
    /// <summary>
    /// ソケットが閉じているか
    /// </summary>
    public bool IsClosed
    {
        get { return (m_socket == null); }
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public virtual void Dispose()
    {
        //Socketを閉じる
        Close();
    }

    public ClientSocket()
    {
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Debug.Log("ソケットの作成");
    }
    public ClientSocket(Action<byte[]> onRecieve, Action onConnect)
    {
        onConnectCallback = onConnect;
        onRecieveCallback = onRecieve;
        //Socket生成
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }
    public ClientSocket(Socket sc)
    {
        m_socket = sc;
    }


    /// <summary>
    /// ソケットを閉じる
    /// </summary>
    public void Close()
    {
        Debug.Log("Close" + " ThreadID:" + Thread.CurrentThread.ManagedThreadId);

        //Socketを無効
        m_socket.Shutdown(SocketShutdown.Both);
        //Socketを閉じる
        m_socket.Close();
        m_socket = null;

        //受信データStreamを閉じる
        if (m_ms != null)
        {
            m_ms.Close();
            m_ms = null;
        }

        //接続断イベント発生
        OnDisconnected(this, new EventArgs());
    }


    /// <summary>
    /// 接続
    /// </summary>
    /// <param name="host">接続先IP</param>
    /// <param name="port">ポート</param>
    public void Connect(string  ip, int port)
    {
        Debug.Log("Connect" + " ThreadID:" + Thread.CurrentThread.ManagedThreadId);

        //IP作成
        IPAddress ipEnd =IPAddress.Parse(ip);
        Debug.Log(ipEnd);
        try
        {
            //ホストに接続
            m_socket.Connect(ipEnd, port);
            Debug.Log("接続施行");
        }
        catch { Debug.Log("接続失敗"); }
            // Connect to the remote endpoint.
        //m_socket.BeginConnect(ipEnd, new AsyncCallback(ConnectCallback), m_socket);

    }

    /// <summary>
    /// 接続コールバック
    /// </summary>
    /// <param name="ar"></param>
    private void ConnectCallback(IAsyncResult ar)
    {
        Debug.Log("ConnectCallback" + " ThreadID:" + Thread.CurrentThread.ManagedThreadId);

        try
        {
            //Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.
            //client.EndConnect(ar);

            //// コールバック
            //onConnectCallback();

            //データ受信開始
            StartReceive();
        }
        catch (SocketException e)
        {
            Debug.Log(e.ToString());
        }
    }
    /// <summary>
    /// データ受信開始
    /// </summary>
    public void StartReceive()
    {
        Debug.Log("StartReceive" + " ThreadID:" + Thread.CurrentThread.ManagedThreadId);

        //受信バッファ
        byte[] rcvBuff = new byte[1024];
        //受信データ初期化
        m_ms = new MemoryStream();


        m_socket.Receive(rcvBuff, rcvBuff.Length, SocketFlags.None);
        ////データ受信開始
        //m_socket.BeginReceive(rcvBuff, 0, rcvBuff.Length, SocketFlags.None, new AsyncCallback(ReceiveDataCallback), rcvBuff);
    }

    /// <summary>
    /// データ受信
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveDataCallback(IAsyncResult ar)
    {
        Debug.Log("ReceiveDataCallback" + " ThreadID:" + Thread.CurrentThread.ManagedThreadId);

        int len = -1;

        if (IsClosed) { return; }
        len = m_socket.EndReceive(ar);

        //切断された
        if (len <= 0)
        {
            Close();
            return;
        }

        //受信データ取り出し
        byte[] rcvBuff = (byte[])ar.AsyncState;
        Debug.Log(rcvBuff.Length);
        onRecieveCallback(rcvBuff);

        //非同期受信を再開始
        if (!IsClosed) { m_socket.BeginReceive(rcvBuff, 0, rcvBuff.Length, SocketFlags.None, new AsyncCallback(ReceiveDataCallback), rcvBuff); }

    }

    /// <summary>
    /// データ送信
    /// </summary>
    /// <param name="bytes">送信データ(byte)</param>
    public void Send(byte[] bytes)
    {
        if (!IsClosed)
        {
            m_socket.Send(bytes);
            Debug.Log("Send OK");
        }
    }
    /// <summary>
    /// データ送信する
    /// </summary>
    /// <param name="str">送信データ(string)</param>
    public void Send(string str)
    {
        Debug.Log("Send" + " ThreadID:" + Thread.CurrentThread.ManagedThreadId);

        if (!IsClosed)
        {
            //文字列をBYTE配列に変換
            byte[] sendBytes = m_enc.GetBytes(str + "\r\n");

            //送信
            m_socket.Send(sendBytes);
            Debug.Log("Send ok");

        }
    }
}
