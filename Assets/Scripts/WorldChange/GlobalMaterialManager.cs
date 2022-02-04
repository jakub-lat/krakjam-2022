using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalMaterialSet
{
    public WorldTypeDict<Material> wallMaterials;
    public WorldTypeDict<Material> floorMaterials;
    public WorldTypeDict<Material> roofMaterials;
    public Material furniturePsychoMaterial;
}

public class GlobalMaterialManager : MonoSingleton<GlobalMaterialManager>
{
    public enum MaterialType { Wall, Floor, Roof, Furniture }

    public List<GlobalMaterialSet> materialList;
    public int materialSetIndex = 0;

    private GlobalMaterialSet current;

    private void Start()
    {
        if(materialSetIndex<0 || materialSetIndex >= materialList.Count)
        {
            Debug.LogError("Material set index out of range");
        }

        current = materialList[materialSetIndex];
    }

    public  Material GetMaterial(MaterialType t, WorldTypeController.WorldType wt = WorldTypeController.WorldType.Normal)
    {
        switch (t)
        {
            case MaterialType.Furniture:
                if (wt == WorldTypeController.WorldType.Normal)
                {
                    Debug.Log("Cant return a normal material dor furniture");
                    return null;
                }
                return current.furniturePsychoMaterial;
            case MaterialType.Wall:
                return current.wallMaterials[wt];
            case MaterialType.Floor:
                return current.floorMaterials[wt];
            case MaterialType.Roof:
                return current.roofMaterials[wt];
            default:
                Debug.LogError("Material type not implemented");
                return null;
        }
    }
}
