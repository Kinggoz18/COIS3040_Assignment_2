/*Answering support questions Application
 * Name: Directory_Service.cs
 * 
 * Written by: Tobi Akinnola and Chigozie Muonagolu
 * 
 * Purpose: A class to hold the methods/functions for communincating with discovered peers
 * 
 * Uasage: call in main_program
 * 
 * Description of parameters: none
 * 
 * Namespaces required: See below
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

namespace SupportQuestionsApplication_DiscoveryService
{

    public class Directory_Service
    {
        //For receiving from the Peer
        private Socket listener;
        private Socket handler;
        //For sending back to the peer
        private Socket peerHandler;
        public static string data = null;
        public List<PeerListEntry> peers;
        public PeerListEntry LastEntry;
        public int peerCount;
        public Directory_Service()
        {
            listener = new DiscoverySocket().setUpSocket();
            peers = new List<PeerListEntry>();
            LastEntry = new PeerListEntry();
            peerCount = 0;
        }
        //Gets the Ip Address and Port number form the most recent connected Peer and adds it to the list
        //Returns true if successfull, flase if something went wrong
        public void ReceivePeerData()
        {
            bool moreData = true;
            string data = null;
            //Data buffer for incoming data
            byte[] bytes = new byte[1024];
            while (true)
            {
                //handler.Accept();
                try
                {
                    while (moreData)
                    {
                        //The bytes received from the Discorver server
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("END") > -1)
                        {
                            moreData = false;
                        }
                    }
                    if (moreData == false)
                        break;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            //Remove "END from the string"
            data = data.Replace("END", "");
            LastEntry=convertToPeerEntry(data);
            peers.Add(LastEntry);
        }
        //Sends a random Ip and port combination from the peer list back to the last add peer entry
        public void sendPeerData()
        {
            byte[] data;
            PeerListEntry peerData=new PeerListEntry(); 
            //If there's more than 1 item in the list
            if (peers.Count > 1) 
            {
                Random rnd=new Random();
                int index = rnd.Next(0, peers.Count);
                peerData= peers[index];
                data = convertToByte(peerData);
            }
            else
            {
                peerData = peers.First();
                data= convertToByte(peerData);
            }
            //Creates a connection to the peer socket 
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //Connect to the last added peer's end point
                sender.Connect(LastEntry.endPoint);
                sender.Send(data);
                Console.WriteLine("Sent data to peer {0}", handler.RemoteEndPoint.ToString());
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
        //Starts the connection - called once in constructor 
        public Socket runConnection()
        {
            //Starts the connecttion and the new socket handler is created
            try
            {
                Console.WriteLine("Creating a connection...");
                handler = listener.Accept();
                Console.WriteLine("==================");
                Console.WriteLine("Connected to Peer {0}", handler.RemoteEndPoint);
                Console.WriteLine("==================");
                ReceivePeerData();
            }
            catch (SocketException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return handler;
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
