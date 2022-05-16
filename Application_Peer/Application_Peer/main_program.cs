/*Answering support questions Application
 * Name: main_program.cs
 * 
 * Written by: Tobi Akinnola and Chigozie Muonagolu
 * 
 * Purpose: To help bring together all the functions of the peer
 * 
 * Uasage: none
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

    //2 Cases
    //Case 1 the peer is not connected to itself
    //Case 2 the peer is connected to itself
    public class main_program
    {
        public static void Main()
        {
            string text, response;
            int i = 0;
            Peer_Server thisPeer = new Peer_Server();
            PeerListener peer=new PeerListener();
            //Starts its own server, Connect to the discorvered server
            //Sends it data to the discorvered server and retrieves a peer's data from the discorvered servers
            thisPeer.Start(peer);
            thisPeer.connectToDiscorver();
            thisPeer.SendPeerData();
            thisPeer.retrievePeerData();

            //Connects to the received peer
            Peer_Application application = new Peer_Application(thisPeer.receivedPeer, peer);
            application.connectToReceivedPeer();
            //Create a second thread to handle the servers automated functions
            //let the main thread handle the clients input driven functions
            Thread runPeerServer = new Thread(application.runPeerServer);
            bool isConnectedToItself = false;
            //CASE 1: The PEER IS NOT CONNECTED TO ITSELF
            if (thisPeer.receivedPeer.endPoint.Equals((IPEndPoint)peer.Socket.LocalEndPoint))
            {
                Console.WriteLine("PEER is connected to itself");
                isConnectedToItself = true;
            }
            //CASE 2: THE PEER IS CONNECTED TO ITSELF
            else
            {
                Console.WriteLine("PEER is not connected to itself");
            }
            Console.WriteLine("==================================");
            Console.WriteLine("Welcome to our support application");
            Console.WriteLine("==================================");

            Console.WriteLine("Please input a command:\nC - to have peer act as a Client, S - to have peer act as a Server");
            char opt=Convert.ToChar(Console.ReadLine());
            opt = char.ToUpper(opt);
            //This peer has been choses has a client
            if(opt =='C')
            {
                if (isConnectedToItself)
                    runPeerServer.Start();
                while (true)
                {
                    Console.Write("Enter:", i);
                    text = Console.ReadLine();
                    text = text.ToLower();
                    application.sendQuestion(text);
                    if (text == "quit")
                    {
                        break;
                    }
                    response = application.getResponse();
                    text = text.ToString();
                    ++i;
                    Console.WriteLine("Reply: {0}", response);
                }
            }
            //This peer has been chosen as a server
            else if(opt == 'S')
            {
                //Starts a thread of the server side of the app
                Console.WriteLine("Peer's server Functions have started.");
                application.runPeerServer();
            }
            if (isConnectedToItself)
                runPeerServer.Join();
            Console.WriteLine("Application has ended");
            Console.WriteLine();
        }
    }
}
