using System;
using Newtonsoft.Json;
using Chilkat;
[Serializable]
public class Transaction : ICloneable{
    public DateTime Timestamp{get;}
    public String From{get;}
    public String To{get;}
    public double Amount{get;}
    public String Hash{get; private set;}
    public String Signature{get; private set;}
    
    public Transaction(String from, String to, double amount){
        this.Timestamp = DateTime.Now;
        this.From = from;
        this.To = to;
        this.Amount = amount;
        this.CreateHash();
    }

    public Boolean IsTransactionValid(double senderBalance){
        Boolean result = true;
        if(!Crypto.ValidateSignature(this.Signature, this.Hash, this.From)){    
            result = false;
            Console.WriteLine("Transaction " + this.Hash + " does not have a valid signature.");
        }
        if(this.Amount > senderBalance){
            result = false;
            Console.WriteLine("Transaction " + this.Hash + " is not valid due to insufficient funds.");
        }
        return result;
    }

    public void AddSignature(String signature){
        this.Signature = signature;
    }

    private void CreateHash(){
        this.Hash = Crypto.CreateHash(Timestamp+From+To+Amount);
    }

    public override String ToString(){
        String output = "\t\t timestamp: " + this.Timestamp + "\n\t\t From: " + this.From + "\n\t\t To: " + this.To + "\n\t\t Amount: " + this.Amount + "\n\t\t Signature: " + this.Signature + "\n\t";
        return output;
    }

    public object Clone(){
        return (Transaction)this.MemberwiseClone();
    }
}