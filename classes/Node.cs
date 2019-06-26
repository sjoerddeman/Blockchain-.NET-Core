using System;
using System.Collections.Generic;

public abstract class Node{
    public List<Block> Blocks {get;set;}
    public String Name{get;set;}
    protected Wallet Wallet = Crypto.createWallet();
    public const int Difficulty = 2;
    public const double initCoins = 25;
    public Node(String name){
        this.Name = name;
    }

    public void addBlock(Block block){
        if(isBlockValid(block)){
            this.Blocks.Add(block);
        }        
    }
    public Boolean isBlockValid(Block block){
        if(block is GenesisBlock){
            return true;
        }
        Boolean result = true;
        for(int i = 0; i<block.Data.Count; i++){
            Transaction tx = block.Data[i];
            double senderBalance = getBalance(tx.From, block.Index-1);
            if(!tx.isTransactionValid(senderBalance)){
                result = false;
                Console.WriteLine("Block " + block.Index + " contains invalid transactions");
            }
        }
        if(block.Hash != block.createBlockHash()){
            result  = false;
            Console.WriteLine("Block " + block.Index + " doesn't have a valid hash");
        }
        if(block.PreviousHash != Blocks[block.Index-1].Hash){
            result = false;
            Console.WriteLine("This Block doesn't use the correct previous hash");
        }
        return result;
    }
    public Boolean isChainValid(){
        Boolean result = true;
        for(int i = 0; i < Blocks.Count; i++){
            if(!this.isBlockValid(Blocks[i])){
                result = false;
            }
        }
        return result;
    }

    public int currentBlockIndex(){
        return this.Blocks.Count-1;
    }

    public String getWalletAddress(){
        return this.Wallet.PublicKey;
    }

    public double getBalance(String publicKey, int blockIndex=-1){
        double txOut = 0;
        double txIn = 0;
        if(publicKey == "0"){txIn = Node.initCoins;}
        if(blockIndex==-1) blockIndex = this.currentBlockIndex();
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
    public void sendTransaction(String to, double amount){
        Transaction tx = new Transaction(this.Wallet.PublicKey, to, amount);
        this.Wallet.signTransAction(tx);
        Channel.sendTransaction(tx);
    }

    public override String ToString(){
        String output = "Chain :\n";
        for(int i = 0; i <this.Blocks.Count; i++){
            output += Blocks[i] +"\n";
        }
        return output;
    }
}