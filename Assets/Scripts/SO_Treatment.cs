using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum ETreatmentType
{
    PILL,
    SYRUP

}

[CreateAssetMenu(fileName = "Treatment", menuName = "ScriptableObjects/SO_Treatment", order = 1)]
public class SO_Treatment : ScriptableObject
{
    public ETreatmentType TreatmentType;

}
