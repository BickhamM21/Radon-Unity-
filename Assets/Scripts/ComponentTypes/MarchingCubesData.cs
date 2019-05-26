using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public struct UnfilteredVerticesArray : IBufferElementData
{
    public float4 value;
}

public struct filteredVerticesArray : IBufferElementData
{

    public Vector3 value;

}

public struct normalArray : IBufferElementData
{

    public Vector3 value;

}

public struct indicesArray : IBufferElementData
{
    public int value;
}

public struct shouldGenerateMesh : IComponentData
{
    
}

public struct shouldFilterMesh : IComponentData
{

}

public struct shouldGenerateNormals : IComponentData
{


}

public struct shouldRender : IComponentData
{


}

public struct completeChunk : IComponentData
{

}