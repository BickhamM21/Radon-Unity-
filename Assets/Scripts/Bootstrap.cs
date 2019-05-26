

using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using System;
using Unity.Rendering;
using Unity.Transforms;

using static Unity.Mathematics.math;

using Unity.Mathematics;

public class Bootstrap : MonoBehaviour
{
    EntityManager entityManager;
    Entity chunkEntity0;
    NativeArray<Entity> chunkEntities;
    private void Start()
    {

        entityManager = World.Active.EntityManager;


        var newChunkArchetype = entityManager.CreateArchetype(typeof(VoxelData),
                                                              typeof(ChunkStatus),
                                                              typeof(shouldGenerateIntialvoxelData),
                                                              typeof(UnfilteredVerticesArray),
                                                              typeof(filteredVerticesArray),
                                                              typeof(indicesArray),
                                                              typeof(normalArray),
                                                              typeof(RenderMesh),
                                                              typeof(LocalToWorld),
                                                              typeof(Translation),
                                                              typeof(Scale));

        



        chunkEntities = new NativeArray<Entity>(4 * 4 * 4, Allocator.Persistent);

        entityManager.CreateEntity(newChunkArchetype, chunkEntities);

        for(int i = 0; i < chunkEntities.Length; i++)
        {
            int z = i / ((4) * (4));
            int b = i - (((4) * (4)) * z);
            int y = b / (4);
            int x = b % (4);


            DynamicBuffer<VoxelData> voxelBuffer = entityManager.GetBuffer<VoxelData>(chunkEntities[i]);
            voxelBuffer.ResizeUninitialized(17 * 17 * 17);

            DynamicBuffer<UnfilteredVerticesArray> unfilteredVerticesBuffer = entityManager.GetBuffer<UnfilteredVerticesArray>(chunkEntities[i]);
            unfilteredVerticesBuffer.ResizeUninitialized(16 * 16 * 16 * 15);

            ChunkStatus initChunkStatus = new ChunkStatus
            {

                SizeX = 17,
                SizeY = 17,
                SizeZ = 17

            };

            Translation initialPosition = new Translation
            {
                Value = float3(x * 16, y * 16, z * 16)
            };

            Scale scale = new Scale
            {
                Value = 1
            };

            
            entityManager.SetComponentData(chunkEntities[i], initChunkStatus);
            entityManager.SetComponentData(chunkEntities[i], initialPosition);
            entityManager.SetComponentData(chunkEntities[i], scale);

        }


        /*for(int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int z = 0; z < 8; z++)
                {
                    DynamicBuffer<VoxelData> voxelBuffer = entityManager.GetBuffer<VoxelData>(chunkEntities[x * y * z]);
                    voxelBuffer.ResizeUninitialized(17 * 17 * 17);

                    DynamicBuffer<UnfilteredVerticesArray> unfilteredVerticesBuffer = entityManager.GetBuffer<UnfilteredVerticesArray>(chunkEntities[x * y * z]);
                    unfilteredVerticesBuffer.ResizeUninitialized(16 * 16 * 16 * 15);

                    ChunkStatus initChunkStatus = new ChunkStatus
                    {

                        SizeX = 17,
                        SizeY = 17,
                        SizeZ = 17

                    };

                    Translation initialPosition = new Translation
                    {
                        Value = float3(0, 0, 0)
                    };

                    Scale scale = new Scale
                    {
                        Value = 1
                    };


                    entityManager.SetComponentData(chunkEntities[x * y * z], initChunkStatus);
                    entityManager.SetComponentData(chunkEntities[x * y * z], initialPosition);
                    entityManager.SetComponentData(chunkEntities[x * y * z], scale);
                }
            }
        }*/


    }

    public void createChunkEntityBuffers(ref Entity chunkEntity)
    {

        DynamicBuffer<VoxelData> voxelBuffer = entityManager.GetBuffer<VoxelData>(chunkEntity);
        voxelBuffer.ResizeUninitialized(17 * 17 * 17);

        DynamicBuffer<UnfilteredVerticesArray> UnfilteredVerticesBuffer = entityManager.GetBuffer<UnfilteredVerticesArray>(chunkEntity);
        UnfilteredVerticesBuffer.ResizeUninitialized(16 * 16 * 16 * 15);

        
    }

    private void Update()
    {
        //DynamicBuffer<VoxelData> buffer = entityManager.GetBuffer<VoxelData>(chunkEntity);
        //Debug.Log(buffer[4913‬].value);
        //Debug.Log(buffer[4912].value);
    }

}
