// code modified from https://github.com/Scrawk/Marching-Cubes/blob/master/Assets/MarchingCubes/Example.cs (MIT LICENSE)

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public enum MARCHING_MODE { CUBES, TETRAHEDRON };

public class MarchingCubesAdapter
{

    public Material material;
    public bool validMesh = false;

    private VoxelGridMC voxelGridMC;

    public void marchingCubesOnVoxelArray(VoxelGridMC voxelGridMC, Material material)
    {
        this.voxelGridMC = voxelGridMC;
        this.material = material;
        bool[,,] voxelsInput = voxelGridMC.getAllVoxes();

        Marching marching = new MarchingCubes
        {

            //Surface is the value that represents the surface of mesh
            //For example the perlin noise has a range of -1 to 1 so the mid point is where we want the surface to cut through.
            //The target value does not have to be the mid point it can be any value with in the range.
            Surface = -0.49f // -> this gives the most accurate results compared to the old intersection method
        };

        //The size of voxel array.
        int width = voxelsInput.GetLength(0);
        int height = voxelsInput.GetLength(1);
        int depth = voxelsInput.GetLength(2);

        VoxelArray voxels = new VoxelArray(width, height, depth);

        //Fill voxels with values based on our 3D bool array
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (voxelsInput[x, y, z])//inside
                    {
                        voxels[x, y, z] = 0.5f;
                    }
                    else // outside
                    {
                        voxels[x, y, z] = -0.5f;
                    }
                }
            }
        }

        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();

        //The mesh produced is not optimal. There is one vert for each index.
        //Would need to weld vertices for better quality mesh.
        marching.Generate(voxels.Voxels, verts, indices, voxelGridMC);

        Transform gridspace = voxelGridMC.gridSpace();

        var position = new Vector3(voxelGridMC.start_x, voxelGridMC.start_y, voxelGridMC.start_z);
        position = gridspace.rotation * Vector3.Scale(gridspace.localScale, position) + gridspace.position; // positioncalc from VoxelGrid
        if (verts.Count != 0)
        {
            validMesh = true;
            CreateMesh32(verts, indices, position);
        }


    }

    private void CreateMesh32(List<Vector3> verts, List<int> indices, Vector3 position)
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.SetVertices(verts);
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        Transform parent = voxelGridMC.gridSpace();

        GameObject go = new GameObject("MarchingCubesMesh");
        go.transform.SetParent(parent, false);
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();
        go.GetComponent<Renderer>().material = material;
        go.GetComponent<MeshFilter>().mesh = mesh;
    }
}


