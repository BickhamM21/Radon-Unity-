using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Entities;
using static Unity.Mathematics.math;

using Unity.Mathematics;

[BurstCompile]
public struct MeshDataCompactionJob : IJobParallelForFilter
{

    [NativeDisableParallelForRestriction]
    public BufferFromEntity<UnfilteredVerticesArray> unFilteredVerticesData;


    public NativeArray<Entity> entities;


    public bool Execute(int i)
    {

        DynamicBuffer<UnfilteredVerticesArray> UnfilteredVerticesArray = unFilteredVerticesData[entities[0]];
        return UnfilteredVerticesArray[i].value.w == 1;
        
    }
}

[BurstCompile]
public struct BuildComapctionArrayJob : IJobParallelFor
{

    [NativeDisableParallelForRestriction]
    public BufferFromEntity<UnfilteredVerticesArray> unFilteredVerticesData;

    [NativeDisableParallelForRestriction]
    public BufferFromEntity<filteredVerticesArray> filteredVerticesData;

    //[DeallocateOnJobCompletion]
    [NativeDisableParallelForRestriction]
    public NativeArray<Entity> entities;



    [ReadOnly]
    [NativeDisableParallelForRestriction]
    public NativeArray<int> FilteredIndices;

    public void Execute(int index)
    {

        DynamicBuffer<UnfilteredVerticesArray> UnfilteredVerticesArray = unFilteredVerticesData[entities[0]];
        DynamicBuffer<filteredVerticesArray> FilteredVerticesArray = filteredVerticesData[entities[0]];

        filteredVerticesArray value = FilteredVerticesArray[index];
        value.value = UnfilteredVerticesArray[FilteredIndices[index]].value.xyz;

        FilteredVerticesArray[index] = value;
    }


}