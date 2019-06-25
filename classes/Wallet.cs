using System;
using Chilkat;
using Newtonsoft.Json;
public class Wallet{
    private String PrivateKey;
    public String PublicKey{get;}
    public Wallet(){

        Rsa rsa = new Rsa();
        rsa.GenerateKey(1024);

        this.PublicKey = rsa.ExportPublicKey();
        this.PrivateKey = rsa.ExportPrivateKey();
    }

    public void signTransAction(Transaction tx){
        tx.addSignature(Crypto.signTransaction(tx.Hash, this.PrivateKey));
    }
}