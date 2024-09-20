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

    void Start()
    {
        originPos = transform.position;
        //���������� �� �͵��� serializeField�� �ڱ� �ڽ� �ȿ� �ִ� ��ü�鸸 ������ �� �ִ�.
        //�����Ŵ����� �����տ� ���ԵǾ����� �����Ƿ� �� ��ã�´�. ���� findobjectbytype���� ã����� ��.
        // �̰� instantiate �� ������ �����տ� �ش��ϴ� �̾߱�, ���̾��Ű�� �����ִ� �����յ��� serializedfield�� �� ã�´�.
        theWeaponManager = FindAnyObjectByType<WeaponManager>();
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
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
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
