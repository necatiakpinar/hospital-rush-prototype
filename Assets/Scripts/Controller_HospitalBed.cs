using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller_HospitalBed : MonoBehaviour
{
    public Transform customerSleepParent;
    public Image ImageTreatment;

    [Header("Treatments")]
    public Sprite ImageTreatmentPill;
    public Sprite ImageTreatmentSyrup;
    public bool isBedFull;


    [Header("Detector")]
    [SerializeField] private Vector3 origin;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float sphereRadius;

    public Vector3 CustomerExitLocation;

    private Manager_Upgrades ManagerUpgrades;

    private void Start()
    {
        // isBedFull = false;    
        ManagerUpgrades = Manager_Game.Instance.ManagerUpgrades;
    }

    public void ShowCustomerTreatment(ETreatmentType P_TreatmentType)
    {
        ImageTreatment.gameObject.SetActive(true);
        if (P_TreatmentType == ETreatmentType.PILL) ImageTreatment.sprite = ImageTreatmentPill;
        else if (P_TreatmentType == ETreatmentType.SYRUP) ImageTreatment.sprite = ImageTreatmentSyrup;
        
    } 

    private void FixedUpdate() 
    {
        DetectColliders();
    }

    private void DetectColliders()
    {
        // origin = this.transform.position;
        direction = this.transform.forward;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + origin, sphereRadius);
        foreach (var hitCollider in hitColliders)
        {
            //Hospital Bed
            if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Controller_Player player = hitCollider.gameObject.GetComponent<Controller_Player>();
                if (player != null && player.listCustomers.Count > 0 && !isBedFull )
                {
                    player.DropToBed(player.listCustomers[player.listCustomers.Count - 1], this);
                    AddHospitalBedToFullList();
                }
            }
        }
    }

    public void AddHospitalBedToFullList() 
    {
        ManagerUpgrades.ListFullHospitalBeds.Add(this);
        ManagerUpgrades.ListEmptyHospitalBeds.Remove(this);
    }
    public void AddHospitalBedEmptyList() 
    {
        ManagerUpgrades.ListEmptyHospitalBeds.Add(this);
        ManagerUpgrades.ListFullHospitalBeds.Remove(this);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + origin + direction,sphereRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position + CustomerExitLocation , 1);

    }
}
