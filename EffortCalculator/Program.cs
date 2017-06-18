﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" # # #   Effort Calculator   # # # ");
            string accessToken = RequestAccessTokenFromUser();

            Console.WriteLine("Drücke 'Enter' um die Anwendung zu beenden!");
            Console.ReadLine();
        }

        private static string RequestAccessTokenFromUser()
        {
            string token = Properties.Settings.Default.AccessToken;

            Console.WriteLine("*** Personal Access Token Eingabe ***");
            Console.Write("Soll der gespeichert Token: {0} verwendet werden? (ja/nein): ", token);
            string retrieveNewKey = Console.ReadLine();

            if (retrieveNewKey.ToLower().Equals("nein"))
            {
                Console.Write("Bitte gib einen neuen Token ein: ");
                token = Console.ReadLine();

                // neuen Wert in den Settings speichern
                Properties.Settings.Default.AccessToken = token;
                Properties.Settings.Default.Save();
            }

            return token;
        }

    }
}
