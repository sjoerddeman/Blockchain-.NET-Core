using System;
using Newtonsoft.Json;
public class Wallet{
    private String PrivateKey;
    public String PublicKey{get;}
    public Wallet(String publicKey, String privateKey){
        PublicKey = publicKey;
        PrivateKey = privateKey;
    }

    public void SignTransAction(Transaction tx){
        tx.AddSignature(Crypto.SignTransaction(tx.Hash, this.PrivateKey));
    }
}