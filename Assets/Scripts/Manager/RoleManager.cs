using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//�û���Ϣ��������ӵ�еĿ��Ƶ���Ϣ��ҵȣ�
public class RoleManager
{
    public static RoleManager Instance = new RoleManager();
    public List<string> cardList;//�洢ӵ�еĿ��Ƶ�id
    public void Init()
    {
        cardList = new List<string>();
        //���Ź����� ���ŷ����� ����Ч����
        cardList.Add("1000");
        cardList.Add("1000");
        cardList.Add("1000");
        cardList.Add("1000");

        cardList.Add("1000");
        cardList.Add("1000");
        cardList.Add("1000");
        cardList.Add("1000");

        cardList.Add("1001");
        cardList.Add("1001");
        cardList.Add("1001");
        cardList.Add("1001");

        cardList.Add("1001");
        cardList.Add("1001");
        cardList.Add("1001");
        cardList.Add("1001");

        cardList.Add("1002");
        cardList.Add("1002");
        cardList.Add("1002");
        cardList.Add("1002");
    }
}
