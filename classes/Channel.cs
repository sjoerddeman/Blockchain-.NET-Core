using System;

using System.Collections.Generic;

public class Channel {
    public List<Block> minedBlocks;
    public List<Transaction> TransactionPool;
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
                instance.minedBlocks = new List<Block>();
                instance.TransactionPool = new List<Transaction>(); 
                instance.Nodes = new List<Node>(); 
                instance.MasterNodes = new List<MasterNode>(); 
            }
        return instance;
        }
    }

    public static void registerNode(ClientNode node){
        if(!Instance.Nodes.Contains(node)){
            Instance.Nodes.Add(node);
        }
    }
    public static void getCurrentChain(ClientNode node){
        if(Instance.MasterNodes.Count!=0){
            foreach(Block block in Instance.MasterNodes[0].Blocks){
                node.addBlock(block.Clone());
            }
        }
    }
    public static void registerMasterNode(MasterNode node){
        Instance.MasterNodes.Add(node);
        Instance.Nodes.Add(node);
    }
     public static void sendTransaction(Transaction tx){
        foreach(MasterNode mn in Instance.MasterNodes){
            mn.addNewTransaction((Transaction)tx.Clone());
        }
     }     
     public static void addBlock(Block block){
        foreach(Node node in Instance.Nodes){
            node.addBlock((Block)block.Clone());
        }
     }
}