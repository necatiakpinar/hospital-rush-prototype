using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager_UI : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text TextCurrency;

    private void Start() 
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        TextCurrency.text = Manager_Game.Instance.Currency.ToString();
    }

}
