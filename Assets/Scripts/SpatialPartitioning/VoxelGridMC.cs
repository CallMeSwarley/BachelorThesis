using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

//represents the Voxelgrid class but suitable for the Marching cubes Algorithm 
public class VoxelGridMC : MonoBehaviour
{
    #region attributes
    public float voxelSize { get; private set; }

    [SerializeField] private Settings settings;
    [SerializeField] private Transform _gridSpace;

    public Transform gridSpace() { return _gridSpace; }

    // the valid voxels
    private bool[,,] voxels;

    private bool calculationReady = true;
    public bool calculationDone() { return calculationReady; }
    [HideInInspector]
    public float start_x, end_x, start_y, end_y, start_z, end_z;
    #endregion attributes

    #region getter
    public Vector3[] getVoxels()
    {
        List<Vector3> validVoxels = new List<Vector3>();
        for (int x = 0; x < voxels.GetLength(0); x++)
        {
            for (int y = 0; y < voxels.GetLength(1); y++)
            {
                for (int z = 0; z < voxels.GetLength(2); z++)
                {
                    if (voxels[x, y, z])
                    {
                        float xPos = start_x + x * voxelSize;
                        float yPos = start_y + y * voxelSize;
                        float zPos = start_z + z * voxelSize;
                        validVoxels.Add(_gridSpace.rotation * Vector3.Scale(_gridSpace.localScale, new Vector3(xPos, yPos, zPos)) + _gridSpace.position);
                    }
                }
            }
        }
        return validVoxels.ToArray();
    }

    public bool[,,] getAllVoxes()
    {
        return voxels;
    }
    public int getValidVoxelCount()
    {
        int count = 0;
        for (int x = 0; x < voxels.GetLength(0); x++)
        {
            for (int y = 0; y < voxels.GetLength(1); y++)
            {
                for (int z = 0; z < voxels.GetLength(2); z++)
                {
                    if (voxels[x, y, z]) // only valid (true) voxels are interesting for us
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }

    public int getTotalVoxelCount()
    {
        int count = 0;
        for (int x = 0; x < voxels.GetLength(0); x++)
        {
            for (int y = 0; y < voxels.GetLength(1); y++)
            {
                for (int z = 0; z < voxels.GetLength(2); z++)
                {
                    count++;
                }
            }
        }
        return count;
    }
    #endregion getter

    #region methods
    public void initialize(float autoVoxelSize = 0.0f)
    {
        if (!calculationDone())
        {
            throw new Exception("Could not start calculation as another one is already running!");
        }

        start_x = settings.getFloatByName("X start");
        end_x = settings.getFloatByName("X end");
        start_y = settings.getFloatByName("Y start");
        end_y = settings.getFloatByName("Y end");
        start_z = settings.getFloatByName("Z start");
        end_z = settings.getFloatByName("Z end");
        voxelSize = (autoVoxelSize == 0.0f) ? settings.getFloatByName("Initial voxel size") : autoVoxelSize;
        int sizeX = (int)Math.Ceiling(Math.Abs((end_x - start_x) / voxelSize));
        int sizeY = (int)Math.Ceiling(Math.Abs((end_y - start_y) / voxelSize));
        int sizeZ = (int)Math.Ceiling(Math.Abs((end_z - start_z) / voxelSize));
        voxels = new bool[sizeX, sizeY, sizeZ];
    }



    public void subdivideVoxels(Func<Vector3, bool> check, bool gridExists)
    {
        calculationReady = false;
        voxelSize /= 2.0f;
        bool[,,] newVoxels = new bool[(int)(voxels.GetLength(0) * 2), (int)(voxels.GetLength(1) * 2), (int)(voxels.GetLength(2) * 2)];
        // storing variables outside the nested loop increases the performance a little
        Quaternion rotation = _gridSpace.rotation;
        Vector3 position = _gridSpace.position;
        Vector3 localScale = _gridSpace.localScale;
        // for iterating the new Array with doubled dimensions
        int xLength = newVoxels.GetLength(0);
        int yLength = newVoxels.GetLength(1);
        int zLength = newVoxels.GetLength(2);
        // old voxelArray dimensions, to look at neighbors
        int xVoxel = voxels.GetLength(0);
        int yVoxel = voxels.GetLength(1);
        int zVoxel = voxels.GetLength(2);
        Vector3 multWithThis = rotation * localScale;

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    int ix = x / 2;
                    int iy = y / 2;
                    int iz = z / 2;
                    /*if the current voxel and all 6 neigboring voxels are false, then we skip
                     -> this improves the performance very much*/
                    if (gridExists && (voxels[ix, iy, iz] || (ix > 0 && voxels[ix - 1, iy, iz]) ||
                        (ix < xVoxel - 1 && voxels[ix + 1, iy, iz]) ||
                        (iy > 0 && voxels[ix, iy - 1, iz]) ||
                        (iy < yVoxel - 1 && voxels[ix, iy + 1, iz]) ||
                        (iz > 0 && voxels[ix, iy, iz - 1]) || (iz < zVoxel - 1 && voxels[ix, iy, iz + 1])))
                    {
                        // you have to multiply x, y, z by the VoxelSize and add startVal to get the real position
                        float xPos = start_x + x * voxelSize;
                        float yPos = start_y + y * voxelSize;
                        float zPos = start_z + z * voxelSize;
                        Vector3 voxel = new Vector3(xPos, yPos, zPos) + position;
                        voxel.x *= multWithThis.x;
                        voxel.y *= multWithThis.y;
                        voxel.z *= multWithThis.z;

                        newVoxels[x, y, z] = check(voxel); //this checks only voxel center
                    }
                    else if (!gridExists) // this initializes the starting grid, which we subdivide later
                    {
                        // you have to multiply x, y, z by the VoxelSize and add startVal to get the real position
                        float xPos = start_x + x * voxelSize;
                        float yPos = start_y + y * voxelSize;
                        float zPos = start_z + z * voxelSize;
                        Vector3 voxel = new Vector3(xPos, yPos, zPos) + position;
                        voxel.x *= multWithThis.x;
                        voxel.y *= multWithThis.y;
                        voxel.z *= multWithThis.z;

                        newVoxels[x, y, z] = check(voxel); //this checks only voxel center
                    }
                    else
                    {
                        newVoxels[x, y, z] = voxels[ix, iy, iz];
                    }

                }
            }
        }
        voxels = newVoxels;
        calculationReady = true;
    }

    public void subdivideVoxelsThreading(Func<Vector3, bool> check, bool gridExists, Quaternion rotation, Vector3 position, Vector3 localScale)
    {
        calculationReady = false;
        voxelSize /= 2.0f;
        bool[,,] newVoxels = new bool[(int)(voxels.GetLength(0) * 2), (int)(voxels.GetLength(1) * 2), (int)(voxels.GetLength(2) * 2)];
        // storing variables outside the nested loop increases the performance a little
        // for iterating the new Array with doubled dimensions
        int xLength = newVoxels.GetLength(0);
        int yLength = newVoxels.GetLength(1);
        int zLength = newVoxels.GetLength(2);
        // old voxelArray dimensions, to look at neighbors
        int xVoxel = voxels.GetLength(0);
        int yVoxel = voxels.GetLength(1);
        int zVoxel = voxels.GetLength(2);
        Vector3 multWithThis = rotation * localScale;

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    int ix = x / 2;
                    int iy = y / 2;
                    int iz = z / 2;
                    /*if the current voxel and all 6 neigboring voxels are false, then we skip
                     -> this improves the performance very much*/
                    if (gridExists && (voxels[ix, iy, iz] || (ix > 0 && voxels[ix - 1, iy, iz]) ||
                        (ix < xVoxel - 1 && voxels[ix + 1, iy, iz]) ||
                        (iy > 0 && voxels[ix, iy - 1, iz]) ||
                        (iy < yVoxel - 1 && voxels[ix, iy + 1, iz]) ||
                        (iz > 0 && voxels[ix, iy, iz - 1]) || (iz < zVoxel - 1 && voxels[ix, iy, iz + 1])))
                    {
                        // you have to multiply x, y, z by the VoxelSize and add startVal to get the real position
                        float xPos = start_x + x * voxelSize;
                        float yPos = start_y + y * voxelSize;
                        float zPos = start_z + z * voxelSize;
                        Vector3 voxel = new Vector3(xPos, yPos, zPos) + position;
                        voxel.x *= multWithThis.x;
                        voxel.y *= multWithThis.y;
                        voxel.z *= multWithThis.z;

                        newVoxels[x, y, z] = check(voxel); //this checks only voxel center
                                                           //newVoxels[x, y, z] = checkVoxelCorners(check, voxel);
                    }
                    else if (!gridExists) // this initializes the starting grid, which we subdivide later
                    {
                        // you have to multiply x, y, z by the VoxelSize and add startVal to get the real position
                        float xPos = start_x + x * voxelSize;
                        float yPos = start_y + y * voxelSize;
                        float zPos = start_z + z * voxelSize;
                        Vector3 voxel = new Vector3(xPos, yPos, zPos) + position;
                        voxel.x *= multWithThis.x;
                        voxel.y *= multWithThis.y;
                        voxel.z *= multWithThis.z;

                        newVoxels[x, y, z] = check(voxel); //this checks only voxel center
                        //newVoxels[x, y, z] = checkVoxelCorners(check, voxel);
                    }
                    else
                    {
                        newVoxels[x, y, z] = voxels[ix, iy, iz];
                    }

                }
            }
        }
        voxels = newVoxels;
        calculationReady = true;
    }

    // we also want the inner voxels. the inner voxels are removed later in marching cubes -> at least one corner should be true
    // if we do it like in VoxelGrid, the mesh would get a smaller coremesh inside
    private bool checkVoxelCorners(Func<Vector3, bool> check, Vector3 voxel)
    {
        bool tmp_result = false;
        bool result = false;
        for (int j = 0; j < 8; j++)
        {
            Vector3 corner = voxel + new Vector3((j % 2 - 0.5f) * voxelSize, (j / 2 % 2 - 0.5f) * voxelSize, (j / 4 % 2 - 0.5f) * voxelSize);
            tmp_result = check(corner);
            result |= tmp_result;
        }
        return result;
    }
    #endregion methods
}

