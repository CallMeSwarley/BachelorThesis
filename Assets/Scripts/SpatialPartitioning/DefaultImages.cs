using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DefaultImages : MonoBehaviour
{
    public static List<GalleryStorage.ImageMarking> createImages()
    {
        List<GalleryStorage.ImageMarking> defaultImageMarkings = new();
        string imagePath = "Assets/Resources/";
        //Marking 1
        Texture2D image = getImage(imagePath + "DefaultImg_1.png");
        Matrix4x4 projectionMatrix = new();
        projectionMatrix.SetRow(0, new Vector4(-0.20349f, 0.56113f, 3.46815f, 0.13889f));
        projectionMatrix.SetRow(1, new Vector4((float)-2.56011, (float)0.59554, (float)-0.24656, (float)-0.37114));
        projectionMatrix.SetRow(2, new Vector4((float)-0.23569, (float)-0.96149, (float)0.14174, (float)0.75694));
        projectionMatrix.SetRow(3, new Vector4((float)-0.23568, (float)-0.96144, (float)0.14173, (float)0.85689));
        GalleryStorage.ImageMarking marking = new GalleryStorage.ImageMarking(image,
            new List<Vector3>() { new Vector3(0.66f, 0.92f, 20.00f), new Vector3(0.66f, 0.89f, 20.00f), new Vector3(0.66f, 0.89f, 20.00f), new Vector3(0.67f, 0.79f, 20.00f), new Vector3((float)0.67, (float)0.79, (float)20.00), new Vector3((float)0.67, (float)0.69, (float)20.00), new Vector3((float)0.67, (float)0.58, (float)20.00), new Vector3((float)0.67, (float)0.58, (float)20.00), new Vector3((float)0.63, (float)0.46, (float)20.00), new Vector3((float)0.63, (float)0.46, (float)20.00), new Vector3((float)0.57, (float)0.36, (float)20.00), new Vector3((float)0.57, (float)0.36, (float)20.00), new Vector3((float)0.48, (float)0.27, (float)20.00), new Vector3(0.48f, 0.27f, 20.00f), new Vector3(0.39f, 0.22f, 20.00f), new Vector3(0.39f, 0.22f, 20.00f), new Vector3(0.23f, 0.18f, 20.00f), new Vector3(0.23f, 0.18f, 20.00f), new Vector3(0.08f, 0.19f, 20.00f), new Vector3(0.08f, 0.19f, 20.00f), new Vector3(-0.02f, 0.26f, 20.00f), new Vector3(-0.02f, 0.26f, 20.00f), new Vector3(-0.09f, 0.39f, 20.00f), new Vector3(-0.09f, 0.39f, 20.00f), new Vector3(-0.14f, 0.51f, 20.00f), new Vector3(-0.14f, 0.51f, 20.00f), new Vector3(-0.12f, 0.73f, 20.00f), new Vector3(-0.12f, 0.73f, 20.00f), new Vector3(-0.07f, 0.84f, 20.00f), new Vector3(-0.07f, 0.84f, 20.00f), new Vector3(0.04f, 0.94f, 20.00f), new Vector3(0.08f, 0.95f, 20.00f), new Vector3(0.10f, 0.94f, 20.00f), new Vector3(0.11f, 0.93f, 20.00f), new Vector3(0.11f, 0.93f, 20.00f) },
            projectionMatrix, new Vector3((float)0.07, (float)0.85, (float)-0.17), new Quaternion((float)0.36837, (float)-0.54323, (float)0.49683, (float)0.56777),
            new List<Vector3>() { new Vector3((float)0.07, (float)0.85, (float)-0.17), new Vector3((float)293.38, (float)-2182.54, (float)-206.34), new Vector3((float)-1176.49, (float)-1843.05, (float)-347.51), new Vector3((float)-1242.21, (float)-1661.81, (float)772.67), new Vector3((float)227.65, (float)-2001.30, (float)913.84) },
            new List<Vector3>() { new Vector3((float)-600.66, (float)-1908.27, (float)167.54), new Vector3((float)-2032.57, (float)-1577.55, (float)30.02), new Vector3((float)-2094.74, (float)-1406.10, (float)1089.66), new Vector3((float)-662.83, (float)-1736.83, (float)1227.19), new Vector3((float)0.07, (float)0.85, (float)-0.17) });
        defaultImageMarkings.Add(marking);

        //Marking 2
        image = getImage(imagePath + "DefaultImg_2.png");
        projectionMatrix.SetRow(0, new Vector4((float)0.47829, (float)0.06287, (float)3.48592, (float)-0.22638));
        projectionMatrix.SetRow(1, new Vector4((float)-0.83302, (float)2.50418, (float)0.06913, (float)-0.30285));
        projectionMatrix.SetRow(2, new Vector4((float)-0.93868, (float)-0.31763, (float)0.13452, (float)0.62889));
        projectionMatrix.SetRow(3, new Vector4((float)-0.93863, (float)-0.31761, (float)0.13452, (float)0.72885));
        marking = new GalleryStorage.ImageMarking(image, new List<Vector3>() { new Vector3((float)0.33, (float)0.34, (float)20.00), new Vector3((float)0.33, (float)0.33, (float)20.00), new Vector3((float)0.31, (float)0.29, (float)20.00), new Vector3((float)0.31, (float)0.29, (float)20.00), new Vector3((float)0.27, (float)0.19, (float)20.00), new Vector3((float)0.27, (float)0.19, (float)20.00), new Vector3((float)0.22, (float)0.10, (float)20.00), new Vector3((float)0.22, (float)0.10, (float)20.00), new Vector3((float)0.16, (float)0.01, (float)20.00), new Vector3((float)0.09, (float)-0.09, (float)20.00), new Vector3((float)0.01, (float)-0.16, (float)20.00), new Vector3((float)0.01, (float)-0.16, (float)20.00), new Vector3((float)-0.12, (float)-0.26, (float)20.00), new Vector3((float)-0.29, (float)-0.30, (float)20.00), new Vector3((float)-0.37, (float)-0.26, (float)20.00), new Vector3((float)-0.37, (float)-0.26, (float)20.00), new Vector3((float)-0.44, (float)-0.18, (float)20.00), new Vector3((float)-0.44, (float)-0.18, (float)20.00), new Vector3((float)-0.50, (float)-0.07, (float)20.00), new Vector3((float)-0.50, (float)-0.07, (float)20.00), new Vector3((float)-0.53, (float)0.03, (float)20.00), new Vector3((float)-0.53, (float)0.03, (float)20.00), new Vector3((float)-0.57, (float)0.15, (float)20.00), new Vector3((float)-0.57, (float)0.26, (float)20.00), new Vector3((float)-0.57, (float)0.26, (float)20.00), new Vector3((float)-0.55, (float)0.42, (float)20.00), new Vector3((float)-0.55, (float)0.42, (float)20.00), new Vector3((float)-0.49, (float)0.54, (float)20.00), new Vector3((float)-0.38, (float)0.61, (float)20.00), new Vector3((float)-0.38, (float)0.61, (float)20.00), new Vector3((float)-0.25, (float)0.66, (float)20.00), new Vector3((float)-0.23, (float)0.64, (float)20.00), new Vector3((float)-0.23, (float)0.64, (float)20.00) },
            projectionMatrix, new Vector3((float)0.66, (float)0.34, (float)-0.03), new Quaternion((float)0.11230, (float)-0.64469, (float)0.10868, (float)0.74830),
            new List<Vector3>() { new Vector3((float)0.66, (float)0.34, (float)-0.03), new Vector3((float)-1715.34, (float)-1360.61, (float)-313.84), new Vector3((float)-2195.67, (float)75.83, (float)-273.84), new Vector3((float)-2041.19, (float)96.14, (float)852.07), new Vector3((float)-1560.86, (float)-1340.30, (float)812.07) },
            new List<Vector3>() { new Vector3((float)-1864.24, (float)-858.48, (float)-167.25), new Vector3((float)-2262.07, (float)331.24, (float)-134.12), new Vector3((float)-2150.41, (float)345.92, (float)679.69), new Vector3((float)-1752.58, (float)-843.80, (float)646.56), new Vector3((float)0.66, (float)0.34, (float)-0.03) });
        defaultImageMarkings.Add(marking);

        //Marking 3
        image = getImage(imagePath + "DefaultImg_3.png");
        projectionMatrix.SetRow(0, new Vector4((float)2.80532, (float)0.34141, (float)-2.09713, (float)0.38241));
        projectionMatrix.SetRow(1, new Vector4((float)0.24359, (float)2.52341, (float)0.73667, (float)-0.41645));
        projectionMatrix.SetRow(2, new Vector4((float)0.59656, (float)-0.27895, (float)0.75260, (float)0.96365));
        projectionMatrix.SetRow(3, new Vector4((float)0.59653, (float)-0.27893, (float)0.75256, (float)1.06360));
        marking = new GalleryStorage.ImageMarking(image, new List<Vector3>() { new Vector3((float)-0.15, (float)0.15, (float)20.00), new Vector3((float)-0.15, (float)0.12, (float)20.00), new Vector3((float)-0.12, (float)0.08, (float)20.00), new Vector3((float)-0.12, (float)0.08, (float)20.00), new Vector3((float)-0.10, (float)-0.02, (float)20.00), new Vector3((float)-0.10, (float)-0.02, (float)20.00), new Vector3((float)-0.10, (float)-0.14, (float)20.00), new Vector3((float)-0.10, (float)-0.27, (float)20.00), new Vector3((float)-0.11, (float)-0.39, (float)20.00), new Vector3((float)-0.11, (float)-0.39, (float)20.00), new Vector3((float)-0.15, (float)-0.49, (float)20.00), new Vector3((float)-0.15, (float)-0.49, (float)20.00), new Vector3((float)-0.20, (float)-0.58, (float)20.00), new Vector3((float)-0.20, (float)-0.58, (float)20.00), new Vector3((float)-0.30, (float)-0.64, (float)20.00), new Vector3((float)-0.33, (float)-0.64, (float)20.00), new Vector3((float)-0.33, (float)-0.64, (float)20.00), new Vector3((float)-0.46, (float)-0.63, (float)20.00), new Vector3((float)-0.46, (float)-0.63, (float)20.00), new Vector3((float)-0.57, (float)-0.61, (float)20.00), new Vector3((float)-0.57, (float)-0.61, (float)20.00), new Vector3((float)-0.66, (float)-0.55, (float)20.00), new Vector3((float)-0.70, (float)-0.46, (float)20.00), new Vector3((float)-0.70, (float)-0.46, (float)20.00), new Vector3((float)-0.71, (float)-0.33, (float)20.00), new Vector3((float)-0.71, (float)-0.33, (float)20.00), new Vector3((float)-0.71, (float)-0.17, (float)20.00), new Vector3((float)-0.71, (float)-0.06, (float)20.00), new Vector3((float)-0.71, (float)-0.06, (float)20.00), new Vector3((float)-0.69, (float)0.04, (float)20.00), new Vector3((float)-0.69, (float)0.04, (float)20.00), new Vector3((float)-0.67, (float)0.15, (float)20.00), new Vector3((float)-0.65, (float)0.18, (float)20.00), new Vector3((float)-0.62, (float)0.19, (float)20.00), new Vector3((float)-0.62, (float)0.19, (float)20.00) },
            projectionMatrix, new Vector3((float)-0.71, (float)0.44, (float)-0.69), new Quaternion((float)0.15110, (float)0.32735, (float)0.00580, (float)0.93273),
            new List<Vector3>() { new Vector3((float)-0.71, (float)0.44, (float)-0.69), new Vector3((float)669.69, (float)-1333.77, (float)1632.39), new Vector3((float)810.92, (float)113.80, (float)2056.98), new Vector3((float)1717.01, (float)224.07, (float)1379.63), new Vector3((float)1575.78, (float)-1223.50, (float)955.04) },
            new List<Vector3>() { new Vector3((float)718.45, (float)-1070.82, (float)1691.20), new Vector3((float)802.62, (float)-208.10, (float)1944.25), new Vector3((float)1195.63, (float)-160.27, (float)1650.45), new Vector3((float)1111.46, (float)-1022.99, (float)1397.41), new Vector3((float)-0.71, (float)0.44, (float)-0.69) });
        defaultImageMarkings.Add(marking);

        return defaultImageMarkings;
    }

    private static Texture2D getImage(string path)
    {
        byte[] imgBytes = File.ReadAllBytes(path);
        Texture2D image = new Texture2D(2, 2);
        if (imgBytes != null)
        {
            image.LoadImage(imgBytes);
            return image;
        }
        else
        {
            throw new FileNotFoundException($"Couldn't get image at {path}");
        }
    }
}
