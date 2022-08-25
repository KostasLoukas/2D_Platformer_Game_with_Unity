using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text speedText;
    [SerializeField]
    private float healthMultiplier = 1.1f;  //upgrading health gives 10% more health (default)
    [SerializeField]
    private float movementSpeedMultiplier = 1.01f;

    [SerializeField]
    private int upgradeCost = 1;

    private PlayerStats playerStats;

    private string hText = "";
    private string sText = "";



    void OnEnable()
    {
        playerStats = PlayerStats.instance;

        if (playerStats.maxHealth >= 300)
        {
            hText = "MAX";
        }
        else
        {
            hText = "";
        }

        if (playerStats.movementSpeed >= 25)
        {
            sText = "MAX";
        }
        else
        {
            sText = "";
        }

        UpdateValues(hText, sText);
    }
    
    
    void UpdateValues(string hText, string sText)
    {
        if (string.Compare(hText, "")  == 0)
        {
            healthText.text = "Health: " + playerStats.maxHealth.ToString();
        }
        else
        {
            healthText.text = "Health: " + hText;
        }

        if (string.Compare(sText, "") == 0)
        {
            speedText.text = "Speed: " + playerStats.movementSpeed.ToString();
        }
        else
        {
            speedText.text = "Speed: " + sText;
        }
    }


    public void UpgradeHealth()
    {
        if (playerStats.maxHealth >= 300)
        {
            hText = "MAX";
            UpdateValues(hText, sText);
        }
        else
        {
            if (GameMaster.Money < upgradeCost)
            {
                AudioManager.instance.PlaySound("NoMoney");
                return;
            }

            hText = "";
            playerStats.maxHealth = (int)(playerStats.maxHealth * healthMultiplier);
            GameMaster.Money -= upgradeCost;
            AudioManager.instance.PlaySound("Money");
            UpdateValues(hText, sText);
        }
    }


    public void UpgradeSpeed()
    {
        if (playerStats.movementSpeed >= 25)
        {
            sText = "MAX";
            UpdateValues(hText, sText);
        }
        else
        {
            if (GameMaster.Money < upgradeCost)
            {
                AudioManager.instance.PlaySound("NoMoney");
                return;
            }

            sText = "";
            //Careful here! When using smaller than 1.1 multipliers, the speed amount will look like it's not changing!
            playerStats.movementSpeed = Mathf.Round(playerStats.movementSpeed * movementSpeedMultiplier);
            GameMaster.Money -= upgradeCost;
            AudioManager.instance.PlaySound("Money");
            UpdateValues(hText, sText);
        }
    }
}
