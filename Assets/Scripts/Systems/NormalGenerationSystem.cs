using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;

[UpdateAfter(typeof(FilterMeshSystem))]
public class NormalGenerationSystem : JobComponentSystem
{
    EntityQuery eq;



    protected override void OnCreate()
    {
        eq = World.Active.EntityManager.CreateEntityQuery(typeof(shouldGenerateNormals));
    }



    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        NativeArray<Entity> entities = eq.ToEntityArray(Allocator.TempJob);


        if (entities.Length == 0)
        {
            return inputDeps;
        }

        World.Active.EntityManager.AddComponent(entities[0], typeof(shouldRender));
        World.Active.EntityManager.RemoveComponent(entities[0], typeof(shouldGenerateNormals));


        NormalGenerationJob NGJ = new NormalGenerationJob
        {
            filteredVerticesData = GetBufferFromEntity<filteredVerticesArray>(false),
            normalsData = GetBufferFromEntity<normalArray>(false),
            indicesData = GetBufferFromEntity<indicesArray>(false),
            entity = entities[0],
            entities = entities,
            initPos = World.Active.EntityManager.GetComponentData<Translation>(entities[0]).Value

        };

        JobHandle normalGenerationHandle = NGJ.Schedule(GetBufferFromEntity<filteredVerticesArray>(false)[entities[0]].Length, 1, inputDeps);

        normalGenerationHandle.Complete();

        return normalGenerationHandle;
    }
}
