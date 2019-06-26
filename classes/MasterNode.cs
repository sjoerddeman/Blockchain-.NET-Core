using System;
using System.Collections.Generic;

public class MasterNode : Node{

    public List<Transaction> OpenTransactions = new List<Transaction>();

    public MasterNode(String name):base(name){
            Channel.RegisterMasterNode(this);
            this.Blocks = new List<Block>();
            CreateGenesisBlock();
    }
    public MasterNode(List<Block> blocks, String name) : base(name){
        if(blocks != null || blocks.Count != 0){
            Blocks = blocks;
        }else{
            throw new Exception();
        }

    }

    public void AddNewTransaction(Transaction tx){
        OpenTransactions.Add(tx);
    }

    public void StartMining(){
        MineNewBlock(AddTransactionsToNewBlock());
    }
    public Block AddTransactionsToNewBlock(){
        Block previousBlock = Blocks[Blocks.Count-1];
        Block block = new Block(previousBlock.Index+1);
        block.PreviousHash = previousBlock.Hash;

        foreach(Transaction tx in OpenTransactions){
            if(tx.IsTransactionValid(GetWalletBalance(tx.From))){
                block.AddTransaction(tx);
            }else{
                Console.WriteLine(this.Name+" doesn't add "+tx.Hash+" to block "+block.Index);
            }
        }
        OpenTransactions = new List<Transaction>();
        return block;
    }
    public void MineNewBlock(Block block){
        String dif = "";
        for(int i = 0; i<Node.Difficulty;i++){
            dif += "0";
        }
        Console.WriteLine("Mining of block "+ block.Index+ " is started.");
        while(!block.Hash.StartsWith(dif)){
            block.Hash = "";
            block.Nonce++;
            String hashString = block.Index+block.Nonce+block.Timestamp.ToString()+block.CreateTransactionDataHash()+block.PreviousHash;
            block.Hash = Crypto.CreateHash(hashString);
        }
        Channel.AddNewBlock(block);
        Console.WriteLine("Block "+ block.Index + " is mined with nonce " + block.Nonce + ".");
    }

    private void CreateGenesisBlock(){
        if(this.Blocks.Count == 0){
            GenesisBlock genesisBlock = new GenesisBlock();
            Transaction iTx1 = new Transaction("0", this.Wallet.PublicKey, Node.InitCoins);

            genesisBlock.AddTransaction(iTx1);

            MineNewBlock(genesisBlock);
        }
    }
}