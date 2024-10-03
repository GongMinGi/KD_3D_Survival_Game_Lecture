using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//�������̽��� ���� ����� �����ϴ�.
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector3 originPos;

    public Item item; // ȹ���� ������
    public int itemCount; //ȹ���� �������� ����
    public Image itemImage; //�������� �̹���


    //�ʿ��� ������Ʈ
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    private WeaponManager theWeaponManager;
    private Rect baseRect; //x,y �ʺ� ���� ��ǥ�� �����ϴ� Ÿ��, �簢���� �����ϴ� Ÿ��
    private InputNumber theInputNumber;

    void Start()
    {
        // �κ��丮 �ڽ��� ũ�⸦ baseRect�� �޾ƿ;� �ϹǷ� ������ �θ��� �θ��� rect�� �����Ѵ�. 
        baseRect = transform.parent.parent.GetComponent<RectTransform>().rect;
        originPos = transform.position;
        //���������� �� �͵��� serializeField�� �ڱ� �ڽ� �ȿ� �ִ� ��ü�鸸 ������ �� �ִ�.
        //�����Ŵ����� �����տ� ���ԵǾ����� �����Ƿ� �� ��ã�´�. ���� findobjectbytype���� ã����� ��.
        // �̰� instantiate �� ������ �����տ� �ش��ϴ� �̾߱�, ���̾��Ű�� �����ִ� �����յ��� serializedfield�� �� ã�´�.
        theWeaponManager = FindAnyObjectByType<WeaponManager>();
        theInputNumber = FindAnyObjectByType<InputNumber>();
    }    


    //�̹����� ���� ����
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color; 
    }

    //������ ȹ��
    public void AddItem(Item _item, int _count = 1)// ������ ȹ�� ������ �⺻���� 1
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equipment) // ���� ���� �������� �����Ƿ� �������� Ÿ���� ��� �ƴ� ���� ������ ǥ���Ѵ�.
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false); //��Ȱ��ȭ�Ǹ� �ڽİ�ü�� ���� �ٲ� �� ���� ������ ���߿� �������ִ� ��.
        }

        SetColor(1);
    }

    //������ ���� ����
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    //���� �ʱ�ȭ. ������ ������ 0�� �Ǹ� ����
    private void ClearSlot()
    {
        itemCount = 0;
        SetColor(0);
        text_Count.text = "0";
        go_CountImage.SetActive(false);
        itemImage.sprite = null;
        item = null;

    }

    //abstract�� ���������� interface ���� �����⸸ ���� �� �ִ�.
    public void OnPointerClick(PointerEventData eventData)
    {
        //�ش� ��ũ��Ʈ�� ���� ��ü�� ��Ŭ���� �ϸ� �̺�Ʈ ���� 
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null)
            {
                if(item.itemType == Item.ItemType.Equipment)
                {
                    //����
                    StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                }
                else
                {
                    //�Ҹ�
                    Debug.Log(item.itemName + " �� ����߽��ϴ�.");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            //������ ��ġ�� �̺�Ʈ�� �߻��� ��ġ�� �̵���Ų�� ( ���콺��ġ)
            DragSlot.instance.transform.position = eventData.position;
        }
      
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    //onEndDrag, Ondrop�� �������� EndDrag�� �巡�װ� ���� ������ ������ ȣ�������, OnDrop�� �巡�װ� ���� ��ġ�� 
    // �ڽ��� ������ �ٸ� ���� �����߸� ȣ��ȴ�

    public void OnEndDrag(PointerEventData eventData)
    {

        Debug.Log("OnEndDragȣ���");
        if(DragSlot.instance.transform.localPosition.x < baseRect.xMin 
            || DragSlot.instance.transform.localPosition.x > baseRect.xMax
            || DragSlot.instance.transform.localPosition.y <baseRect.yMin
            || DragSlot.instance.transform.localPosition.y > baseRect.yMax)
        {
            Debug.Log("�κ��丮 ������ �����");
            //// theWeaponManager�� �÷��̾� ������Ʈ�� �پ� �����Ƿ�, �÷��̾��� ��ġ + �ణ forward �ʿ� �����ǰ� �ȴ�. Quaternion�� identity�� ���������Ƿ� ȸ������ ����.
            //Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, theWeaponManager.transform.position + theWeaponManager.transform.forward, Quaternion.identity);
            //DragSlot.instance.dragSlot.ClearSlot(); //�������� �����Ѵ����� �������� �ϹǷ� ClearSlot���� �����ش�.
            if(DragSlot.instance.dragSlot != null)
            {

                theInputNumber.Call();
            }
        }
        else
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDropȣ���");
        if(DragSlot.instance.dragSlot != null) // �巡�� �ϰ� �մ� ������ ���� ���� ������ ��ȯ�Ѵ�.(��������)
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        Item _tempItem = item; //���� ���Կ� �ִ� �����۵������� 
        int _tempItemCount = itemCount;

        //drag���Կ� �ִ� �����۵��� ���Կ� �ִ´�.
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if(_tempItem != null)//�ű���� ���Կ� �������� �ִٸ�, temp�� �����ߴ� �������� �Ű��ְ�, ���� ��ĭ�̾��ٸ� �׳� �����ڸ��� clear gownsek. 
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}
