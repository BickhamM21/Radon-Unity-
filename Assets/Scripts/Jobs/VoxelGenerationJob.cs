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
public struct VoxelGenerationJob : IJobParallelFor
{


    public int SizeX;
    public int SizeY;


    //public float3 initPos;
    [NativeDisableParallelForRestriction]
    public BufferFromEntity<VoxelData> voxelData;

    public Entity entity;
    
    [DeallocateOnJobCompletion]
    public NativeArray<Entity> entities;


    public float3 initPos;

    public void Execute(int i)
    {
        DynamicBuffer<VoxelData> voxels = voxelData[entity];
        int iz = i / ((SizeX - 0) * (SizeY - 0));
        int ib = i - (((SizeX - 0) * (SizeY - 0)) * iz);
        int iy = ib / (SizeX - 0);
        int ix = ib % (SizeX - 0);

        float z = iz;
        float y = iy;
        float x = ix;


        VoxelData value = voxels[i];
        value.value = fbm_Noise((float3(x, y, z) + initPos)/64 ,3);
        voxels[i] = value;
    }

    float fbm_Noise(float3 pos, int octaves)
    {

        float total = 0;
        float frequency = 1;
        float amplitude = 1;

        
        for (int i = 0; i < octaves; i++)
        {
            total += noise.snoise((pos * frequency)) * amplitude;


            amplitude *= .25f;
            frequency *= 2f;
        }

        return total;
    }


    float3 rotate(float3 p, float3x3 m)
    {

        return float3(p.x * m.c0.x + p.x * m.c1.x + p.x * m.c2.x, p.y * m.c0.y + p.y * m.c1.y + p.y * m.c2.y, p.z * m.c0.z + p.z * m.c1.z + p.z * m.c2.z);
    }


    float Circle(float3 pos)
    {
        float x = pos.x - 8;
        float y = pos.y - 8;
        float z = pos.z - 8;

        return x * x + y * y + z * z - 36;

    }

}

