using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Manager_Upgrades : MonoBehaviour
{

    [Header("Hospital Beds")]
    private float BedYPosition = 0.8f;
    [SerializeField] private GameObject PrefabHospitalBed;
    [SerializeField] private Transform BedParent;
    public List<Controller_HospitalBed> ListHospitalBeds;
    public List<Controller_HospitalBed> ListFullHospitalBeds;
    public List<Controller_HospitalBed> ListEmptyHospitalBeds;
    public int AvailableHospitalBedCount { get { return ListEmptyHospitalBeds.Count; } private set{}}
    

    public float PlayerResetOffset_Y;

    private void Start() 
    {
        ListEmptyHospitalBeds = new List<Controller_HospitalBed>();
        ListFullHospitalBeds = new List<Controller_HospitalBed>();
        
        foreach(Transform hospitalBed in BedParent)
        {
            ListEmptyHospitalBeds.Add(hospitalBed.gameObject.GetComponent<Controller_HospitalBed>());
        }
    }

    public void CreateHospitalBed(GameObject P_Hotspot)
    {
        
        Controller_HospitalBed hospitalBed = GameObject.Instantiate(PrefabHospitalBed, new Vector3(P_Hotspot.transform.position.x,BedYPosition,P_Hotspot.transform.position.z), Quaternion.identity, BedParent).GetComponent<Controller_HospitalBed>();
        ListEmptyHospitalBeds.Add(hospitalBed);
        Manager_Game.Instance.Player.CharacterController.enabled = false;
        Manager_Game.Instance.Player.transform.position = hospitalBed.transform.position + hospitalBed.CustomerExitLocation + new Vector3(0, PlayerResetOffset_Y, 0);
        Manager_Game.Instance.Player.CharacterController.enabled = true;

        if(Manager_Game.Instance.Player.LastCustomer != null)
        {
            Debug.Log("why");
            Manager_Game.Instance.Player.DropToBed(Manager_Game.Instance.Player.LastCustomer, hospitalBed);
            hospitalBed.isBedFull = true;
        } 

        Manager_Game.Instance.ManagerCustomers.SpawnCustomer();
        GameObject.Destroy(P_Hotspot);
    }

}
