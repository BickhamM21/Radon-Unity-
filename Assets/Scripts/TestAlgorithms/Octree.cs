using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;


using static Unity.Mathematics.math;
using Unity.Mathematics;

public struct Octree
{
    public NativeHashMap<int, OctreeNode> octreeNodes;
    public int Size;
    public OctreeNode rootNode;
    public int LODlevel;
    public OctreeNode GetParentNode(OctreeNode node)
    {
        int locCodeParent = node.LocCode >> 3;
        return LookupNode(locCodeParent);

    }

    public OctreeNode TryGetValue(int LocCode) {
        OctreeNode node;
        if(!octreeNodes.TryGetValue(LocCode, out node))
        {
            Debug.Log("ERROR: Trying to retrieve octreeNode who's location code doesnt exist!");

        }
        return node;
    }


    public int GetParentLocCode(OctreeNode node)
    {

        return node.LocCode >> 3;
    }

    public OctreeNode LookupNode(int LocCode)
    {
        OctreeNode node;

        if(!octreeNodes.TryGetValue(LocCode, out node))
        {
            Debug.Log("You done oofed dude, the octree node could not be found with the LocCode provided, please try again or stay oofed");
        }



        return node;

    }

    public int GetNodeTreeDepth(OctreeNode node)         
    {

        
        return (int)floor(log2(node.LocCode)/3);


    }

    public int GetNodeTreeDepth(int LocCode)
    {


        return (int)floor(log2(LocCode) / 3);


    }

    public void createRootNode()
    {
        rootNode = new OctreeNode { LocCode = 1, Size = Size};
        octreeNodes.TryAdd(1, rootNode);
    }

    public void createOctreeNodes(OctreeNode node)
    {
        if(node.Size == LODlevel)
        {
            return;
        }

        int childSize = node.Size / 2;

        for(int i = 0; i < 8; i++)
        {
            OctreeNode childNode = new OctreeNode { LocCode = ((node.LocCode << 3) + i), Size = childSize};

            octreeNodes.TryAdd(childNode.LocCode, childNode);

            createOctreeNodes(childNode);

        }



    }

    


}

public struct OctreeNode
{
    public int LocCode;
    public int Size;

}
