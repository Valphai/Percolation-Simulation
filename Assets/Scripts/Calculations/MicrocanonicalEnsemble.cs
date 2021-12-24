using UnityEngine;
using UnityEngine.Pool;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Calculations
{
    public class MicrocanonicalEnsemble
    {
        private void Load () 
        {
            string path = Path.Combine(Application.persistentDataPath, "test.map");

            using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
            {
                
            }
        }

        
    }
}