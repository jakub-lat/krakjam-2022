using System.Collections.Generic;
using System.Linq;
using Cyberultimate.Unity;
using UnityEngine;

namespace Game
{
    public class LevelGenerator : MonoSingleton<LevelGenerator>
    {
        [SerializeField] private List<LevelPart> levelParts;
        [SerializeField] private float levelPartSize;
        private Vector3 bottomLeft;
        
        public void GenerateLevel(float difficulty)
        {
            // var level = new GameObject[3, 3];
            var parts = new List<LevelPart>();
            for (var i = 0; i < 9; i++)
            {
                var part = levelParts[Random.Range(0, levelParts.Count - 1)];
                Debug.Log(part.name);
                
                var pos = bottomLeft;
                int x = i / 3, z = i % 3;
                pos.x += levelPartSize * x;
                pos.z += levelPartSize * z;

                parts.Add(Instantiate(part, pos, Quaternion.identity, transform));
            }
            
            
        }
    }
}
