// Source: https://msdn.microsoft.com/en-us/library/system.net.sockets.socket(v=vs.80).aspx

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class NamedServerStream : MonoBehaviour {
    
    public int port;

    Thread tcpListenerThread;

    private Queue<DataPointViewModel> dataPointsQueue = new Queue<DataPointViewModel>();
    private object queueLock = new System.Object();

	// Use this for initialization
	void Start () {
        tcpListenerThread = new Thread(() => ListenForMessages(port));
        tcpListenerThread.Start();
	}
	
	// Update is called once per frame
	void Update () {
        Queue<DataPointViewModel> tempqueue;
	    lock (queueLock)
	    {
            tempqueue = dataPointsQueue;
            dataPointsQueue = new Queue<DataPointViewModel>();
        }
        foreach (var point in tempqueue)
        {
            Debug.Log("Read Message");
            SendMessage("DataReceived", point);
        }
    }

    public void ListenForMessages(int port)
    {
        TcpListener server = null;
        try
        {
            // Set the TcpListener on port 13000.
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            // Enter the listening loop.
            while (true)
            {
                Debug.Log("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                using (TcpClient client = server.AcceptTcpClient())
                {

                    Debug.Log("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Debug.Log(String.Format("Received: {0}", data));

                        var viewModel = JsonUtility.FromJson<DataPointViewModel>(data);
                        lock (queueLock)
                        {
                            dataPointsQueue.Enqueue(viewModel);
                        }

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Debug.Log(String.Format("Sent: {0}", data));
                    }
                }
            }
        }
        catch (SocketException e)
        {
            Debug.LogError(String.Format("SocketException: {0}", e));
        }
        finally
        {
            // Stop listening for new clients.
            server.Stop();
        }
    }
}

//using System;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//class MyTcpListener
//{
//    public static void Main()
//    {
//        TcpListener server = null;
//        try
//        {
//            // Set the TcpListener on port 13000.
//            Int32 port = 13000;
//            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

//            // TcpListener server = new TcpListener(port);
//            server = new TcpListener(localAddr, port);

//            // Start listening for client requests.
//            server.Start();

//            // Buffer for reading data
//            Byte[] bytes = new Byte[256];
//            String data = null;

//            // Enter the listening loop.
//            while (true)
//            {
//                Console.Write("Waiting for a connection... ");

//                // Perform a blocking call to accept requests.
//                // You could also user server.AcceptSocket() here.
//                TcpClient client = server.AcceptTcpClient();
//                Console.WriteLine("Connected!");

//                data = null;

//                // Get a stream object for reading and writing
//                NetworkStream stream = client.GetStream();

//                int i;

//                // Loop to receive all the data sent by the client.
//                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
//                {
//                    // Translate data bytes to a ASCII string.
//                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
//                    Console.WriteLine("Received: {0}", data);

//                    // Process the data sent by the client.
//                    data = data.ToUpper();

//                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

//                    // Send back a response.
//                    stream.Write(msg, 0, msg.Length);
//                    Console.WriteLine("Sent: {0}", data);
//                }

//                // Shutdown and end connection
//                client.Close();
//            }
//        }
//        catch (SocketException e)
//        {
//            Console.WriteLine("SocketException: {0}", e);
//        }
//        finally
//        {
//            // Stop listening for new clients.
//            server.Stop();
//        }


//        Console.WriteLine("\nHit enter to continue...");
//        Console.Read();
//    }
//}