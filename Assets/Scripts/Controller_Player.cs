using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Controller_Player : MonoBehaviour
{
    [Header("Attributes")]
    public Transform carryParent;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("Components")]
    [SerializeField] private Animator Animator;
    [SerializeField] private Transform CustomerDropPosition;
    
    [Header("Collision Detector")]
    [SerializeField] private float sphereRadius;
    [SerializeField] private Vector3 origin;
    [SerializeField] private Vector3 direction;

    
    public CharacterController CharacterController;
    private Rigidbody playerBody;
    public Vector3 MovementVector = Vector3.zero;


    public List<Controller_Customer> listCustomers;
    public List<Controller_Treatment> listTreatments;
    
    public Controller_Customer LastCustomer { get { return (listCustomers.Count > 0) ? listCustomers[listCustomers.Count - 1] : null; }}
    private void Awake() 
    { 
        this.CharacterController = GetComponent<CharacterController>();
        this.playerBody = GetComponent<Rigidbody>();
        this.listCustomers = new List<Controller_Customer>();
        this.listTreatments = new List<Controller_Treatment>();
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) Animator.SetBool("IsDancing",true);
        
    }

    void FixedUpdate()
    {
        KeyboardMovement();
        MouseMovement();
        //DetectColliders();
    }
    private void LateUpdate()
    {
        if (MovementVector.magnitude > 0) this.Animator.SetBool("IsRunning",true);
        else this.Animator.SetBool("IsRunning",false);
    }

    private void KeyboardMovement()
    {
        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");
        MovementVector = new Vector3(Horizontal, 0f, Vertical).normalized;
        this.CharacterController.Move(MovementVector * movementSpeed * Time.deltaTime);
        // this.transform.Translate(MovementVector * movementSpeed * Time.deltaTime,Space.World);
        
        if (MovementVector != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(MovementVector,Vector3.up);
            this.transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotation,rotationSpeed * Time.deltaTime);
        }
    }
    public float TouchMovementThreshold = 1;    
    public Vector3 MouseStart;
    public float MouseStartToCurrentDistance;
    private void MouseMovement()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MouseStart = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0))
        {
            MouseStartToCurrentDistance = Vector3.Distance(MouseStart, Input.mousePosition);
            if(MouseStartToCurrentDistance > TouchMovementThreshold)
            {
                Vector3 direction = (Input.mousePosition - MouseStart);
                MovementVector = new Vector3(direction.x, 0, direction.y).normalized;
                CharacterController.Move(MovementVector * movementSpeed * Time.deltaTime);

                if (MovementVector != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(MovementVector, Vector3.up);
                    this.transform.rotation = Quaternion.RotateTowards(transform.rotation,toRotation,rotationSpeed * Time.fixedDeltaTime);
                }
            }
        }
    }

    
    private void DetectColliders()
    {
        origin = this.transform.position;
        direction = this.transform.forward;
        Collider[] hitColliders = Physics.OverlapSphere(origin, sphereRadius);
        foreach (var hitCollider in hitColliders)
        {
            //Customer
            if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Customer") &&  hitCollider.gameObject.GetComponent<Controller_Customer>() != null && //Pick Customer
                hitCollider.gameObject.GetComponent<Controller_Customer>().currentCustomerStatus == ECustomerStatus.WAITING)
            {
                Controller_Customer customer = hitCollider.gameObject.GetComponent<Controller_Customer>();
                if (customer.timer.timerFinished)
                {
                    this.PickCustomer(customer);
                }
                else if (!customer.timer.IsTimerRunning)
                {
                    customer.timer.StartPickingCounter();
                }
                
            } 
            else if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Customer") &&  hitCollider.gameObject.GetComponent<Controller_Customer>() != null && //Give Medicine
                hitCollider.gameObject.GetComponent<Controller_Customer>().currentCustomerStatus == ECustomerStatus.RESTORATION &&
                this.listTreatments.Count > 0)
            {
                Controller_Customer customer = hitCollider.gameObject.GetComponent<Controller_Customer>();
                this.GiveMedicine(customer,listTreatments);
            }
        
        }
    }

    #region General
    public void ClearPlayerHands()
    {
        foreach(Controller_Customer customer in listCustomers) //If there is customer in player hands, make sure you changed customers status FIRST, then clear the list!
        {
            customer.GetComponent<Collider>().enabled = true;                
            customer.transform.parent = Manager_Game.Instance.GroundPosition;

            customer.transform.position = CustomerDropPosition.position;
            customer.currentCustomerStatus = ECustomerStatus.WALKING_TO_WAITING_PLACE; //You can make Waiting 
            customer.animator.SetBool("IsPicked", false);
            customer.animator.SetBool("IsWalking", true);

            //customer.transform.localPosition = Vector3.zero;
            customer.transform.localRotation = Quaternion.Euler(0,0,0);
        }

        foreach(Transform entity in carryParent) if (entity.gameObject.GetComponent<Controller_Treatment>()) GameObject.Destroy(entity.gameObject);

        currentCustomerYPosition = 0;
        currentTreatmentYPosition = 0;
        this.listCustomers.Clear();
        this.listTreatments.Clear(); //Clear all the treatments

        //Animation
        Animator.SetBool("IsCarrying",false);

    }
    #endregion



    #region Customer
    float currentCustomerYPosition = 0;
    float customerYPositionIncreaseRate = 0.4f;

    public void PickCustomer(Controller_Customer P_Customer)
    {
        if (this.listTreatments.Count > 0) ClearPlayerHands(); //Make sure player carries only customer before carring the next one!

        P_Customer.GetComponent<Collider>().enabled = false;
        P_Customer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        this.listCustomers.Add(P_Customer);
        P_Customer.currentCustomerStatus = ECustomerStatus.PICKED;
        P_Customer.transform.parent = carryParent;
        P_Customer.transform.localPosition = new Vector3(0,currentCustomerYPosition,0);
        currentCustomerYPosition += customerYPositionIncreaseRate;
        P_Customer.transform.localRotation = Quaternion.Euler(0,90,0);
        

        //Animation
        Animator.SetBool("IsCarrying",true);
        P_Customer.animator.SetBool("IsWalking",false);
        P_Customer.animator.SetBool("IsPicked",true);
    }
    public void DeleteCustomer(Controller_Customer P_Customer)
    {
        this.listCustomers.Remove(P_Customer);
    }
    public void DropToBed(Controller_Customer P_Customer, Controller_HospitalBed P_HospitalBed)
    {
        P_Customer.GetComponent<Collider>().enabled = true;
        P_Customer.navAgent.enabled = false;
        P_Customer.currentCustomerStatus = ECustomerStatus.RESTORATION;
        P_Customer.transform.parent = P_HospitalBed.customerSleepParent;
        P_Customer.transform.localPosition = Vector3.zero;
        P_Customer.transform.localRotation = Quaternion.Euler(0,0,0);
        currentCustomerYPosition -= customerYPositionIncreaseRate;
        P_Customer.SetHospitalBed(P_HospitalBed);
        this.DeleteCustomer(P_Customer);


        P_HospitalBed.isBedFull = true;
        P_HospitalBed.ShowCustomerTreatment(P_Customer.customerNeededTreatmentType);
        
        //Animation
        if (this.listCustomers.Count == 0) Animator.SetBool("IsCarrying",false);

    }

    public void GiveMedicine(Controller_Customer P_Customer, List<Controller_Treatment> P_TreatmentList)
    {
        foreach (Controller_Treatment treatment in listTreatments)
        {
            if (P_Customer.customerNeededTreatmentType == treatment.treatmentProperties.TreatmentType)
            {
                P_Customer.TreatCustomer();
                GameObject.Destroy(treatment.gameObject);
                this.RemoveTreatment(treatment);
                
                break;
            }
        }

        //Re-order treatment game objects
        currentTreatmentYPosition = 0;
        foreach (Controller_Treatment treatment in listTreatments)
        {
            treatment.transform.localPosition = new Vector3(0,currentTreatmentYPosition,0);
            currentTreatmentYPosition += treatmentYPositionIncreaseRate;
        }
    }

    #endregion

    #region Treatment
    float currentTreatmentYPosition = 0;
    float treatmentYPositionIncreaseRate = 0.9f;
    public void PickTreatment(Controller_Treatment P_Treatment)
    {
        if (listCustomers.Count > 0) ClearPlayerHands(); //Make sure player carries only customer before carring the next one!

        this.listTreatments.Add(P_Treatment);   
        P_Treatment.transform.parent = carryParent;
        P_Treatment.transform.localPosition = new Vector3(0,currentTreatmentYPosition,0);
        P_Treatment.transform.localRotation = Quaternion.identity;
        currentTreatmentYPosition += treatmentYPositionIncreaseRate;

        //Animation
        Animator.SetBool("IsCarrying",true);

    }
    public void RemoveTreatment(Controller_Treatment P_Treatment)
    {
        this.listTreatments.Remove(P_Treatment);
        if (this.listTreatments.Count == 0) Animator.SetBool("IsCarrying",false);
    }

    #endregion

    
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin + direction,sphereRadius);
    }

    public void AddCurrency(int P_GoldAmount)
    {
        Manager_Game.Instance.IncreaseCurrency(P_GoldAmount);
    }    

}
