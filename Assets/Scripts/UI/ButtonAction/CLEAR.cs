using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLEAR : MonoBehaviour
{
    [SerializeField] VoxelGridVisualizer visualizer;
    public void OnClick()
    {
        Destroy(GameObject.Find("exportedGLTF"));
        Destroy(GameObject.Find("serverGLTF"));
        visualizer.meshExists = false;
        visualizer.serverMeshExists = false;
    }
}
