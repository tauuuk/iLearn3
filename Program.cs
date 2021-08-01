using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
       
        static void Main(string[] argv)
        {
            if (isCorrect(argv) == false) return;
            byte[] keyData = new byte[16];
            RandomNumberGenerator rkey = new RNGCryptoServiceProvider(keyData);
            rkey.GetBytes(keyData);
            string key = BitConverter.ToString(keyData).Replace("-", string.Empty);      
            string botMove = argv[new Random().Next(0, argv.Length - 1)];         
            Console.WriteLine(HMACHASH(botMove, key));
            Console.WriteLine("Awailiable moves:");
            Dictionary<int, string> enterDict = new Dictionary<int, string>();
            for (int i = 1; i <= argv.Length; i++)
                enterDict.Add(i, argv[i - 1]);
            enterDict.Add(0, "Exit");
            foreach (KeyValuePair<int, string> keyValue in enterDict)
            {
                Console.WriteLine(keyValue.Key + " - " + keyValue.Value);
            }     
            Console.Write("Choose your move: ");
            int userM = Convert.ToInt32(Console.ReadLine());
            string yourMove = enterDict[userM];
            Console.WriteLine("Your move: " + yourMove);
            if (yourMove.Equals("Exit")) return;
            Console.WriteLine("Bot move: " + botMove);
            whoWins(createDict(argv), yourMove, botMove);
            Console.WriteLine(key);
        }

        static bool isCorrect(string[] moves)
        {
            int l = moves.Length;
            if (((l % 2) == 0) || (l <= 1))
            {
                Console.WriteLine("You entered wrong number of moves! Try again.");
                return false;
            }
            if (moves.Distinct().Count() != l)
            {
                Console.WriteLine("You entered equals moves! Try again.");
                return false;
            }
            return true;
        }

        static string HMACHASH(string str, string key)
        {
            byte[] bkey = Encoding.Default.GetBytes(key);
            HMACSHA256 hmac = new HMACSHA256(bkey);
            byte[] bstr = Encoding.Default.GetBytes(str);
            byte[] bhash = hmac.ComputeHash(bstr);
            return BitConverter.ToString(bhash).Replace("-", string.Empty);
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
        static Dictionary<string, string> createDict(string[] par)
        {
            string[] words = par.Concat(par).ToArray();
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
