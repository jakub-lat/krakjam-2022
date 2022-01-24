using UnityEngine;
namespace LetterBattle
{
    public static class ParticleHelper
    {
        public static GameObject Spawn(GameObject obj, Vector2 pos)
        {
            if (obj == null) return null;
            GameObject temp = UnityEngine.Object.Instantiate(obj);
            temp.transform.position = pos;
            return temp;
        }
    }
}