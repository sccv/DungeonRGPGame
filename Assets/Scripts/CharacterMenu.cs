using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // Text fields
    public Text levelText, hitpointText, pesosText, upgradeCostText, xpText;

    // Logic 
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    // Character Selection Function
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            // If we went too far away (out of the character array)
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;

            OnSelectionChanged();  
        }
        else
        {
            currentCharacterSelection--;

            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;

            OnSelectionChanged();
        }
    }

    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    // Weapon upgrade
    public void OnUpgradeClick()
    {
        GameManager.instance.TryUpgradeWeapon();
        UpdateMenu();
    }

    // Upgrade character information
    public void UpdateMenu()
    {
        // Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponSprites.Count - 1)
        {
            upgradeCostText.text = "MAX";
            upgradeCostText.color = Color.yellow; 
        }
        else
        {
            upgradeCostText.text = "Upgrade \n" + GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString() + " pesos";
        }
        

        // Text
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();

        // XP bar
        if (GameManager.instance.GetCurrentLevel() == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " experience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXP = GameManager.instance.GetXPToLevel(GameManager.instance.GetCurrentLevel() - 1);
            int currentLevelXp = GameManager.instance.GetXPToLevel(GameManager.instance.GetCurrentLevel());

            int diff = currentLevelXp - prevLevelXP;
            int currentXPIntoLevel = GameManager.instance.experience - prevLevelXP;

            float completionRatio = (float)currentXPIntoLevel / diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currentXPIntoLevel.ToString() + " / " + diff;
        }
    }
}
