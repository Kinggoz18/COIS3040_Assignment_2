/*Answering support questions Application
 * Name: PeerListEntry.cs
 * 
 * Written by: Tobi Akinnola and Chigozie Muonagolu
 * 
 * Purpose: Class to hold the received peers End Point data
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
using System.Text;
using System.Threading.Tasks;

namespace Application_Peer
{
    public class PeerListEntry
    {
        public IPEndPoint endPoint;
        public IPAddress ipAddress;
        public int portNumber;
        public override string ToString()
        {
            return string.Format("{0}       {1}", ipAddress, portNumber);
        }
    }
}
