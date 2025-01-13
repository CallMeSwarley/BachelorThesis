using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

// Used to render a voxel grid into the scene
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class VoxelGridVisualizer : MonoBehaviour
{
    [SerializeField]
    private VoxelGridMC voxelGridMC;
    [SerializeField] 
    private VoxelGrid voxelGrid;
    [SerializeField]
    private GameObject voxelPrefab;

    [SerializeField]
    private PopupMessage popupMessage;
    [SerializeField]
    private GameObject loadingScreen;

    public bool meshExists = false;
    public bool serverMeshExists = false;

    //private void Start()
    //{
    //    GLTFstuff.ImportGameObjectFromPath(GameObject.Find("ImageTarget"), "file:///Assets/Resources/GLTF/exportedGLTF.gltf");
    //}
    // for triggering a visualization/update from inspector for debug purposes
    //[SerializeField] private bool visualizing;
    //public void Update()
    //{
    //    if (visualizing)
    //    {
    //        visualizing = false;
    //        sendToserverAndVisualize();
    //        //visualize();
    //    }
    //}


    // store references too all instantiated game objects to delete them (reusing them would be more efficient...)
    private List<GameObject> voxelMarkers = new List<GameObject>();


    public void visualize()
    {
        if (meshExists)
        {
            Destroy(GameObject.Find("exportedGLTF"));
        }
        foreach (GameObject voxelMarker in voxelMarkers)
        {
            Destroy(voxelMarker);
        }
        voxelMarkers = new List<GameObject>();

        Vector3[] voxels = voxelGrid.getVoxels();
        Debug.Log($"###Active Voxel count: {voxels.Length}");
        //popupMessage.PopUp($"Valid Voxel count: {voxels.Length}");
        voxelPrefab.transform.localScale = voxelGrid.voxelSize * Vector3.one;
        // Code for mesh combining adaptedt from //https://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html
        List<MeshFilter> meshFilters = new();
        foreach (Vector3 voxel in voxels)
        {
            GameObject cube = Instantiate(voxelPrefab, voxel, Quaternion.identity, voxelGrid.gridSpace());
            //voxelMarkers.Add(cube);
            meshFilters.Add(cube.GetComponent<MeshFilter>());
            Destroy(cube);
        }

        MeshHelper.combineMesh(meshFilters, voxelGrid.gridSpace(), transform.GetComponent<Renderer>().material);
        GLTFstuff.ExportGameObjectToPath(GameObject.Find("VoxelGridAsMesh"), Application.persistentDataPath);
        meshExists = true;
    }

    public void visualizeMarchingCubes()
    {
        if (meshExists)
        {
            Destroy(GameObject.Find("exportedGLTF"));
            meshExists = false;
        }
        MarchingCubesAdapter adapter = new();
        adapter.marchingCubesOnVoxelArray(voxelGridMC, transform.GetComponent<Renderer>().material);
        //Debug.Log("We have " + voxelGridMC.getValidVoxelCount() + " valid Voxels and " + voxelGridMC.getTotalVoxelCount() + " Voxels in total!");
        //MeshHelper.generateMesh(voxelGridMC, transform.GetComponent<Renderer>().material);
        if (adapter.validMesh)
        {
            GLTFstuff.ExportGameObjectToPath(GameObject.Find("MarchingCubesMesh"), Application.persistentDataPath);
            meshExists = true;
        }
        else
        {
            popupMessage.PopUp($"Couldn't find enough valid Voxels!");
        }
    }

    public void uploadToserverAndVisualize()
    {
        if (serverMeshExists) // es soll nur eins in der szene existieren
        {
            Destroy(GameObject.Find("serverGLTF"));
        }
        SendGLTFtoServer(Application.persistentDataPath + "/exportedGLTF", Application.persistentDataPath + "/serverGLTF");// sendet und importiert dann die antwort in die szene
        serverMeshExists = true;
    }

    public void SendGLTFtoServer(string filePath = "Assets/Resources/GLTF/exportedGLTF", string storeToPath = "Assets/Resources/GLTF/serverGLTF")
    {
        loadingScreen.SetActive(true);
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        StartCoroutine(ServerCommunicator.uploadGLTF(filePath + ".gltf", storeToPath + ".gltf"));
        StartCoroutine(ServerCommunicator.uploadBIN(filePath + ".bin", storeToPath + ".bin", storeToPath + ".gltf", stopwatch, popupMessage, loadingScreen));
    }

    public void downloadAndVisualizeGLTF()
    {
        loadingScreen.SetActive(true);
        if (meshExists) // es soll nur eins in der szene existieren
        {
            Destroy(GameObject.Find("exportedGLTF"));
        }
        string savePath = Application.persistentDataPath + "/exportedGLTF";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        StartCoroutine(ServerCommunicator.downloadGLTF(savePath + ".gltf"));
        StartCoroutine(ServerCommunicator.downloadBIN(savePath + ".bin", savePath + ".gltf", stopwatch, popupMessage, loadingScreen)); // visualisiert im anschluss den download
        meshExists = true;
    }
}
