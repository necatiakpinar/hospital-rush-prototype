using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EUpgradeType
{
    BED,
    AREA,
    EXPANSION

}

[CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/SO_Upgrade", order = 1)]
public class SO_Upgrade : ScriptableObject
{
    public EUpgradeType UpgradeType;
}