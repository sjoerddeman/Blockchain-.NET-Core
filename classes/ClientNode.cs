using System;
using System.Collections.Generic;

public class ClientNode : Node{

    public ClientNode(String name):base(name){
        Channel.RegisterNode(this);
        this.Blocks = new List<Block>();
        Channel.GetCurrentChain(this);
    }


}