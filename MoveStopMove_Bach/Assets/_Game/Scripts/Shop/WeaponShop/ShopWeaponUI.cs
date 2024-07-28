using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class ShopWeaponUI : Singleton<ShopWeaponUI>
{
    [SerializeField] private TMP_Text coinText;

    public TMP_Text priceText;
    public TMP_Text nameEquipment;
    public Image hud;

    private Player player;
    private int currentIndex = 0;


    private void Start()
    {
        SetInfoItem(currentIndex);

        player = GameManager.Ins.Player;
    }

    //change sprite
    public void ChangeNextItem()
    {
        currentIndex++;

        ChangeToNextSprite();
    }
    
    public void ChangeBackItem()
    {
        currentIndex--;

        ChangeToNextSprite();
    }

    private void ChangeToNextSprite()
    {
        hud.sprite = null;

        // if over number of list, return first
        if (currentIndex >= SOManager.Ins.weaponS0.Count)
        {
            currentIndex = 0;
        }

        else if(currentIndex <=0)
        {
            currentIndex = SOManager.Ins.weaponS0.Count - 1;
        }

        SetInfoItem(currentIndex);
    }

    private void SetInfoItem(int currentIndex)
    {

        int shopItemID = SOManager.Ins.weaponS0[currentIndex].IDWeapon;

        if (hud)
        {
            hud.sprite = SOManager.Ins.weaponS0[currentIndex].hud;

            nameEquipment.text = SOManager.Ins.weaponS0[currentIndex].weaponName;

            bool isUnlocked = Pref.GetBool(PrefConst.WEAPON_PEFIX + shopItemID);

            if (isUnlocked)
            {
                if (shopItemID == Pref.CurWeaponId)
                {
                    if (priceText) priceText.text = "Eqquiped";
                }

                else
                {
                    if (priceText) priceText.text = "Select";
                }
            }

            else
            {
                if (priceText) priceText.text = SOManager.Ins.weaponS0[currentIndex].weaPonPrice.ToString();
            }
        }
    }

    public void BuyWeapon()
    {

        int shopItemID = SOManager.Ins.weaponS0[currentIndex].IDWeapon;

        bool isUnlocked = Pref.GetBool(PrefConst.WEAPON_PEFIX + shopItemID);

        if (isUnlocked)
        {
            if (shopItemID == Pref.CurWeaponId) return;

            //change data currentid
            Pref.CurWeaponId = shopItemID;
            player.ChangeWeapon(SOManager.Ins.weaponS0[currentIndex].IDWeapon);
            if (priceText) priceText.text = "Eqquiped";
        }

        else
        {
            if (player.Coins >= SOManager.Ins.weaponS0[currentIndex].weaPonPrice) // check coin
            {
                //update coin
                player.UpdateCoin(SOManager.Ins.weaponS0[currentIndex].weaPonPrice, false);

                //change to unlock
                Pref.SetBool(PrefConst.WEAPON_PEFIX + shopItemID, true);
                Pref.CurWeaponId = shopItemID;

                player.ChangeWeapon(SOManager.Ins.weaponS0[currentIndex].IDWeapon);
                if (priceText) priceText.text = "Eqquiped";
                this.SetCoinText(player.Coins);
            }
        }
    }

    public void SetCoinText(int coin)
    {
        coinText.text = coin.ToString();
    }



}
