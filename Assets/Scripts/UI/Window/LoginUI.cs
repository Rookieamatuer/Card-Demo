using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoginUI : UIBase
{
    private void Awake()
    {
        // Game start
        Register("Background/MainMenu/StartButton").onClick = onStartGameBtn;

    }

    private void onStartGameBtn(GameObject go, PointerEventData pointerEventData)
    {
        Close();
        // Battle initialize
        FightManager.Instance.ChangeType(FightType.Init);
    }

}
