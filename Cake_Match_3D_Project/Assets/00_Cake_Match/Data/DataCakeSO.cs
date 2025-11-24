using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Cake", fileName = "ListCakeData")]
public class DataCakeSO : ScriptableObject
{
    public List<CAKE> L_TypeCake;

    public CAKE getCakeByType(E_TypeCake typeCake)
    {
        for (int i = 0; i < L_TypeCake.Count; i++)
        {
            if (L_TypeCake[i]._TypeCake == typeCake)
            {
                return L_TypeCake[i];
            }
        }

        return null;
    }
}

[Serializable]
public class CAKE
{
    public E_TypeCake _TypeCake;
    public Mesh _Mesh;
    public Material _Material;
    public int PointLevel;
}
