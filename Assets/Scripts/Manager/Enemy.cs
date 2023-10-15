using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType
{
    None,
    Defend,
    Attack,
}

// Enemy script
public class Enemy : MonoBehaviour
{
    protected Dictionary<string, string> data;  // Enemy data
    public ActionType type;

    public GameObject hpItemObj;
    public GameObject actionObj;

    // UI
    public Transform attackTf;
    public Transform defendTf;
    public Text defendTxt;
    public Text hpTxt;
    public Image hpImg;

    // Status
    public int Defend;
    public int Attack;
    public int MaxHp;
    public int CurHp;
    public void Init(Dictionary<string, string> data)
    {
        this.data = data;
    }

    void Start()
    {
        type = ActionType.None;
        hpItemObj = UIManager.Instance.CreateHpItem();
        actionObj = UIManager.Instance.CreateActionIcon();
        attackTf = actionObj.transform.Find("attack");
        defendTf = actionObj.transform.Find("defend");
        defendTxt = hpItemObj.transform.Find("shield/Text").GetComponent<Text>();
        hpTxt = hpItemObj.transform.Find("healthText").GetComponent<Text>();
        hpImg = hpItemObj.transform.Find("fill").GetComponent<Image>();
        // Set health bar
        hpItemObj.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.down * 0.2f);
        actionObj.transform.position = Camera.main.WorldToScreenPoint(transform.Find("head").position);
        SetRandomAction();
        // Initialize status
        Attack = int.Parse(data["Attack"]);
        CurHp = int.Parse(data["Hp"]);
        MaxHp = CurHp;
        Defend = int.Parse(data["Defend"]);

        UpdateHp();
        UpdateDefend();
    }

    // Set a random action
    void SetRandomAction()
    {
        int rand = Random.Range(0, 2);
        type = (ActionType)rand;
        switch (type)
        {
            case ActionType.None:
                break;
            case ActionType.Defend:
                attackTf.gameObject.SetActive(false);
                defendTf.gameObject.SetActive(true);
                break;
            case ActionType.Attack:
                attackTf.gameObject.SetActive(true);
                defendTf.gameObject.SetActive(false);
                break;
        }
    }

    // Update Health
    public void UpdateHp()
    {
        hpTxt.text = CurHp + "/" + MaxHp;
        hpImg.fillAmount = (float)CurHp / (float)MaxHp;
    }

    // Update shield
    public void UpdateDefend()
    {
        defendTxt.text = Defend.ToString();
    }
}
