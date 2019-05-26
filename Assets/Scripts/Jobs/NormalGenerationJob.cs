using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using static Unity.Mathematics.math;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public struct NormalGenerationJob : IJobParallelFor
{



    [NativeDisableParallelForRestriction]
    public BufferFromEntity<filteredVerticesArray> filteredVerticesData;

    [NativeDisableParallelForRestriction]
    public BufferFromEntity<normalArray> normalsData;

    public Entity entity;

    [DeallocateOnJobCompletion]
    public NativeArray<Entity> entities;

    [NativeDisableParallelForRestriction]
    public BufferFromEntity<indicesArray> indicesData;

    public float3 initPos;



    public void Execute(int i)
    {


        DynamicBuffer<normalArray> normalArray = normalsData[entity];
        DynamicBuffer<filteredVerticesArray> filteredVerticesArray = filteredVerticesData[entity];
        DynamicBuffer<indicesArray> indicesArray = indicesData[entity];

        indicesArray index = indicesArray[i];
        index.value = i;
        indicesArray[i] = index;


        normalArray normal = normalArray[i];
        normal.value = fbm_NoiseDerivative((float3(filteredVerticesArray[i].value) + initPos) / (64), 3);
        normalArray[i] = normal;

        //float3 normal;
        //noise.snoise((float3(meshData[i].x, meshData[i].y, meshData[i].z) + initPos)/(32), out normal);
        //normalData[i] = fbm_NoiseDerivative((float3(meshData[i].x + time, meshData[i].y, meshData[i].z)) / (16), 3);

    }

    float3 fbm_NoiseDerivative(float3 pos, int octaves)
    {

        float3 total = float3(0,0,0);
        float frequency = 1;
        float amplitude = 1;

        float3 derivative = float3(0,0,0);
        for (int i = 0; i < octaves; i++)
        {
            noise.snoise((pos * frequency), out derivative);
            total += derivative * amplitude;

            amplitude *= .5f;
            frequency *= 2;
        }

        return -normalize(total);
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

    float3 CircleNormal(float3 pos)
    {

        float H = 0.1f;
        float dx = Circle(pos + float3(H, 0, 0)) - Circle(pos - float3(H, 0, 0));
        float dy = Circle(pos + float3(0, H, 0)) - Circle(pos - float3(0, H, 0));
        float dz = Circle(pos + float3(0, 0, H)) - Circle(pos - float3(0, 0, H));
        return float3(dx, dy, dz);

    }


}
