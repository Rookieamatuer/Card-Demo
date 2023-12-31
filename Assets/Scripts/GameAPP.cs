using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAPP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Initialize config list
        GameConfigManager.Instance.Init();
        // Initialize player information
        RoleManager.Instance.Init();
        // Initialize audio manager
        AudioManager.Instance.Init();

        UIManager.Instance.ShowUI<LoginUI>("LoginUI");

        // Play BGM
        AudioManager.Instance.PlayBGM("bgm1", true);

        // test
        //string name = GameConfigManager.Instance.GetCardById("1001")["Name"];
        //Debug.Log(name);
    }

}
