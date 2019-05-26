using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

public class OctreeTest : MonoBehaviour
{
    // Start is called before the first frame update
    public Octree octree;
    void Start()
    {

        octree = new Octree
        {

            octreeNodes = new NativeHashMap<int, OctreeNode>(5000, Allocator.Persistent),
            Size = 8,
            LODlevel = 2

        };

        octree.createRootNode();

        octree.createOctreeNodes(octree.rootNode);

        
        Debug.Log(octree.TryGetValue(127).Size);

        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
