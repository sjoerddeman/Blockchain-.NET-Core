using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Chilkat;
[Serializable]
public class Block {
    public int Index {get;}
    public int Nonce{get; set;}
    public DateTime Timestamp{get;}
    public List<Transaction> Data{get;}
    public String PreviousHash{get; set;}
    public String Hash {get; set;}

    public Block(int index){
        this.Index = index;
        this.Nonce = 0;
        this.Timestamp = DateTime.Now;
        this.Data = new List<Transaction>();
        this.Hash = "";
    }

    public virtual void AddTransaction(Transaction tx){
        Data.Add(tx);
        Console.WriteLine("Transaction " + tx.Hash + " is added to block " + this.Index);
    }
    
    public String CreateTransactionDataHash(){
        String txHash = "";
        for(int i = 0; i<Data.Count; i++){
            txHash += Data[i].Hash;
        }
        return Crypto.CreateHash(txHash);
    }

    public String CreateBlockHash(){
        return Crypto.CreateHash(this.Index+this.Nonce+this.Timestamp.ToString()+this.CreateTransactionDataHash()+this.PreviousHash);
    }

    public Block Clone(){  
        MemoryStream ms = new MemoryStream();  
        BinaryFormatter bf = new BinaryFormatter();  
        bf.Serialize(ms, this);  
        ms.Position = 0;  
        object result = bf.Deserialize(ms);  
        ms.Close();  
        return (Block)result;    
    }

    public override String ToString(){
        String output = "\t index: " + this.Index + "\n\t nonce: " + this.Nonce + "\n\t timestamp: " + this.Timestamp + "\n\t previousHash: " + this.PreviousHash + "\n\t hash: " + this.Hash + "\n\t Transactions: \n";
        for(int i = 0; i <this.Data.Count; i++){
            output += Data[i] +"\n";
        }
        return output;
    }
    
}