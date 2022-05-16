using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Peer
{
    /*Answering support questions Application
    * Name: IBaseKeyboard
    * 
    * Written by: Tobi Akinnola and Chigozie Muonagolu
    * 
    * Purpose: Interface class for RealKeyboard class and TestKeyboard class
    * 
    * Uasage: inherited by RealKeyboard and TestKeyboard
    * 
    * Description of parameters: none
    * 
    * Namespaces required: See below
   */
    public interface IBaseKeyboard
    {
        public string ReadInput();
        public void WriteOutput(string output);
    }
    /*Answering support questions Application
    * Name: RealKeyboard
    * 
    * Written by: Tobi Akinnola and Chigozie Muonagolu
    * 
    * Purpose: Class to abstract the keyboard input and console output, this class is not automated 
    * 
    * Uasage: IBaseKeyboard keyboard = new TestKeyboard();
    * 
    * Description of parameters: none
    * 
    * Namespaces required: See below
    */
    public class RealKeyboard : IBaseKeyboard
    {
        public string ReadInput()
        {
            string input = Console.ReadLine();
            return input;
        }
        public void WriteOutput(string output)
        {
            Console.WriteLine(output);
        }
    }
    /*Answering support questions Application
   * Name: TestKeyboard
   * 
   * Written by: Tobi Akinnola and Chigozie Muonagolu
   * 
   * Purpose: Class to abstract the keyboard input and console output, this class is automated 
   * 
   * Uasage: IBaseKeyboard keyboard = new TestKeyboard();
   * 
   * Description of parameters: none
   * 
   * Namespaces required: See below
   */
    public class TestKeyboard : IBaseKeyboard
    {
        //Array of stored questions
        string[] responses = { "Who has the most rings in the nba", "Who was the first man on the moon",
            "Where is Trent University located", "What is the meaning of life", "Where is the closest coffee shop",
            "Why is flying impossible", "Why is called the dead sea", "What happened at the museum", "Where is the party located","Who was the first prime minister of canada"};
        int count = 0;
        public string ReadInput()
        {
            Random rand = new Random();
            int strIndex = rand.Next(0, responses.Length);
            //Quit after 10 runs
            if (count == 9)
            {
                return "quit";
            }
            return responses[strIndex];
        }
        public void WriteOutput(string output)
        {
            Console.WriteLine(output);
        }
    }
}
