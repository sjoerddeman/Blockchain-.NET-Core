using System;
using Chilkat;
public static class Crypto{
    public static string createHash(string input){
        Crypt2 crypt = new Crypt2();
        crypt.HashAlgorithm = "sha256";
        return crypt.HashStringENC(input);
    }

    public static string signTransaction(string hash, string privateKey){
        Rsa rsa = new Rsa();
        rsa.ImportPrivateKey(privateKey);

        return rsa.EncryptStringENC(hash, true);         
    }
    public static Boolean validateSignature(string sig, string hash, string publicKey){
        Rsa rsa = new Rsa();
        rsa.ImportPublicKey(publicKey);

        String signedHash = rsa.DecryptStringENC(sig, false);
        return hash == signedHash;    

    }
}