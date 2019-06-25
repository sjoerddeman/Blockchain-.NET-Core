using System;
using System.Collections.Generic;

public class ClientNode : Node{

    public ClientNode(String name):base(name){
        Channel.registerNode(this);
        this.Blocks = new List<Block>();
        Channel.getCurrentChain(this);
    }


}