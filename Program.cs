using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Console.WriteLine("Set game pool whit different odd number of moves:");
            string inptLine = Console.ReadLine();
            string[] avMoves = inptLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


            byte[] keyData = new byte[16];
            RandomNumberGenerator rkey = new RNGCryptoServiceProvider();
            rkey.GetBytes(keyData);
            string key = BitConverter.ToString(keyData).Replace("-", string.Empty);
            string botMove = botMoves(avMoves);
            
            Console.WriteLine(HMACHASH(botMove, keyData));

            Console.WriteLine("Awailiable moves:");
            foreach (string i in avMoves)
                Console.WriteLine("~  " + i);          
            Console.Write("Choose your move: ");
            string userM = Console.ReadLine();

            Console.WriteLine("Bot move: " + botMove);
            whoWins(createDict(inptLine), userM, botMove);

            Console.WriteLine(key);
            
        }

        static string HMACHASH(string str, byte[] bkey)
        {
            HMACSHA256 hmac = new HMACSHA256(bkey);
            byte[] bstr = Encoding.Default.GetBytes(str);
            byte[] bhash = hmac.ComputeHash(bstr);
            return BitConverter.ToString(bhash).Replace("-", string.Empty);
            
        }
        static string botMoves(string[] moves)
        {          
            string res = moves[new Random().Next(0, moves.Length - 1)];
            return res;
        }

        static void whoWins(Dictionary<string, string> myDict, string userMove, string botMove)
        {
            int count = 0;
            foreach (KeyValuePair<string, string> keyValue in myDict)
            {
                
                if (userMove.Equals(botMove))
                {
                    Console.WriteLine("Draw!");
                    return;
                }
                if (keyValue.Key.Equals(userMove) && keyValue.Value.Contains("[" + botMove + "]"))
                {
                    Console.WriteLine("You win!");
                    return;
                }
                else count++;
                if (count == myDict.Count) Console.WriteLine("You lose =(");
            }
        }
        static Dictionary<string, string> createDict(string s)
        {
            string str = s + " " + s;
            string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int stlen = words.Length / 2;
            Dictionary<string, string> dict = new Dictionary<string, string>(stlen);
            for (int i = 0; i < stlen; i++)
            {
                string res = "";
                for (int j = i + 1; j <= i + (stlen / 2); j++)
                {
                    res = res + "[" + words[j] + "] ";
                }   
                dict.Add(words[i], res);    
            }
            return dict;
        } 
    }
}
