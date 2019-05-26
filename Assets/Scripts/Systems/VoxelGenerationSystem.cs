using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using Unity.Transforms;

using static Unity.Mathematics.math;
using Unity.Mathematics;

public class VoxelGenerationSystem : JobComponentSystem
{
    EntityQuery eq;
    BufferFromEntity<VoxelData> voxelData;

    protected override void OnCreate()
    {
        eq = World.Active.EntityManager.CreateEntityQuery(typeof(shouldGenerateIntialvoxelData));
        //voxelData = GetBufferFromEntity<VoxelData>(false);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        //eq = World.Active.EntityManager.CreateEntityQuery(typeof(VoxelData));

        NativeArray<Entity> entities = eq.ToEntityArray(Allocator.TempJob);

        if(entities.Length == 0)
        {
            return inputDeps;
        }

        int SizeX = World.Active.EntityManager.GetComponentData<ChunkStatus>(entities[0]).SizeX;
        int SizeY = World.Active.EntityManager.GetComponentData<ChunkStatus>(entities[0]).SizeY;
        int SizeZ = World.Active.EntityManager.GetComponentData<ChunkStatus>(entities[0]).SizeY;

        World.Active.EntityManager.AddComponent(entities[0], typeof(shouldGenerateMesh));

        World.Active.EntityManager.RemoveComponent(entities[0], typeof(shouldGenerateIntialvoxelData));

        VoxelGenerationJob VGJob = new VoxelGenerationJob
        {
            SizeX = SizeX,
            SizeY = SizeY,
            voxelData = GetBufferFromEntity<VoxelData>(false),
            entity = entities[0],
            entities = entities,
            initPos = World.Active.EntityManager.GetComponentData<Translation>(entities[0]).Value
        };
        JobHandle VGHandle = VGJob.Schedule(SizeX * SizeY * SizeZ, 1, inputDeps);
        //JobHandle[] voxelGenerationJobs = new JobHandle[entities.Length];
        
        //VGHandle.Complete();

        //entities.Dispose();

        //throw new System.Exception();
        return VGHandle;
    }


    






}