using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkinHatsSO", menuName = "SkinHatsSO")]
public class SkinHatSO : ScriptableObject
{
    public Skin skinHatPrefab;
    public int IDSkin;
    public int skinPrice;
    public Sprite hud;
}

