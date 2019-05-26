using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Entities.Extensions;
using Unity.Jobs;
using Unity.Rendering;

[UpdateAfter(typeof(NormalGenerationSystem))]
public class RenderChunkMesh : JobComponentSystem
{


    EntityQuery eq;
    List<Vector3> verticesList;
    List<int> trianglesList;
    List<Vector3> normalsList;
    //Mesh mesh;
    Material material;


    protected override void OnCreate()
    {
        eq = World.Active.EntityManager.CreateEntityQuery(typeof(shouldRender));
        verticesList = new List<Vector3>();
        trianglesList = new List<int>();
        normalsList = new List<Vector3>();
        material = Resources.Load("Test", typeof(Material)) as Material;
    }





    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        //throw new System.NotImplementedException();

        NativeArray<Entity> entities = eq.ToEntityArray(Allocator.TempJob);


        if (entities.Length == 0)
        {
            return inputDeps;
        }

        World.Active.EntityManager.AddComponent(entities[0], typeof(completeChunk));
        World.Active.EntityManager.RemoveComponent(entities[0], typeof(shouldRender));


        //Mesh mesh = new Mesh();
        /*SetMesh(mesh, GetBufferFromEntity<filteredVerticesArray>(false)[entities[0]].Reinterpret<Vector3>(),
                      GetBufferFromEntity<normalArray>(false)[entities[0]].Reinterpret<Vector3>(),
                      GetBufferFromEntity<indicesArray>(false)[entities[0]].Reinterpret<int>());

        //World.Active.EntityManager.GetComponentObject<Mesh>(entities[0]) = mesh; 


        /*World.Active.EntityManager.SetSharedComponentData(entities[0], new RenderMesh {
            mesh = mesh,
            material = material
        });*/
        //Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);

        //mesh.Clear();


        RenderMesh rM = new RenderMesh();
        rM.mesh = new Mesh();
        SetMesh(rM.mesh, GetBufferFromEntity<filteredVerticesArray>(false)[entities[0]].Reinterpret<Vector3>(),
                      GetBufferFromEntity<normalArray>(false)[entities[0]].Reinterpret<Vector3>(),
                      GetBufferFromEntity<indicesArray>(false)[entities[0]].Reinterpret<int>());
        rM.material = material;


        World.Active.EntityManager.SetSharedComponentData(entities[0], rM);


        entities.Dispose();

        return inputDeps;
    }

    private void SetMesh(
            Mesh mesh,
            DynamicBuffer<Vector3> vertices,
            DynamicBuffer<Vector3> normals,
            DynamicBuffer<int> triangles)
    {
        mesh.Clear();

        if (vertices.Length == 0)
        {
            return;
        }
        
        verticesList.AddRange(vertices);
        normalsList.AddRange(normals);
        trianglesList.AddRange(triangles);





        mesh.SetVertices(this.verticesList);
        mesh.SetNormals(this.normalsList);
        mesh.SetTriangles(this.trianglesList, 0);

        verticesList.Clear();
        normalsList.Clear();
        trianglesList.Clear();
    }

}
