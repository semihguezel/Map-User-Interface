using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using UnityEditor;

public class InputConnection : MonoBehaviour
{
    private Thread _mThread;
    IPAddress _localAdd;
    private TcpListener _listener;
    private TcpClient _client;
    private int _hasConnected;

    public string connectionIP = "127.0.0.1";
    public int connectionPort = 49152;
    private string _dataReceived;
    private bool _running;

    private void Start()
    {
        StartConnection();
    }
    //Set's connection with python script
    void StartConnection()
    {
        ThreadStart ts = new ThreadStart(GetInfo);
        _mThread = new Thread(ts);
        _mThread.Start();
    }
    void GetInfo()
    {
        _localAdd = IPAddress.Parse(connectionIP);
        _listener = new TcpListener(IPAddress.Any, connectionPort);
        _listener.Start();

        _client = _listener.AcceptTcpClient();
        Thread.Sleep(100);
        
        _running = true;
        
        while (_running)
        {
            SendAndReceiveData();
        }
        _listener.Stop();
    }
    void SendAndReceiveData()
    {
        NetworkStream nwStream = _client.GetStream();
        byte[] buffer = new byte[_client.ReceiveBufferSize];
        
        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, _client.ReceiveBufferSize); //Getting data in Bytes from Python
        _dataReceived= Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string
        Debug.Log("Received data :" + _dataReceived);
        
        if (_dataReceived.Contains("next"))
        {
            Debug.Log("argument received connection has been closed ...");
            _mThread.Abort();
        }
        if (!String.IsNullOrEmpty(_dataReceived))
        {
            //---Sending Data to Host----
            byte[] myWriteBuffer = Encoding.ASCII.GetBytes("Sending data to python  ..." ); //Converting string to byte data
            nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
        }

    }
    public new void SendMessage(string activationKey)
    {
        if (_dataReceived != null)
        {
            NetworkStream nwStream = _client.GetStream();
            byte[] myWriteBuffer =
                Encoding.ASCII.GetBytes(
                    activationKey); //Converting string to byte data
            nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
            _hasConnected = 1;
        }
        
        else
        {
            EditorUtility.DisplayDialog("Null reference exception has bee occured", "Please make sure connection between scripts established","ok", "");
            _hasConnected = 0;
        }
        
    }
    public int ConnectionStatus()
    {
        return _hasConnected;
    }
}
