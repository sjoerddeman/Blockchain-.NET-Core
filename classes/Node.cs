using System;
using System.Collections.Generic;

public abstract class Node{
    public List<Block> Blocks {get;set;}
    public String Name{get;set;}
    protected Wallet Wallet = Crypto.CreateWallet();
    public const int Difficulty = 2;
    public const double InitCoins = 25;
    public Node(String name){
        this.Name = name;
    }

    public void AddBlock(Block block){
        if(IsBlockValid(block) && block.Index==this.Blocks.Count){
            this.Blocks.Add(block);
        }        
    }
    public Boolean IsBlockValid(Block block){
        Boolean result = true;
        if(!(block is GenesisBlock)){
            for(int i = 0; i<block.Data.Count; i++){
                Transaction tx = block.Data[i];
                double senderBalance = GetWalletBalance(tx.From, block.Index-1);
                if(!tx.IsTransactionValid(senderBalance)){
                    result = false;
                    Console.WriteLine("Block " + block.Index + " contains invalid transactions:\n\t");
                }
            }
            if(block.PreviousHash != Blocks[block.Index-1].Hash){
                result = false;
                Console.WriteLine("This Block doesn't use the correct previous hash");
            }
        }
        if(block.Hash != block.CreateBlockHash()){
            result  = false;
            Console.WriteLine("Block " + block.Index + " doesn't have a valid hash");
        }

        return result;
    }
    public Boolean IsChainValid(){
        Boolean result = true;
        for(int i = 0; i < Blocks.Count; i++){
            if(!this.IsBlockValid(Blocks[i])){
                result = false;
            }
        }
        return result;
    }

    public int CurrentBlockIndex(){
        return this.Blocks.Count-1;
    }

    public String GetWalletAddress(){
        return this.Wallet.PublicKey;
    }

    public double GetWalletBalance(String publicKey, int blockIndex=-1){
        double txOut = 0;
        double txIn = 0;
        if(publicKey == "0"){txIn = Node.InitCoins;}
        if(blockIndex==-1) blockIndex = this.CurrentBlockIndex();
        for(int b = 0; b <= blockIndex; b++){
            for(int t = 0; t < this.Blocks[b].Data.Count; t++){
                Transaction tx = this.Blocks[b].Data[t];
                if(tx.From == publicKey){
                    txOut += tx.Amount;
                }else if(tx.To == publicKey){
                    txIn += tx.Amount;
                }
            }
        }
        return txIn - txOut;
    }
    public void SendTransaction(String to, double amount){
        Transaction tx = new Transaction(this.Wallet.PublicKey, to, amount);
        this.Wallet.SignTransAction(tx);
        Channel.AddNewTransaction(tx);
    }

    public override String ToString(){
        String output = "Chain :\n";
        for(int i = 0; i <this.Blocks.Count; i++){
            output += Blocks[i] +"\n";
        }
        return output;
    }
}