using DG.Tweening;
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

    public Animator ani;

    SkinnedMeshRenderer _meshRenderer;

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

        _meshRenderer = transform.GetComponentInChildren<SkinnedMeshRenderer>();

        ani = transform.GetComponent<Animator>();

        UpdateHp();
        UpdateDefend();

        //TODO:测试
        // OnSelect();
    }

    // Set a random action
    public void SetRandomAction()
    {
        int rand = Random.Range(1, 3);
        type = (ActionType)rand;
        Debug.Log(type);
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

    //被攻击卡选中，显示红边
    public void OnSelect()
    {
        _meshRenderer.material.SetColor("_OtlColor", Color.red);
    }

    //未选中
    public void OnUnSelect()
    {
        _meshRenderer.material.SetColor("_OtlColor", Color.black);
    }

    //受伤
    public void Hit(int val)
    {
        //先扣护盾
        if (Defend > val)
        {
            //扣护盾
            Defend -= val;
            //播放受伤
            ani.Play("hit", 0, 0);
        }
        else
        {
            val = val - Defend;
            Defend = 0;
            CurHp -= val;
            if (CurHp <= 0)
            {
                CurHp = 0;
                // 播放死亡
                ani.Play("die");
                //敌人从列表中移除
                EnemyManeger.Instance.DeleteEnemy(this);
                Destroy(gameObject, 1);
                Destroy(actionObj);
                Destroy(hpItemObj);
            }
            else
            {
                //受伤
                ani.Play("hit", 0, 0);
            }
        }
        //刷新血量等ui
        UpdateDefend();
        UpdateHp();
    }

    //隐藏怪物头上的行动标志
    public void HideAction()
    {
        attackTf.gameObject.SetActive(false);
        defendTf.gameObject.SetActive(false);
    }

    //执行敌人行动
    public IEnumerator DoAction()
    {
        HideAction();
        Debug.Log("DoAction " + type);
        // ActionType type = (ActionType)(Random.Range(1, 2));
        switch (type)
        {
            case ActionType.None:
                break;
            case ActionType.Defend:
                // 加防御
                Defend += 1;
                UpdateDefend();
                //可以播放对应的特效
                break;
            case ActionType.Attack:
                //播放对应的动画
                ani.Play("attack");
                //等待某一时间的后执行对应的行为（也可以配置到excel表）
                yield return new WaitForSeconds(0.5f);//这里我写死了
                //摄像机可以抖一抖
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                // 玩家扣血
                FightManager.Instance.GetPlayerHit(Attack);
                break;
        }
        //等待动画播放完（这里的时长也可以配置）
        yield return new WaitForSeconds(1);
        //播放待机
        ani.Play("idle");
    }

}
