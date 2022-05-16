/*Answering support questions Application
 * Name: DesrializeJson.cs
 * 
 * Written by: Tobi Akinnola and Chigozie Muonagolu
 * 
 * Purpose: To load the json data and convert them to the appropriate data
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
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Application_Peer
{
    //This class desrializes json data and passes them to the send response class
    public class DesrializeJson
    {
        private string fileName;
        ChatResponsePackets loadedResponses;
        public DesrializeJson()
        {
            this.fileName = "Responses.json";
            loadedResponses = new ChatResponsePackets();
        }
        public ChatResponsePackets LoadChatResponses()
        {
            //Trys to deserialize the json file
            try
            {
                string json = File.ReadAllText(fileName);
                loadedResponses = JsonConvert.DeserializeObject<ChatResponsePackets>(json);
            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.ToString());
            }
            catch (JsonException ex)
            {
                throw new Exception(ex.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return loadedResponses;
        }
    }
}
