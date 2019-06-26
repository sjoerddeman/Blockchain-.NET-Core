using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Chilkat;
[Serializable]
public class GenesisBlock : Block {

    public GenesisBlock(): base(0){
        this.Hash = "";
        this.PreviousHash = "0";
    }

}