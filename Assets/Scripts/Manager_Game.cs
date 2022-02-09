using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SLayers
{
    public int Player;
    public int Customer;
    public int Hotspot;
    public int HospitalBed;
    public int Money;
}
public class Manager_Game : MonoBehaviour
{

    [Header("Managers")]
    public Manager_Upgrades ManagerUpgrades;
    public Manager_Treatments ManagerTreatments;
    public Manager_Customers ManagerCustomers;
    public Manager_UI ManagerUI;

    [Header("Objects")]
    public GameObject PF_Money;
    public Controller_Player Player;
    public Transform ExitDoor;
    
    public static Manager_Game Instance;

    public Transform GroundPosition;

    public int Currency;
    public SLayers Layers;

    private void Awake() 
    {
        Instance = this;
        ManagerUpgrades = this.GetComponent<Manager_Upgrades>();
        ManagerTreatments = this.GetComponent<Manager_Treatments>();
        ManagerCustomers = this.GetComponent<Manager_Customers>();
        ManagerUI = this.GetComponent<Manager_UI>();

        Currency = 0;
        
        Layers.Player = LayerMask.NameToLayer("Player");
        Layers.Customer = LayerMask.NameToLayer("Customer");
        Layers.Hotspot = LayerMask.NameToLayer("Hotspot");
        Layers.HospitalBed = LayerMask.NameToLayer("HospitalBed");
        Layers.Money = LayerMask.NameToLayer("Money");
        
    }

    public void IncreaseCurrency(int P_Amount)
    {
        Currency += P_Amount;
        ManagerUI.TextCurrency.text = Currency.ToString();
    }

    public void DecreaseCurrency(int P_Amount)
    {
        Currency -= P_Amount;
        ManagerUI.TextCurrency.text = Currency.ToString();
    }
    
}
