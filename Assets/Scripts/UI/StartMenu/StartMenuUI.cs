using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    public static StartMenuUI instance;
    public Slider bgmSlider, effectSlider;
    public static bool isEasy = true;
    public static bool isPaused = false;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    } else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void Start()
    {
        // bgmSlider = FindAnyObjectByType<Slider>();;
    }
    // Start is called before the first frame update
    public void GameStart()
    {
        gameObject.SetActive(false);
        // Battle initialize
        // FightManager.Instance.ChangeType(FightType.Init);
        // GameAPP.setting = GameObject.FindGameObjectWithTag("setting");
        GameAPP.setting.SetActive(true);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // FightManager.Instance.ChangeType(FightType.Init);

    }

    public void GameQuit()
    {
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void EasyLevel()
    {
        isEasy = true;
        Debug.Log(isEasy);
    }

    public void DifficultLevel()
    {
        isEasy = false;
        Debug.Log(isEasy);
    }

    public void BgmVolume()
    {
        AudioManager.Instance.bgmSourceVolume(bgmSlider.value);
    }

    public void BgmMute()
    {
        AudioManager.Instance.toggleBgmSource();
    }

    public void EffectVolume()
    {
        AudioManager.Instance.effectSoundVolume(effectSlider.value);
    }

    public void EffectMute()
    {
        AudioManager.Instance.toggleEffect();
    }

    public void isGamePaused()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0.0f;
            UIManager.Instance.GetUI<FightUI>("FightUI").gameObject.SetActive(false);// InvisibleAllCards();
        } else
        {
            Time.timeScale = 1.0f;
            UIManager.Instance.GetUI<FightUI>("FightUI").gameObject.SetActive(true);//VisibleAllCards();
        }
    }

    public void backToMenu()
    {
        FightManager.Instance.StopAllCoroutines();
        // UIManager.Instance.GetUI<FightUI>("FightUI").RemoveAllCards();
        // FightCardManager.Instance.Init();
        // UIManager.Instance.DeleteAllActionIcons();
        foreach (GameObject hps in GameObject.FindGameObjectsWithTag("HpItem"))
        {
            Destroy(hps);
        }
        foreach (GameObject act in GameObject.FindGameObjectsWithTag("actionIcon"))
        {
            Destroy(act);
        }
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(e);
        }
        UIManager.Instance.CloseAll();
        UIManager.Instance.ShowUI<LoginUI>("LoginUI");
        AudioManager.Instance.PlayBGM("bgm1", true);
    }

    public void enableSetting()
    {
        GameObject.FindGameObjectWithTag("setting").gameObject.SetActive(true);
    }
}
