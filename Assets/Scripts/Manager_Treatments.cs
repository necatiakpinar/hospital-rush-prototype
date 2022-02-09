using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Treatments : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject PrefabPill;
    [SerializeField] private GameObject PrefabSyrup;

    public GameObject CreateTreatment(ETreatmentType P_TreatmentType)
    {
        GameObject treatmentObject;
        if (P_TreatmentType == ETreatmentType.PILL) treatmentObject = GameObject.Instantiate(PrefabPill,Vector3.zero,Quaternion.identity);
        else if (P_TreatmentType == ETreatmentType.SYRUP) treatmentObject = GameObject.Instantiate(PrefabSyrup,Vector3.zero,Quaternion.identity);
        else treatmentObject = null;

        return treatmentObject;
    }
}
