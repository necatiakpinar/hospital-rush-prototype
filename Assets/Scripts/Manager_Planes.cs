using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Planes : MonoBehaviour
{
    [Header("Planes")]
    [SerializeField] private List<Controller_Plane> ListPlanes;
    [SerializeField] private GameObject purchaseArea;
    public bool isPlanesActive;

    private void Start() 
    {
        if (isPlanesActive) DisablePurchaseArea();
        else EnablePurchaseArea();
    }
    public void EnableAllPlanes()
    {
        foreach(Controller_Plane plane in ListPlanes) plane.EnablePlane();
    }

    public void EnablePurchaseArea() => purchaseArea.SetActive(true);
    public void DisablePurchaseArea()  => purchaseArea.SetActive(false);
    
}
