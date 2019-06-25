using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Chilkat;
[Serializable]
public class GenesisBlock : Block {

    public GenesisBlock(): base(0){
        this.Hash = "0x0000000000000000000000000000000000000000000000000000000000000000";
        this.PreviousHash = "0";
    }

}