using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SupportQuestionsApplication_DiscoveryService
{
/*Answering support questions Application
 * Name: DiscoverySocket
 * 
 * Written by: Tobi Akinnola and Chigozie Muonagolu
 * 
 * Purpose: A class to setup the socket used to communicate with discovered peers
 * 
 * Uasage: call in main_program
 * 
 * Description of parameters: none
 * 
 * Namespaces required: See below
*/
    public class DiscoverySocket
    {
        private static IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        private static IPAddress ipAddress = ipHostInfo.AddressList[0];
        private static Socket DiscorverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        private static IPEndPoint discoverServerEP = new IPEndPoint(ipAddress, 11000);

        public Socket setUpSocket()
        {
            try
            {
                DiscorverSocket.Bind(discoverServerEP);
                DiscorverSocket.Listen(10);
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
    /*Answering support questions Application
    * Name: PeerListEntry
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
