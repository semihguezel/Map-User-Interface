using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;

public class AgvConnection : MonoBehaviour
{
    private Thread _mThread;
    IPAddress _localAdd;
    private TcpListener _listener;
    private TcpClient _client;

    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25001;
    private string _dataReceived;
    private bool _running;
    Vector3 _receivedPos = Vector3.zero;
    
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
            //---Using received data---
            _receivedPos = StringToVector3(_dataReceived); //<-- assigning receivedPos value from Python
            print("received pos data, and moved the AGV!" + _receivedPos);
            
            //---Sending Data to Host----
            byte[] myWriteBuffer = Encoding.ASCII.GetBytes("Sending data to python  ..." ); //Converting string to byte data
            nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
        }

    }
    private static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }
        
        // split the items
        string[] sArray = sVector.Split(',');
        
        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0], CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(sArray[1], CultureInfo.InvariantCulture.NumberFormat),
            float.Parse(sArray[2], CultureInfo.InvariantCulture.NumberFormat));

        return result;
    }
    public Vector3 GetPosition()
    {
        return _receivedPos;
    }
    public new void SendMessage(string activationKey)
    {
        NetworkStream nwStream = _client.GetStream();
        byte[] myWriteBuffer =
            Encoding.ASCII.GetBytes(
                    activationKey); //Converting string to byte data
        nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
        
    }
}