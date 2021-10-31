using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Grid;

namespace PlayMode
{
    public class CoordinatesTest
    {
        [UnityTest]
        public IEnumerator Disks_Match_Bins()
        {
            GridSystem grid = GridSystem.GridSetup();

            for (int i = 0; i < grid.bins.Length; i++)
            {
                GridBin bin = grid.bins[i];
                // for (int j = 0; j < bin.Neighbors.Length; j++)
                // {
                Coordinates diskCoords = bin.disks[0].Coordinates;

                Assert.AreEqual(bin.Coordinates.IntVectorPositon(),
                                diskCoords.IntVectorPositon());
                // }
            }
            yield return null;
        }
    }
}
