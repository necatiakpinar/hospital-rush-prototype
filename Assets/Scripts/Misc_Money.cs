using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misc_Money : MonoBehaviour
{
    public int CurrencyValue = 25;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == Manager_Game.Instance.Layers.Player)
        {
            Manager_Game.Instance.IncreaseCurrency(CurrencyValue);
            Debug.Log($"Güncel para tutar {Manager_Game.Instance.Currency}");
            GameObject.Destroy(this.gameObject);
        }
    }
}
