using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopSkinHairUI : MonoBehaviour
{
    [SerializeField] private Button btn;

    public SkinHatSO skinHairSO;
    public Image hud;
    public UnityAction<string> ShopSkinItemAction;

    private int shopItemID;

    private void Start()
    {
        btn.onClick.AddListener(ResetButton);
    }

    private void ResetButton()
    {
        BuySkinButton.Ins.SetShopSkinTag(ShopSkinTag.hair);
        BuySkinButton.Ins.SetSkinHairSO(skinHairSO);


        shopItemID = skinHairSO.IDSkin;

        bool isUnlocked = Pref.GetBool(PrefConst.CUR_SKINSHIELD_ID + shopItemID);

        if (isUnlocked)
        {
            if (shopItemID == Pref.CurShieldId) this.ShopSkinItemAction("UnEqqip");
            else this.ShopSkinItemAction("Select");
        }
        else
        {
            this.ShopSkinItemAction(skinHairSO.skinPrice.ToString());
        }
    }


    public void SetInfoItem(int currentIndex)
    {
        hud.sprite = SOManager.Ins.skinHairS0[currentIndex].hud;

        skinHairSO = SOManager.Ins.skinHairS0[currentIndex];

    }


}
