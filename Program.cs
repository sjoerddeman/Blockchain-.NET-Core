using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Chilkat;
namespace CSBlockChain
{
    class Program
    {
        static void Main(string[] args)
        {
            MasterNode mn = new MasterNode("Master Node");
            ClientNode cn1 = new ClientNode("Client Node 1"); 
            ClientNode cn2 = new ClientNode("Client Node 2"); 
            ClientNode cn3 = new ClientNode("Client Node 3");           

            mn.SendTransaction(cn1.GetWalletAddress(), 5);
            mn.SendTransaction(cn2.GetWalletAddress(), 10);
            mn.SendTransaction(cn3.GetWalletAddress(), 5);

            mn.StartMining();

            cn2.SendTransaction(cn1.GetWalletAddress(), 5);
            cn3.SendTransaction(mn.GetWalletAddress(), 10);

            mn.StartMining();

            ClientNode cn4 = new ClientNode("Client Node 4");
            
            cn2.SendTransaction(cn4.GetWalletAddress(), 5);
            cn3.SendTransaction(mn.GetWalletAddress(), 5);
            
            mn.StartMining();

            Console.WriteLine("The chain is valid: " + mn.IsChainValid());
       
            Console.WriteLine("Masternode has: "+mn.GetWalletBalance(mn.GetWalletAddress())+" coins");
            Console.WriteLine(cn1.Name+" has: "+cn1.GetWalletBalance(cn1.GetWalletAddress())+" coins");
            Console.WriteLine(cn2.Name+" has: "+cn2.GetWalletBalance(cn2.GetWalletAddress())+" coins");
            Console.WriteLine(cn3.Name+" has: "+cn3.GetWalletBalance(cn3.GetWalletAddress())+" coins");
            Console.WriteLine(cn4.Name+" has: "+cn4.GetWalletBalance(cn4.GetWalletAddress())+" coins");   

            Console.WriteLine(mn);         
        }
    }
}
