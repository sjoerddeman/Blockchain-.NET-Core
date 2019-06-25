using System;
using System.Collections.Generic;

public class MasterNode : Node{

    public List<Transaction> openTransactions = new List<Transaction>();
    public MasterNode(String name):base(name){
            Channel.registerMasterNode(this);
            this.Blocks = new List<Block>();
            createGenesisBlock();
    }
    public MasterNode(List<Block> blocks, String name) : base(name){
        if(blocks != null || blocks.Count != 0){
            Blocks = blocks;
        }else{
            throw new Exception();
        }

    }

    public void addNewTransaction(Transaction tx){
        openTransactions.Add(tx);
    }
    public int mineBlock(){
        Block previousBlock = Blocks[Blocks.Count-1];
        Block block = new Block(previousBlock.Index+1);
        block.PreviousHash = previousBlock.Hash;

        foreach(Transaction tx in openTransactions){
            if(tx.isTransactionValid(getBalance(tx.From))){
                block.addTransaction(tx);
            }else{
                Console.WriteLine(this.Name+" doesn't add "+tx.Hash+" to block "+block.Index);
            }
        }
        openTransactions = new List<Transaction>();
        String dif = "";
        for(int i = 0; i<Node.Difficulty;i++){
            dif += "0";
        }
        Console.WriteLine("Mining of block "+ block.Index+ " is started.");
        while(!block.Hash.StartsWith(dif)){
            block.Hash = "";
            block.Nonce++;
            String hashString = block.Index+block.Nonce+block.Timestamp.ToString()+block.createTransactionDataHash()+block.PreviousHash;
            block.Hash = Crypto.createHash(hashString);
        }
        Channel.addBlock(block);
        Console.WriteLine("Block "+ block.Index + " is mined with nonce " + block.Nonce + ".");
        return block.Nonce;
    }

    private void createGenesisBlock(){
        if(this.Blocks.Count == 0){
            GenesisBlock genesisBlock = new GenesisBlock();
            Transaction iTx1 = new Transaction("0", this.Wallet.PublicKey, Node.initCoins);

            genesisBlock.addTransaction(iTx1);

            Channel.addBlock(genesisBlock);
        }
    }
}