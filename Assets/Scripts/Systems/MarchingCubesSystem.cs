using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

[UpdateAfter(typeof(VoxelGenerationSystem))] 
public class MarchingCubesSystem : JobComponentSystem
{

    EntityQuery eq;

    protected override void OnCreate()
    {

        eq = World.Active.EntityManager.CreateEntityQuery(typeof(shouldGenerateMesh));


    }



    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        //throw new System.NotImplementedException();
        NativeArray<Entity> entities = eq.ToEntityArray(Allocator.TempJob);







        if (entities.Length == 0)
        {
            return inputDeps;
        }

        World.Active.EntityManager.AddComponent(entities[0], typeof(shouldFilterMesh));

        World.Active.EntityManager.RemoveComponent(entities[0], typeof(shouldGenerateMesh));


        int SizeX = World.Active.EntityManager.GetComponentData<ChunkStatus>(entities[0]).SizeX;
        int SizeY = World.Active.EntityManager.GetComponentData<ChunkStatus>(entities[0]).SizeY;
        int SizeZ = World.Active.EntityManager.GetComponentData<ChunkStatus>(entities[0]).SizeZ;


        NativeArray<float3> cornerMemoryPool = new NativeArray<float3>(SizeX * SizeY * SizeZ * 8, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        NativeArray<float> gridMemoryPool = new NativeArray<float>(SizeX * SizeY * SizeZ * 8, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        NativeArray<float3> vertMemoryPool = new NativeArray<float3>(SizeX * SizeY * SizeZ * 12, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);



        NativeArray<int> edgeTable = new NativeArray<int>(256, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
        NativeArray<int> triTable = new NativeArray<int>(4096, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

        edgeTable.CopyFrom(MCTables.CubeEdgeFlags);
        triTable.CopyFrom(MCTables.triTable);




        MarchingCubesJob MCJob = new MarchingCubesJob
        {
            SizeX = SizeX ,
            SizeY = SizeY ,
            GridSize = 1f,
            edgeTable = edgeTable,
            triTable = triTable,
            vertListSlicePool = vertMemoryPool,
            cornerSlicePool = cornerMemoryPool,
            gridSlicePool = gridMemoryPool,
            voxelData = GetBufferFromEntity<VoxelData>(false),
            unfilteredVerticesData = GetBufferFromEntity<UnfilteredVerticesArray>(false),
            entities = entities,
            entity = entities[0]
        };

        JobHandle MCHandle = MCJob.Schedule((SizeX - 1) * (SizeY - 1) * (SizeZ - 1), 1, inputDeps);


        return MCHandle;
    }
}
