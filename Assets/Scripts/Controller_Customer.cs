using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[System.Serializable]
public enum ECustomerStatus{
    WALKING_TO_WAITING_PLACE, //Use this when customers start moving
    WAITING,
    PICKED,
    RESTORATION,
    HEALED,
    WALKING_TO_EXIT  //Use this when customers start moving
}

public class Controller_Customer : MonoBehaviour

{
    [Header("Enums")]
    public ECustomerStatus currentCustomerStatus;
    public ETreatmentType customerNeededTreatmentType;
    
    [Header("Detector")]
    [SerializeField] private Vector3 origin;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float sphereRadius;


    //Timer
    public Timer timer;
    [Header("Components")]
    public Animator animator;
    public Controller_Player player;
    public Controller_HospitalBed hospitalBed;

    public Transform customerMovePlace;

    public NavMeshAgent navAgent;
    //Timer
    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        SetTreatmentType();
        //this.currentCustomerStatus = ECustomerStatus.WAITING; //Make this WALKING when customers start moving!
        this.currentCustomerStatus = ECustomerStatus.WALKING_TO_WAITING_PLACE; //Make this WALKING when customers start moving!
    }
    
    private void FixedUpdate() 
    {
        DetectColliders();
        CustomerMovement();
        
    }

    private void Update()
    {
    }
    private void CustomerMovement()
    {
        if(currentCustomerStatus == ECustomerStatus.WALKING_TO_EXIT)
        {
            this.animator.SetBool("IsWalking", true);
            navAgent.enabled = true;
            navAgent.destination = Manager_Game.Instance.ExitDoor.position;
            if (navAgent.isActiveAndEnabled && navAgent.remainingDistance > 0 && navAgent.remainingDistance < 1) //iLK ÖNCE SIFIR ALIYOR O YÜZDEN DİREKT DESTROY EDİYOR!
            {
                GameObject.Destroy(this.gameObject);
            }
        } 
        else if (currentCustomerStatus == ECustomerStatus.WALKING_TO_WAITING_PLACE) 
        {
            this.animator.SetBool("IsWalking", true);
            navAgent.enabled = true;
            navAgent.destination = customerMovePlace.position;
            if (navAgent.isActiveAndEnabled && navAgent.remainingDistance > 0 && navAgent.remainingDistance < 1) //iLK ÖNCE SIFIR ALIYOR O YÜZDEN DİREKT DESTROY EDİYOR!
            {
                currentCustomerStatus = ECustomerStatus.WAITING;
                this.animator.SetBool("IsWalking", false);
            }
        }
        else navAgent.enabled = false;
        
    }

    public Collider[] hitColliders;
    private void DetectColliders()
    {
        origin = this.transform.position;
        direction = this.transform.forward;
        hitColliders = Physics.OverlapSphere(origin, sphereRadius);
        foreach (var hitCollider in hitColliders)
        {
            //Customer
            if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Controller_Player player = hitCollider.gameObject.GetComponent<Controller_Player>();
                if (this.currentCustomerStatus == ECustomerStatus.WAITING || this.currentCustomerStatus == ECustomerStatus.WALKING_TO_WAITING_PLACE)
                {
                    if (this.timer.timerFinished)
                    {
                        player.PickCustomer(this);
                        this.timer.timerFinished = false;
                    }
                    else if (!this.timer.IsTimerRunning)
                    {
                        this.timer.StartPickingCounter();
                    }
                }
                else if (currentCustomerStatus == ECustomerStatus.RESTORATION && player.listTreatments.Count > 0)
                {
                    player.GiveMedicine(this,player.listTreatments);
                }
               
            }
            else if(!IsPlayerExist()) timer.StopPickingCounter();
        }
    }

    private bool IsPlayerExist()
    {
        foreach (var hitCollider in hitColliders)
        {
            //Customer
            if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return true;
            }
        }
        return false;
                
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin + direction,sphereRadius);
    }
    public void SetTreatmentType()
    {
        int randomIndex = UnityEngine.Random.Range(0,2);
        ETreatmentType randomTreatment = (ETreatmentType)(Enum.GetValues(customerNeededTreatmentType.GetType())).GetValue(randomIndex);
        this.customerNeededTreatmentType = randomTreatment;
    }

    public void SetHospitalBed(Controller_HospitalBed P_HospitalBed)
    {
        this.hospitalBed = P_HospitalBed;
    }
    public void TreatCustomer()
    {
        this.GetComponent<Collider>().isTrigger = true;
        this.currentCustomerStatus = ECustomerStatus.WALKING_TO_EXIT;
        this.navAgent.enabled = true;
        this.navAgent.speed = 2;
        this.GoToExitPlace(Manager_Game.Instance.ManagerCustomers.CustomerExitPlace);
        this.hospitalBed.isBedFull = false;
        this.hospitalBed.ImageTreatment.gameObject.SetActive(false);
        this.transform.position = hospitalBed.transform.position + hospitalBed.CustomerExitLocation;
        this.animator.SetBool("IsPicked", false);
        GameObject money = GameObject.Instantiate(Manager_Game.Instance.PF_Money, hospitalBed.transform.position + hospitalBed.CustomerExitLocation, Quaternion.identity, hospitalBed.transform);
        this.hospitalBed.AddHospitalBedEmptyList();
        this.hospitalBed = null;

        Manager_Game.Instance.ManagerCustomers.SpawnCustomer();
        
    }

    public void GoToExitPlace(Transform P_ExitPlace)
    {
        this.customerMovePlace = P_ExitPlace;
    }
    
}
 
