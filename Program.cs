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
            new Global().UnlockBundle("Anything for 30-day trial");


            MasterNode mn = new MasterNode("Master Node");
            ClientNode cn1 = new ClientNode("Client Node 1"); 
            ClientNode cn2 = new ClientNode("Client Node 2"); 
            ClientNode cn3 = new ClientNode("Client Node 3");           

            mn.sendTransaction(cn1.getWalletAddress(), 5);
            mn.sendTransaction(cn2.getWalletAddress(), 10);
            mn.sendTransaction(cn3.getWalletAddress(), 5);

            mn.mineBlock();

            cn2.sendTransaction(cn1.getWalletAddress(), 5);
            cn3.sendTransaction(mn.getWalletAddress(), 10);

            mn.mineBlock();

            ClientNode cn4 = new ClientNode("Client Node 4");
            
            cn2.sendTransaction(cn4.getWalletAddress(), 5);
            cn3.sendTransaction(mn.getWalletAddress(), 5);
            
            mn.mineBlock();

            Console.WriteLine("The chain is valid: " + mn.isChainValid());

            Console.WriteLine(mn.Name+ " has "+mn.Blocks.Count+" blocks");
            Console.WriteLine(cn1.Name+" has "+cn1.Blocks.Count+" blocks");
            Console.WriteLine(cn2.Name+" has "+cn2.Blocks.Count+" blocks");
            Console.WriteLine(cn3.Name+" has "+cn3.Blocks.Count+" blocks");
            Console.WriteLine(cn4.Name+" has "+cn4.Blocks.Count+" blocks");            

            //Console.WriteLine("Block 1: \n "+mn.Blocks[1]+"\n");

            Console.WriteLine("Masternode has: "+mn.getBalance(mn.getWalletAddress()));
            Console.WriteLine(cn1.Name+" has: "+cn1.getBalance(cn1.getWalletAddress()));
            Console.WriteLine(cn2.Name+" has: "+cn2.getBalance(cn2.getWalletAddress()));
            Console.WriteLine(cn3.Name+" has: "+cn3.getBalance(cn3.getWalletAddress()));
            Console.WriteLine(cn4.Name+" has: "+cn4.getBalance(cn4.getWalletAddress()));            
        }
    }
}
