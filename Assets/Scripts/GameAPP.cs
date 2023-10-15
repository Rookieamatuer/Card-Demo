using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAPP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Initialize audio manager
        // AudioManager.Instance.Init();

        UIManager.Instance.ShowUI<StartInterface>("StartInterface");

        // Play BGM
        // AudioManager.Instance.PlayBGM("", true);
    }

}
