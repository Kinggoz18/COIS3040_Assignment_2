/*Answering support questions Application
 * Name: Peer_Server.cs
 * 
 * Written by: Tobi Akinnola and Chigozie Muonagolu
 * 
 * Purpose: To hold all the sockets and functions used for communication between the Discovery Server and the Peer Program
 * 
 * Uasage: Declare in main
 * 
 * Description of parameters: none
 * 
 * Namespaces required: See below
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Application_Peer
{
    public class Peer_Server
    {
        //Sockets for the Discorver Server
        Socket discorverSender;
        Socket discorverHandler;
        ServerHandler discover;
        public PeerListener peer;
        public PeerListEntry receivedPeer;

        public Peer_Server()
        {
            discover = new ServerHandler();
        }

        public void Start(Object listener)
        {
            PeerListener newListener = listener as PeerListener;
            if (peer == null && newListener != null)
            {
                this.peer = newListener;
            }
            Peer_Server thisPeer = new Peer_Server();
            Thread inputThread = new Thread(thisPeer.Start);
            if (newListener != null)
            {
                inputThread.Start(newListener.Socket);
            }
        }
        //Sends the IP and port number its socket to the discorver server
        public void SendPeerData()
        {
            try
            {

                //Converts the sockets endpoint into byte so they can be sent 
                byte[] data = Encoding.ASCII.GetBytes(peer.Socket.LocalEndPoint.ToString());
                byte[] end = Encoding.ASCII.GetBytes("END");
                byte[] data2 = data.Concat(end).ToArray();

                int bytesSent = discorverSender.Send(data2);

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.ToString());
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        //Creates a connection to the Discover Servers Socket using 
        public void connectToDiscorver()
        {
            discorverSender = discover.connectToDiscorver();
            Console.WriteLine("==================");
            Console.WriteLine("Connected to {0}", discorverSender.RemoteEndPoint);
            Console.WriteLine("==================");
        }
        //Retrieves a peers data after sending its own at start up
        public void retrievePeerData()
        {
            discorverHandler = peer.Socket;
            string data=null;
            //Data buffer for incoming data
            byte[] bytes = new byte[1024];
            try
            {
                bool moreData = true;
                while(true)
                {
                    Socket handler= discorverHandler.Accept();
                    while(moreData)
                    {
                        //The bytes received from the Discorver server
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("END") > -1)
                        {
                            moreData = false;
                        }
                    }
                    //Remove "END from the string"
                    data = data.Replace("END", "");
                    receivedPeer = convertToPeerEntry(data);
                    Console.WriteLine("Ready to connect to Peer " + receivedPeer.ToString());
                    if (moreData == false)
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        //Converts form string to PeerEntry
        public PeerListEntry convertToPeerEntry(string data)
        {
            IPEndPoint newEP = IPEndPoint.Parse(data);
            //Add them to the list of discorvered peers
            PeerListEntry entry = new PeerListEntry();
            entry.portNumber = newEP.Port;
            entry.ipAddress = newEP.Address;
            entry.endPoint = newEP;
            return entry;
        }
        //Converts the end point to byte to be sent back to the peer
        public byte[] convertToByte(PeerListEntry peer)
        {
            byte[] data = Encoding.ASCII.GetBytes(peer.endPoint.ToString());
            byte[] end = Encoding.ASCII.GetBytes("END");
            byte[] data2 = data.Concat(end).ToArray();
            return data2;
        }
    }
}
