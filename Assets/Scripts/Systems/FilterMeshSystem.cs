using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;

[UpdateAfter(typeof(MarchingCubesSystem))]
public class FilterMeshSystem : JobComponentSystem
{

    EntityQuery eq;
    NativeList<int> filteredIndices = new NativeList<int>(Allocator.Persistent);

    

    protected override void OnCreate()
    {

        eq = World.Active.EntityManager.CreateEntityQuery(typeof(shouldFilterMesh));

        filteredIndices = new NativeList<int>(Allocator.Persistent);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        NativeArray<Entity> entities = eq.ToEntityArray(Allocator.TempJob);


        if (entities.Length == 0)
        {
            return inputDeps;
        }

        World.Active.EntityManager.AddComponent(entities[0], typeof(shouldGenerateNormals));

        World.Active.EntityManager.RemoveComponent(entities[0], typeof(shouldFilterMesh));

        filteredIndices.Clear();


        int SizeX = World.Active.EntityManager.GetComponentData<ChunkStatus>(entities[0]).SizeX;
        int SizeY = World.Active.EntityManager.GetComponentData<ChunkStatus>(entities[0]).SizeY;
        int SizeZ = World.Active.EntityManager.GetComponentData<ChunkStatus>(entities[0]).SizeY;

       

        MeshDataCompactionJob filterHandle = new MeshDataCompactionJob
        {
            unFilteredVerticesData = GetBufferFromEntity<UnfilteredVerticesArray>(false),
            entities = entities
        };

        JobHandle MCJ = filterHandle.ScheduleAppend(filteredIndices, (SizeX - 1) * (SizeY - 1) * (SizeZ - 1) * 15, 1, inputDeps);

        MCJ.Complete();
        //Debug.Log(filteredIndices.Length);

        GetBufferFromEntity<filteredVerticesArray>(false)[entities[0]].ResizeUninitialized(filteredIndices.Length);
        GetBufferFromEntity<normalArray>(false)[entities[0]].ResizeUninitialized(filteredIndices.Length);
        GetBufferFromEntity<indicesArray>(false)[entities[0]].ResizeUninitialized(filteredIndices.Length);


        //Debug.Log(filteredIndices.Length);

        //BuildComapctionArrayJob filteredVertices = new NativeArray<float4>(filteredIndices.Length, Allocator.TempJob);
        BuildComapctionArrayJob buildHandle = new BuildComapctionArrayJob
        {
            unFilteredVerticesData = GetBufferFromEntity<UnfilteredVerticesArray>(false),
            FilteredIndices = filteredIndices,
            filteredVerticesData = GetBufferFromEntity<filteredVerticesArray>(false),
            entities = entities
        };

        JobHandle BCA = buildHandle.Schedule(filteredIndices.Length, 1);
        //BCA.Complete();

        



        return BCA;
    }



}
