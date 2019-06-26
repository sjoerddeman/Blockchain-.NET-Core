using System;

using System.Collections.Generic;

public class Channel {
    public List<Node> Nodes;
    public List<MasterNode> MasterNodes;
    private static Channel instance;
    private Channel() {}
    static Channel(){
      
    }
    public static Channel Instance{
        get{
            if(instance == null){
                instance = new Channel();
                instance.Nodes = new List<Node>(); 
                instance.MasterNodes = new List<MasterNode>(); 
            }
        return instance;
        }
    }

    public static void RegisterNode(ClientNode node){
        if(!Instance.Nodes.Contains(node)){
            Instance.Nodes.Add(node);
        }
    }

    public static void RegisterMasterNode(MasterNode node){
        if(!Instance.MasterNodes.Contains(node)){
            Instance.MasterNodes.Add(node);
        }
        if(!Instance.Nodes.Contains(node)){
            Instance.Nodes.Add(node);
        }
    }
    public static void GetCurrentChain(ClientNode node){
        if(Instance.MasterNodes.Count!=0){
            foreach(Block block in Instance.MasterNodes[0].Blocks){
                node.AddBlock(block.Clone());
            }
        }
    }

     public static void AddNewTransaction(Transaction tx){
        foreach(MasterNode mn in Instance.MasterNodes){
            mn.AddNewTransaction((Transaction)tx.Clone());
        }
     }     
     public static void AddNewBlock(Block block){
        foreach(Node node in Instance.Nodes){
            node.AddBlock((Block)block.Clone());
        }
     }
}