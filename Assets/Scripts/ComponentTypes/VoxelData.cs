using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Burst;
using static Unity.Mathematics.math;

using Unity.Mathematics;

public struct VoxelData : IBufferElementData
{
    public float value;
}


public struct ChunkStatus : IComponentData
{
    public int SizeX;
    public int SizeY;
    public int SizeZ;
}

public struct shouldGenerateIntialvoxelData : IComponentData
{


}





