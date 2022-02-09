using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Customers : MonoBehaviour
{
    [SerializeField] private GameObject PrefabCustomer;
    [SerializeField] private Transform CustomersParent;
    public List<Controller_Customer> ListCustomer;

    public Transform CustomerWaitPlace;
    public Transform CustomerExitPlace;
    private void Start() 
    {
        ListCustomer = new List<Controller_Customer>();
        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        GameObject customerObject = GameObject.Instantiate(PrefabCustomer,CustomersParent);
        Controller_Customer customer = customerObject.GetComponent<Controller_Customer>();
        customer.customerMovePlace = CustomerWaitPlace;
        ListCustomer.Add(customer);
    }


}
