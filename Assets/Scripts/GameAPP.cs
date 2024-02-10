using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameAPP : MonoBehaviour
{
    public static GameAPP instance;
    public static GameObject setting;

    private void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize config list
        GameConfigManager.Instance.Init();
        // Initialize player information
        RoleManager.Instance.Init();
        // Initialize audio manager
        AudioManager.Instance.Init();
        setting = GameObject.FindGameObjectWithTag("setting");
        UIManager.Instance.ShowUI<LoginUI>("LoginUI");

        // Play BGM
        AudioManager.Instance.PlayBGM("bgm1", true);

        // test
        //string name = GameConfigManager.Instance.GetCardById("1001")["Name"];
        //Debug.Log(name);
    }

}
