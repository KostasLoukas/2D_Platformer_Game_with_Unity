using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public class MoneyCounter : MonoBehaviour
{
    private Text moneyText;

    
    void Awake()
    {
        moneyText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "Money: " + GameMaster.Money;
    }
}
