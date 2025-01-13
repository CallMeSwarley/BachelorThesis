// Marching cubes implementation taken ande adapted to our codebase from // https://github.com/Scrawk/Marching-Cubes (MIT license)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class MeshHelper : MonoBehaviour
{
    static void displayMesh(string meshName, Mesh mesch, Transform gridSpace, Material material)
    {
        GameObject voxelGridMesh = new GameObject(meshName);
        voxelGridMesh.AddComponent<MeshFilter>();
        MeshRenderer render = voxelGridMesh.AddComponent<MeshRenderer>();
        if (render != null)
        {
            render.material = material;
        }
        voxelGridMesh.transform.GetComponent<MeshFilter>().mesh = mesch;
        voxelGridMesh.transform.SetParent(gridSpace, false);
        voxelGridMesh.transform.gameObject.SetActive(true);
    }

    //angepasst von https://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html
    // kombiniert alle meshes und gibt diese zurück -> braucht viel speicher da uch innere ecken gespeichert werden
    public static void combineMesh(List<MeshFilter> meshFilters, Transform gridSpace, Material material)
    {
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        int i = 0;
        while (i < meshFilters.Count)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i].gameObject);
            i++;
        }
        Mesh mesch = new Mesh();
        mesch.indexFormat = IndexFormat.UInt32;
        mesch.CombineMeshes(combine);
        displayMesh("VoxelGridAsMesh", mesch, gridSpace, material);
    }
}
