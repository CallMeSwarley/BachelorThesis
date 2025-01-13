using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

/*
 * Represents a voxel grid use for finding the volume covered by the projected markings
 */
public class VoxelGrid : MonoBehaviour
{

    #region attributes
    
    // only current voxel size, not initial voxel size
    public float voxelSize { get; private set; }

    [SerializeField] private Settings settings;
    
    //[SerializeField] private Text volume;

    [SerializeField]
    private Transform _gridSpace;

    public Transform gridSpace()
    {
        return _gridSpace;
    }

    // the valid voxels
    private ConcurrentBag<Vector3> voxels;

    // to resolve shared voxelcorners just once
    private ConcurrentDictionary<Vector3, bool> voxelCorners;

    // temporary buffer
    private Vector3[] voxelArray;


    public bool calculationDone() { return voxelsLeft == 0; }
    private int voxelsLeft;

    #endregion attributes
    

    #region getter
    public Vector3[] getVoxels()
    {
        return voxels.ToArray();
    }

    public int getVoxelCount()
    {
        return voxels.Count;
    }
    #endregion getter


    #region methods
    public void initialize(float autoVoxelSize = 0.0f)
    {
        if(!calculationDone())
        {
            // this should never happen, but better throwing an exception than getting undefined behavior
            throw new Exception("Could not start calculation as another one is already running!");
        }

        voxels = new ConcurrentBag<Vector3>();
        voxelSize = (autoVoxelSize == 0.0f)? settings.getFloatByName("Initial voxel size") : autoVoxelSize;

        float 
            start_x = settings.getFloatByName("X start"),
            end_x = settings.getFloatByName("X end"),
            start_y = settings.getFloatByName("Y start"),
            end_y = settings.getFloatByName("Y end"),
            start_z = settings.getFloatByName("Z start"),
            end_z = settings.getFloatByName("Z end");

        var cubeVolume = (end_x - start_x) * (end_y - start_y) *
                         (end_z - start_z);
        
        //volume.text = "Volume is: " + cubeVolume;
        
        /* 
         * At least the designated space should be filled.
         * If the space is too small for one Voxel (but not 0) it should be placed nevertheless.
         * If a voxel has a size of 1 and the space a length of 3.001, then 4 voxels are being placed along the corresponding axis.
         */
        for (float x = start_x + voxelSize / 2; x < end_x + voxelSize / 2; x += voxelSize)
        {
            for (float y = start_y + voxelSize / 2; y < end_y + voxelSize / 2; y += voxelSize)
            {
                for(float z=start_z + voxelSize/2; z < end_z + voxelSize / 2; z += voxelSize)
                {
                    voxels.Add(_gridSpace.rotation * Vector3.Scale(_gridSpace.localScale, new Vector3(x, y, z)) + _gridSpace.position);
                }
            }
        }
    }
    
    /*
     * Receives a function, which returns a list of voxel centers for each position.
     * This is intended to be used for both subdivison tasks and simple checks.
     * Subdivison task:
     *      E.g., func takes each voxel, divides it into 8 subvoxels and checks some condition on them. If they pass, they are added to the list and then returned.
     * Simple check:
     *      E.g., func takes each voxel and evaluates a function on their center. If they pass, they are returned as the single element of the list. Otherwise, an empty list is returned.
     *      
     * doForAllVoxels() deletes all initial voxels and stores only the combined results of all func()-calls
     */ 
    public void doForAllVoxels(Func<Vector3, List<Vector3>> func)
    {
        voxelArray = voxels.ToArray();
        voxels = new ConcurrentBag<Vector3>();

        /*
         * Cache all checked voxel corners with the corresponding solution, as 8 neighbouring voxels need the same result.
         * It would help with calculation, if it was not renewed not per subdivison iteration (as here) but per entire intersection algorithm. Because any voxel corner in an iteration before, might be used again in the subdivided voxel grid in the next iteration.
         * However, this would also store those voxel corners which got cut off and therefore unnecessarily increase the memory used.
         */
        voxelCorners = new ConcurrentDictionary<Vector3, bool>();

        voxelsLeft = voxelArray.Length;
        int voxelCount = voxelArray.Length;

        if(voxelCount == 0)
        {
            Debug.Log("Empty voxel grid!");
            return;
        }

        for (int i = 0; i < voxelCount; i++)
        {
            int voxelIndex = i;

            // this is very ugly (but also because the ThreadPool does not seem to show occured exceptions) this might help to easily switch to an iterative approach for debugging
#if true
            ThreadPool.QueueUserWorkItem(state => {
                List<Vector3> list = checkSingleVoxel(func, voxelIndex, ref voxelsLeft);
                foreach (Vector3 voxel in list)
                {
                    voxels.Add(voxel);
                }
                });
#else
            try
            {
                List<Vector3> list = checkSingleVoxel(func, tmp, ref voxelsLeft);
                foreach (Vector3 voxel in list)
                {
                    voxels.Add(voxel);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
#endif
        }
    }

    [Obsolete("In the current approach, this is not used, as all voxels have to be checked iff they got subdivided before. Use subdivideVoxels() instead.")]
    public void checkVoxels(Func<Vector3, bool> check)
    {
        doForAllVoxels(new Func<Vector3, List<Vector3>> (voxel =>
            {
                List<Vector3> result = new List<Vector3>();
                if(checkAllCorners(check, voxel))
                {
                    result.Add(voxel);
                }
                return result;
            }));
    }

    // subdivides all referenced voxels and add all those new voxels, which pass the check, to the new list of voxels
    public void subdivideVoxels(Func<Vector3, bool> check)
    {
        voxelSize /= 2.0f;
        doForAllVoxels(new Func<Vector3, List<Vector3>>(voxel => {
            List<Vector3> result = new List<Vector3>();
            for (int j = 0; j < 8; j++)
            {
                // iterate over all 8 points which are +-voxelSize in each axis direction off the center
                Vector3 tmp = voxel + new Vector3((j % 2 - 0.5f) * (voxelSize), ((j / 2) % 2 - 0.5f) * (voxelSize), ((j / 4) % 2 - 0.5f) * (voxelSize));
                if(checkAllCorners(check, tmp))
                {
                    result.Add(tmp);
                }
            }
            return result;
        }));
    }

    /* Perform the check on all 8 corners of the voxel.
     * Returns true iff at least one voxel failed the check and one passed it.
     * I.e., only voxels at the border of at least one marking result in true.
     */
    public bool checkAllCorners(Func<Vector3, bool> check, Vector3 voxel)
    {
        bool tmp_result = false;
        bool result = true;
        for (int j = 0; j < 8; j++)
        {
            Vector3 corner = voxel + new Vector3((j % 2 - 0.5f) * (voxelSize), ((j / 2) % 2 - 0.5f) * (voxelSize), ((j / 4) % 2 - 0.5f) * (voxelSize));
            if (j!=0)
            {
                result &= tmp_result == checkSinglePoint(check, corner);
            } else
            {
                tmp_result = checkSinglePoint(check, corner);
            }
        }
        return !result;
    }

    /*
     * Performs the check on a single point in space.
     * This could be a corner or a voxel center, if the corner-based approach is not desired and only the voxel centers are tested for intersection.
     */
    public bool checkSinglePoint(Func<Vector3, bool> check, Vector3 point)
    {
        bool result = false;
        if(voxelCorners.TryGetValue(point, out result))
        {
            return result;
        } else
        {
            result = check(point);
            voxelCorners.TryAdd(point, result);
            return result;
        }
    }

#region submethods
    /*
     * Returns the result from applying func on a the voxel with index voxelIndex.
     * This should be a function similar to the one used in subdivideVoxels() or checkVoxels()
     * 
     * It also decrements the voxelsLeft counter to inform when all voxels have ben processed
     */ 
    private List<Vector3> checkSingleVoxel(Func<Vector3, List<Vector3>> func, int voxelIndex, ref int voxelsLeft)
    {
        List<Vector3> result = func(voxelArray[voxelIndex]);
        Interlocked.Decrement(ref voxelsLeft);

        return result;
    }
#endregion submethods

#endregion methods
}
