/*Answering support questions Application
 * Name: ChatResponsePackets.cs
 * 
 * Written by: Tobi Akinnola and Chigozie Muonagolu
 * 
 * Purpose: Creates a list of strings to store the json responses
 * 
 * Uasage: call in the main_program
 * 
 * Description of parameters: none
 * 
 * Namespaces required: See below
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Peer
{
    public class ChatResponsePackets
    {
        public IList<string> Responses { get; set; }
    }

}
