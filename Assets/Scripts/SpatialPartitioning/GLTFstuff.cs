using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityGLTF;
using System.IO;

public class GLTFstuff : MonoBehaviour
{
    //Code adapted from: https://github.com/prefrontalcortex/UnityGLTF/blob/master/UnityGLTF/Assets/UnityGLTF/Samples~/Sample%20Scenes/Scripts/GLTFExporterTest.cs
    public static void ExportGameObjectToPath(GameObject gameObject, string path = "Assets/Resources/GLTF/exportedGLTF.gltf")
    {
        if (gameObject == null)
        {
            Debug.LogError("The gameObject you want to export is empty!");
        }

        gameObject.name = "exportedGLTF";
        var exporter = new GLTFSceneExporter(new[] { gameObject.transform }, new ExportOptions());
        exporter.SaveGLTFandBin(path,"exportedGLTF");

        Debug.Log($"safed VoI at {path}");
    }

    // Code adapted from: https://github.com/atteneder/glTFast/blob/main/Documentation~/ImportRuntime.md
    public static void ImportGameObjectFromPath(GameObject attachToThis, string path = "file:///Assets/Resources/GLTF/exportedGLTF.gltf",
        bool changeColor = false)
    {
        if (attachToThis == null)
        {
            Debug.LogError("couldnt find gameobject to Attach import to!");
            return;
        }
        var gltf = attachToThis.AddComponent<GLTFast.GltfAsset>();
        gltf.Url = path;
        Debug.Log($"Imported Gameobject from {path} and attached it to {attachToThis.name}");
    }
}
