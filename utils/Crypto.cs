using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
public static class Crypto{
    public static string createHash(string input){
        var encData = Encoding.UTF8.GetBytes(input);
        Sha256Digest sha256 = new Sha256Digest();
        sha256.BlockUpdate(encData, 0, encData.Length);
        byte[] hash = new byte[sha256.GetDigestSize()];
        sha256.DoFinal(hash, 0);
        String hashString = BitConverter.ToString(hash).Replace("-","").ToLower();
        return hashString;
    }

    public static Wallet createWallet(){ 
        RsaKeyPairGenerator rsaKeyPairGnr = new RsaKeyPairGenerator(); 
        rsaKeyPairGnr.Init(new KeyGenerationParameters(new SecureRandom(), 512)); 
        AsymmetricCipherKeyPair keyPair = rsaKeyPairGnr.GenerateKeyPair();   

        byte[] serializedPrivateBytes = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private).GetDerEncoded();
        string serializedPrivateString = Convert.ToBase64String(serializedPrivateBytes);

        byte[] serializedPublicBytes = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public).GetDerEncoded();
        string serializedPublicString = Convert.ToBase64String(serializedPublicBytes);

        return new Wallet(serializedPublicString, serializedPrivateString);
    }
    
    public static string signTransaction(string hash, string privateKey){
        byte[] serializedPrivateBytes = Convert.FromBase64String(privateKey); 
        AsymmetricKeyParameter key = PrivateKeyFactory.CreateKey(serializedPrivateBytes);

        ISigner signer = SignerUtilities.GetSigner("SHA1withRSA");
        signer.Init(true, key);   
        
        byte[] hashBytes = Encoding.UTF8.GetBytes(hash);
        
        signer.BlockUpdate(hashBytes, 0, hashBytes.Length);
        
        byte[] sig = signer.GenerateSignature();
        String signature = Convert.ToBase64String(sig);

        return signature;         
    }
    public static Boolean validateSignature(string sig, string hash, string publicKey){
        Console.WriteLine(publicKey);
        byte[] serializedPublicBytes = Convert.FromBase64String(publicKey); 
        AsymmetricKeyParameter key = PublicKeyFactory.CreateKey(serializedPublicBytes);

        ISigner signer = SignerUtilities.GetSigner("SHA1withRSA");
        signer.Init(false, key);   
        
        byte[] hashBytes = Encoding.UTF8.GetBytes(hash);
        
        signer.BlockUpdate(hashBytes, 0, hashBytes.Length);
        
        return signer.VerifySignature(Convert.FromBase64String(sig));
    }
}