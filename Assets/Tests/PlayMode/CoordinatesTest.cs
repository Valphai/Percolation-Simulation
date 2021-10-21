using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Grid;

namespace PlayMode
{
    public class CoordinatesTest
    {
        [UnityTest]
        public IEnumerator Disks_Match_Bins()
        {
            GridSystem grid = GridSetup();

            for (int i = 0; i < grid.bins.Length; i++)
            {
                var binCoords = grid.bins[i].Coordinates;
                var diskCoords = grid.bins[i].disks[0].Coordinates;
                Assert.AreEqual(new Vector2(binCoords.x, binCoords.z),
                                    new Vector2(diskCoords.x, diskCoords.z));
            }

            yield return null;
        }

        private static GridSystem GridSetup()
        {
            GameObject g = new GameObject();
            g.transform.position = Vector3.zero;

            GameObject p = new GameObject();
            p.transform.SetParent(g.transform, false);
            p.AddComponent(typeof(GridMesh));

            g.AddComponent(typeof(GridSystem));
            GridSystem grid = g.GetComponent<GridSystem>();
            return grid;
        }
    }
}
