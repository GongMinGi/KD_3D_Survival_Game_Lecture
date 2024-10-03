using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    private bool activiated;

    [SerializeField]
    private Text text_Preview;
    [SerializeField]
    private Text text_Input;
    [SerializeField]
    private InputField if_text;

    [SerializeField]
    private GameObject go_Base;


    [SerializeField]
    private ActionController thePlayer;

    void Update()
    {
        if (activiated)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OK();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cancel();
            }
        }

    }

    public void Call()
    {
        go_Base.SetActive(true);
        activiated = true;
        if_text.text = ""; //ȣ���Ҷ����� InputField�� �ؽ�Ʈ�� �ʱ�ȭ��Ŵ
        text_Preview.text = DragSlot.instance.dragSlot.itemCount.ToString(); //�Լ��� �����ϸ� textpreview�� �������� �ִ밳���� �����ش�.
    }

    public void Cancel()
    {
        activiated = false;
        go_Base.SetActive(false);
        DragSlot.instance.SetColor(0); //��Ҹ� ������ ������ �巡�׽����� ����ȭ�ǰ� null�� �� �� �ֵ��� ó��
        DragSlot.instance.dragSlot = null;
    }

    public void OK()
    {
        DragSlot.instance.SetColor(0);
        //int�� ĳ���ͷ� ���� ����ȯ 
        int num;
        if (text_Input.text != "") // ������ �ƴϾ�� �Ѵ�. ���� �������� ������ ������ �ȉ´ٴ� ��
        {
            if (CheckNumber(text_Input.text))// �ؽ�Ʈ�� ���ڰ� �ƴ϶�� �̻��� ���� ������ ������ �������� ���� Ȯ���Ѵ�.
            {
                num = int.Parse(text_Input.text); //int�� ����ȯ
                if (num > DragSlot.instance.dragSlot.itemCount)
                    num = DragSlot.instance.dragSlot.itemCount; // �ִ� ������ �������� ���� �������� �ϸ� ���簡���� �ִ� �������� �ִ�ġ�� �����Բ� �����Ѵ�.
            }
            else //���ڰ� �ƴ� ���ڰ� ������ ���
            {
                num = 1;// ���ڰ� �ƴϸ� �׳� �� ���� �����Բ� ����
            }
        }
        else //�ƹ��͵� ���� �ʾ�������
        {
            //text_Preview���� �������� �ִ밳���� ������ ���̴�.
            num = int.Parse(text_Preview.text); //������ �ִ� �ִ밳���� ������.
        }

        StartCoroutine(DropItemCoroutine(num));
    }


    //������� �ڷ�ƾ�� �̿��Ͽ� �ణ�� ��⸦ �ɾ��� ���̴�.
    //�ѹ��� �������� ������ �ݶ��̴��� ��ġ�鼭 ���������ٵ� �ڷ�ƾ�� �̿��ؼ� ������ �ϳ��� ������
    //�׷� ������ ������ �� ���� ��.

    IEnumerator DropItemCoroutine(int _num)
    {


        for (int i = 0; i < _num; i++)
        {
            //�������� ������ ������ �������� �������� �Ҵ�Ǿ� �����븸 �����ϰ� �׷��� ������ �׳� �ı��Ѵ�.
            if(DragSlot.instance.dragSlot.item.itemPrefab != null)
            {
                Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, thePlayer.transform.position + thePlayer.transform.forward, Quaternion.identity);
            }
            DragSlot.instance.dragSlot.SetSlotCount(-1);
            yield return new WaitForSeconds(0.05f); // ������ �ϳ��� 0.05���� �����̷� ������.
        }

        DragSlot.instance.dragSlot = null; //�������� ��� ����ϸ� �巡�� ������ ����ش�.
        go_Base.SetActive(false);
        activiated = false; // ��� ������ ������ Ȱ��ȭ ���� ����
    }

  

    private bool CheckNumber(string _argString)
    {
        //ToCharArray()�� ����ϸ� string�� �ѱ��ھ� �迭�� �־��ش�. 
        char[] _tempCharArray = _argString.ToCharArray();
        bool isNumber = true;// Ȯ���ϱ� ���� �ϴ� ���ڶ�� ����

        for (int i = 0; i < _tempCharArray.Length; i++)
        {
            //�����ڵ忡�� 0�� 0030 �̰�, 16�����̱� ������ 10�������� 48~57������ 0~9�̴�
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57)
            {
                continue; //48~57���̶�� �����̹Ƿ� continue�� ���� ���� �ܾ� ���� 
            }
            isNumber = false;// ���� ���ڰ� �ƴ϶�� isNumber�� false�� �ٲ۴�.

        }

        return isNumber;
    }
}
