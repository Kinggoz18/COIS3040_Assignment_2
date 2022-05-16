/*Answering support questions Application
* Name: Peer_Server.cs
* 
* Written by: Tobi Akinnola and Chigozie Muonagolu
* 
* Purpose: Base class for the sockets used to connect to the peer and the discovery server
* 
* Uasage: Declare in main
* 
* Description of parameters: none
* 
* Namespaces required: See below
*/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Application_Peer
{
    //Listens for incoming connection from other peers
    public class PeerListener
    {
        public IPEndPoint peerEndPoint;
        private Socket socket;
        public PeerListener()
        {
            //Sets up the socket when a new peer listener is created
            socket=setUpSocket();
        }
        public Socket Socket
        {
            get { return socket; }
        }

        //Set up the peer's listening socket
        private Socket setUpSocket()
        {
            //Assign an IP and endpoint for this peer
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            //set a server endpoint using a free port
            IPEndPoint EP = new IPEndPoint(ipAddress, 0);
            Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(EP);
            peerEndPoint = (IPEndPoint)serverSocket.LocalEndPoint;
            serverSocket.Listen(10);
            return serverSocket;
        }

        //Sets up the peers handlerSocket
    }
    //Handles sending its IP and Port combination to the discorver server
    public class ServerHandler
    {
        //Creates a connection to the discorver socket and returns the socket
        public Socket connectToDiscorver()
        {
            IPHostEntry iPHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = iPHostInfo.AddressList[0];
            Socket DiscorverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint discoverServerEP = new IPEndPoint(ipAddress, 11000);
            try
            {
                DiscorverSocket.Connect(discoverServerEP);
                return DiscorverSocket;
            }
            catch (SocketException ex)
            {
                throw new Exception(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}