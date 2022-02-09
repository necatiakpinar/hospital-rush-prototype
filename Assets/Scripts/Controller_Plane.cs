using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Plane : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material MVisiblePlane;
    [SerializeField] private Material MInVisiblePlane;

    [Header("Plane Miscs")]
    [SerializeField] private List<GameObject> ListHotspots;

    private Manager_Planes ManagerPlanes;

    private void Awake() 
    {
        ManagerPlanes = (this.transform.parent.GetComponent<Manager_Planes>()) ? this.transform.parent.GetComponent<Manager_Planes>() : null;
    }
    public void Start() 
    {
        if (ManagerPlanes.isPlanesActive) EnablePlane();
        else DisablePlane();
    }

    public void EnablePlane()
    {
        foreach(GameObject hotspot in ListHotspots) hotspot.SetActive(true);
        this.gameObject.GetComponent<MeshRenderer>().material = MVisiblePlane;
    }

    public void DisablePlane()
    {
        foreach(GameObject hotspot in ListHotspots) hotspot.SetActive(false);
        this.gameObject.GetComponent<MeshRenderer>().material = MInVisiblePlane;
    }


}
