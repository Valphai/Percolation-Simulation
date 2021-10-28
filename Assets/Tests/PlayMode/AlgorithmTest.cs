using System.Collections;
using Grid;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace PlayMode
{
    public class AlgorithmTest
    {
        [UnityTest]
        public IEnumerator Cluster_Length_Greater_Than_L()
        {
            GridSystem g = Utilities.GridSetup();

            Vector3Int v1;
            foreach (Disk d in UnionFind.Disks)
            {
                if (d == null) break;

                g.unionFind.Find(d.DiskIndex, out v1);
                
                if (v1.x >= g.L || v1.y >= g.L || v1.z >= g.L)
                {
                    Assert.GreaterOrEqual(v1.magnitude, g.L);
                }
            }


            yield return null;
        }
    }
}
