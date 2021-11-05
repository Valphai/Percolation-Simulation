using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.Pool;
using Grid;
using UnityEngine;

namespace PlayMode
{
    public class CoordinatesTest
    {
        [UnityTest]
        public IEnumerator Disks_Match_Bins()
        {
            int b = 1;

            GameObject c = new GameObject();
            c.AddComponent(typeof(PoolHelper));
            PoolHelper pH = c.GetComponent<PoolHelper>();

            var dPool = new ObjectPool<Disk>(
                pH.CreateDisk, pH.TakeFromPool, pH.ReleaseFromPool);

            GridSystem grid = GridSystem.GridSetup(dPool,
                    Grid.Metrics.SpawnLower, Grid.Metrics.SpawnHigher);

            for (int i = 0; i < grid.bins.Length; i++)
            {
                GridBin bin = grid.bins[i];
                for (int j = 0; j < bin.Disks.Count; j++)
                {
                    Coordinates diskCoords = bin.Disks[j].Coordinates;
                    if (bin.Coordinates.IntVectorPositon() != diskCoords.IntVectorPositon())
                    {
                        b = 0;
                    }
                }
            }
            Assert.AreEqual(b,1);
            yield return null;
        }
    }
}
