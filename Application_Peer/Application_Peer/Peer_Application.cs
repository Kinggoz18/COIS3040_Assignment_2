/*Answering support questions Application
 * Name: Peer_Application.cs
 * 
 * Written by: Tobi Akinnola and Chigozie Muonagolu
 * 
 * Purpose: To hold all the server and client functions of the Application
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
    //A class containing all the peer's functions
    public class Peer_Application
    {
        private Dictionary<int, string> responses;
        private string[] keywords = { "hello", "hi", "most", "rings", "nba","first", "moon", "trent", "university", "located","location", "mean", "meaning", "life", "coffee", "shop", "flying",
            "fly","impossible", "sea", "museum", "party", "prime", "minister", "canada",  "dead" };
        private DesrializeJson data;
        private ChatResponsePackets packets;
        private int listLen;
        private int[] keys;
        //Used to receive and send to server peer
        Socket peerHandler;
        //Used to receive and send to client peer
        Socket peerListener;
        Socket handler;

        PeerListEntry peer;
        //Data buffer from incoming from connected peer
        byte[] bytes;

        private string userQuestion;
        public Peer_Application(PeerListEntry peer, PeerListener socket)
        {
            this.peer = peer;
            this.peerListener = socket.Socket;
            //initializes the following and loads the responses into the list
            this.data = new DesrializeJson();
            this.packets = data.LoadChatResponses();

            this.responses = new Dictionary<int, string>();
            listLen = packets.Responses.Count;
            keys = new int[listLen];
            populateDictionary();
        }
        //Runs a thread of the application
        public void runPeerServer()
        {
            string userInput = "";
            while (true)
            {
                userInput = GetUserQuestion();
                //If user input is quit end connection
                userInput = userInput.ToLower();
                if(userInput == "quit?")
                {
                    Console.WriteLine("Ending Connection");
                    EndConnection();
                    break;
                }
                Console.WriteLine("Clinet: " + userInput);
                SendResponse();
            }
        }
        public void EndConnection()
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            peerHandler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        //Creates a connection to the received peer's socket
        public void connectToReceivedPeer()
        {
            //Creates a connection to the peer socket 
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            peerHandler = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                peerHandler.Connect(peer.endPoint);
                Console.WriteLine("Connection Established to Peer: {0}", peerHandler.RemoteEndPoint.ToString());
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
        //////////////////////////////////////////////////////////////////////////////////////////////////
        ///Client Function Methods
        //Gets response from the server
        public string getResponse()
        {
            bool moreData = true;
            string response = "";
            byte[] data= new byte[1024];
            while (moreData)
            {
                // Receive the response from the remote device.  
                int receivedBytes = peerHandler.Receive(data);
                response += Encoding.ASCII.GetString(data, 0, receivedBytes);
                if (response.IndexOf('.') > -1)
                {
                    moreData = false;
                }
            }
            return response;
        }
        //Sends questions to the server
        public void sendQuestion(string question)
        {
            question += "?";
            // Encode the data string into a byte array for seding across the network.  
            byte[] msg;

            msg = Encoding.ASCII.GetBytes(question);
            peerHandler.Send(msg);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        ///Server Function Methods
        private void populateDictionary()
        {
            string current;
            int i = 0;
            string[] words;
            string key;
            //Creates Keywords and uses them to store the values in the dictionary
            while (i < listLen)
            {
                current = packets.Responses[i];
                key = "";
                words = current.Split(' ', '.', '!');
                foreach (string s in words)
                {
                    //If the key word in the response if found add first word to the key at index i to build its key 
                    if (keywords.Contains(s.ToLower()))
                    {
                        key += s[0];
                    }
                }
                keys[i] = hashKey(key.ToLower());
                i += 1;
            }
            //Finally stores the Key and their values in the dictionary
            for (int a = 0; a < listLen; a++)
            {
                current = packets.Responses[a];
                responses.Add(keys[a], current);
            }
        }
        //Hashes the key for the dictionary
        private int hashKey(string key)
        {
            char[] chars = key.ToCharArray();
            int newKey = 0;
            foreach (char c in chars)
            {
                newKey += c.ToString().GetHashCode();
            }
            return newKey;
        }
        //Sends a response to the client program
        public void SendResponse()
        {
            string reply = findResponse(FindKeyWord());
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(reply);
                handler.Send(msg);
            }
            catch (SocketException ex)
            {
                throw new Exception(ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //Gets the Users Input from the Client side
        public string GetUserQuestion()
        {
            bool moreData = true;
            bytes = new Byte[1024];
            userQuestion = "";
            try
            {
                //Sleep before restart for 5 seconds
                //Thread.Sleep(3000);
                if (handler==null)
                {
                    handler = peerListener.Accept();
                }
                while (moreData)
                {

                    int receivedBytes = handler.Receive(bytes);
                    userQuestion += Encoding.ASCII.GetString(bytes, 0, receivedBytes);
                    if (userQuestion.IndexOf('?') > -1 || (userQuestion == "Q"))
                    {
                        moreData = false;
                    }
                }
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode == 10060)
                {
                    Console.WriteLine("Application timeout! User took too long to reply after, {0} milliseconds", peerListener.ReceiveTimeout);
                    throw new Exception(ex.ToString());
                }
                else
                    throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return userQuestion;
        }
        //Finds the apropriate response from the dictionary list
        private string findResponse(string key)
        {
            int newKey = hashKey(key);
            int defaultKey = hashKey("");
            if (keys.Contains(newKey))
            {
                return responses[newKey];
            }
            else
            {
                return responses[defaultKey];
            }
        }
        //Finds key words in users input
        private string FindKeyWord()
        {
            try
            {
                string[] splitText = userQuestion.Split(' ', '?');
                int keyIndex = 0, i = 0;
                string Key = "";
                foreach (string s in splitText)
                {
                    if (keywords.Contains(s.ToLower()))
                    {
                        Key += s[i];
                    }
                }
                return Key.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        //Starts the connection - called once in constructor 
        //Disconnects the servers socket
        public void Quit()
        {
            peerListener.Close();
            Console.WriteLine("Application ended.");
        }
    }
}
