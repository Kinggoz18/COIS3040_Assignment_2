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

namespace SupportQuestionsApplication_DiscoveryService
{
    public class main_program
    {
        public static void Main()
        {
            Directory_Service discorveryService = new Directory_Service();
            while (true)
            {

                discorveryService.runConnection();
                Console.WriteLine("Discovered Peer's");
                foreach(PeerListEntry peer in discorveryService.peers)
                {
                    Console.WriteLine(peer.ToString());
                }
                discorveryService.sendPeerData();
            }
            Console.ReadLine();
        }
    }
}
