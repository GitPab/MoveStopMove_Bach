using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : UICanvas
{
    public void ContinueButton()
    {
        GameManager.Ins.ResumeGame();

        UIManager.Ins.OpenUI<GamePlay>().SetupOnOpen(GameManager.Ins.Player);
        Close(0);
    }
    public void RestartButton()
    {
        GameManager.Ins.RestartGame();
        Close(0);
        UIManager.Ins.OpenUI<MainMenu>();
    }
}