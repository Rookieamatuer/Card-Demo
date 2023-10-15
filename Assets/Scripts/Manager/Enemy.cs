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

        //TODO:����
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

    //��������ѡ�У���ʾ���
    public void OnSelect()
    {
        _meshRenderer.material.SetColor("_OtlColor", Color.red);
    }

    //δѡ��
    public void OnUnSelect()
    {
        _meshRenderer.material.SetColor("_OtlColor", Color.black);
    }

    //����
    public void Hit(int val)
    {
        //�ȿۻ���
        if (Defend > val)
        {
            //�ۻ���
            Defend -= val;
            //��������
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
                // ��������
                ani.Play("die");
                //���˴��б����Ƴ�
                EnemyManeger.Instance.DeleteEnemy(this);
                Destroy(gameObject, 1);
                Destroy(actionObj);
                Destroy(hpItemObj);
            }
            else
            {
                //����
                ani.Play("hit", 0, 0);
            }
        }
        //ˢ��Ѫ����ui
        UpdateDefend();
        UpdateHp();
    }

    //���ع���ͷ�ϵ��ж���־
    public void HideAction()
    {
        attackTf.gameObject.SetActive(false);
        defendTf.gameObject.SetActive(false);
    }

    //ִ�е����ж�
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
                // �ӷ���
                Defend += 1;
                UpdateDefend();
                //���Բ��Ŷ�Ӧ����Ч
                break;
            case ActionType.Attack:
                //���Ŷ�Ӧ�Ķ���
                ani.Play("attack");
                //�ȴ�ĳһʱ��ĺ�ִ�ж�Ӧ����Ϊ��Ҳ�������õ�excel��
                yield return new WaitForSeconds(0.5f);//������д����
                //��������Զ�һ��
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                // ��ҿ�Ѫ
                FightManager.Instance.GetPlayerHit(Attack);
                break;
        }
        //�ȴ����������꣨�����ʱ��Ҳ�������ã�
        yield return new WaitForSeconds(1);
        //���Ŵ���
        ani.Play("idle");
    }

}
