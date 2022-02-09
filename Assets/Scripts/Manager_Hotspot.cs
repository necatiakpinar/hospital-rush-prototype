using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public enum EHotspotType{
    TREATMENT,
    UPGRADE
}

public class Manager_Hotspot : MonoBehaviour
{
    public Controller_Player Player;
    public EHotspotType currentHotspotType;
    public ETreatmentType treatmentType;
    public EUpgradeType upgradeType;
    public int hotspotPrice;

    public Timer Timer;

    [Header("UI")]
    [SerializeField] public GameObject UIMoney;
    [SerializeField] public GameObject UILocked;
    [SerializeField] public GameObject UIBorders;
    [SerializeField] public GameObject UIHover;
    [SerializeField] public TMP_Text UITextHotspotPrice;

    public Transform CustomerDropPosition;

    private Manager_Upgrades ManagerUpgrades;
    private Controller_Treatment ManagerTreatments;
    public Manager_Planes ManagerPlanes;
    
    private float timer;

    private void OnValidate() 
    {
        if (currentHotspotType == EHotspotType.TREATMENT)
        {
            hotspotPrice = 0;
            UIMoney.SetActive(false);
            UILocked.SetActive(false);
            UIBorders.SetActive(false);
            UIHover.SetActive(true);
        }
    }
    private void Awake()
    {
        timer = 0;
    }
    private void Update() 
    {
        if (currentHotspotType == EHotspotType.TREATMENT)
        {
            if (Timer.timerFinished) OnTimerFinished();
        } 
        else if (hotspotPrice == 0) OnTimerFinished();
    }

    public void OnTimerFinished()
    {
        if (currentHotspotType == EHotspotType.TREATMENT)
        {
            GameObject treatmentObject;
            if (treatmentType == ETreatmentType.PILL) treatmentObject = Manager_Game.Instance.ManagerTreatments.CreateTreatment(ETreatmentType.PILL);
            else if (treatmentType == ETreatmentType.SYRUP) treatmentObject = Manager_Game.Instance.ManagerTreatments.CreateTreatment(ETreatmentType.SYRUP);
            else treatmentObject = null;
            
            Player.PickTreatment(treatmentObject.GetComponent<Controller_Treatment>());
        }
        else if (currentHotspotType == EHotspotType.UPGRADE && upgradeType == EUpgradeType.BED)
        {
            Manager_Game.Instance.ManagerUpgrades.CreateHospitalBed(this.gameObject);

        }
        else if (currentHotspotType == EHotspotType.UPGRADE && upgradeType == EUpgradeType.AREA)
        {
            ManagerPlanes.EnableAllPlanes();
            GameObject.Destroy(this.gameObject);
        }

        
        if (Player != null) Timer.StartPickingCounter(); //If player still in hotspot restart timer!

    }

    private void OnTriggerEnter(Collider P_Collision) 
    {
        if (P_Collision.gameObject.layer == Manager_Game.Instance.Layers.Player)
        {
            Player = P_Collision.GetComponent<Controller_Player>();
//            if(Player == null || (currentHotspotType == EHotspotType.TREATMENT && Player.LastCustomer != null)) return;

            if (currentHotspotType == EHotspotType.TREATMENT) Timer.StartPickingCounter();
        }
    }

    private int moneyDecreaseRate = 1;
    private void OnTriggerStay(Collider P_Collision) 
    {
        if (P_Collision.gameObject.layer == Manager_Game.Instance.Layers.Player)
        {
            if (currentHotspotType != EHotspotType.TREATMENT)
            {
                if (hotspotPrice > 0 && Manager_Game.Instance.Currency > 0 && P_Collision.gameObject.GetComponent<Controller_Player>().MovementVector == Vector3.zero)
                {
                    hotspotPrice -= moneyDecreaseRate;
                    Manager_Game.Instance.DecreaseCurrency(moneyDecreaseRate);
                    UITextHotspotPrice.text = hotspotPrice.ToString();
                }
            }
        }
    }

    private void OnTriggerExit(Collider P_Collision) 
    {
        Player = null;
        Timer.StopPickingCounter();
    }
}
