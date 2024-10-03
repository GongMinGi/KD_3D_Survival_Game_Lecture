using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//인터페이스는 다중 상속이 가능하다.
public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector3 originPos;

    public Item item; // 획득한 아이템
    public int itemCount; //획득한 아이템의 개수
    public Image itemImage; //아이템의 이미지


    //필요한 컴포넌트
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    private WeaponManager theWeaponManager;
    private Rect baseRect; //x,y 너비 높이 좌표를 정의하는 타입, 사각형을 정의하는 타입
    private InputNumber theInputNumber;

    void Start()
    {
        // 인벤토리 박스의 크기를 baseRect로 받아와야 하므로 슬롯의 부모의 부모의 rect에 접근한다. 
        baseRect = transform.parent.parent.GetComponent<RectTransform>().rect;
        originPos = transform.position;
        //프리팹으로 된 것들은 serializeField가 자기 자신 안에 있는 객체들만 참조할 수 있다.
        //웨폰매니저는 프리팹에 포함되어잇지 않으므로 잘 못찾는다. 따라서 findobjectbytype으로 찾아줘야 함.
        // 이건 instantiate 로 생성된 프리팹에 해당하는 이야기, 하이어아키에 나와있는 프리팹들은 serializedfield가 잘 찾는다.
        theWeaponManager = FindAnyObjectByType<WeaponManager>();
        theInputNumber = FindAnyObjectByType<InputNumber>();
    }    


    //이미지의 투명도 조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color; 
    }

    //아이템 획득
    public void AddItem(Item _item, int _count = 1)// 아이템 획득 개수의 기본값은 1
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equipment) // 보통 장비는 겹쳐지지 않으므로 아이템의 타입이 장비가 아닐 때만 개수를 표시한다.
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false); //비활성화되면 자식객체의 값을 바꿀 수 없기 때문에 나중에 실행해주는 것.
        }

        SetColor(1);
    }

    //아이템 개수 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    //슬롯 초기화. 아이템 개수가 0이 되면 실행
    private void ClearSlot()
    {
        itemCount = 0;
        SetColor(0);
        text_Count.text = "0";
        go_CountImage.SetActive(false);
        itemImage.sprite = null;
        item = null;

    }

    //abstract와 마찬가지로 interface 또한 껍데기만 남길 수 있다.
    public void OnPointerClick(PointerEventData eventData)
    {
        //해당 스크립트를 가진 객체에 우클릭을 하면 이벤트 실행 
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null)
            {
                if(item.itemType == Item.ItemType.Equipment)
                {
                    //장착
                    StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
                }
                else
                {
                    //소모
                    Debug.Log(item.itemName + " 을 사용했습니다.");
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

            //슬롯의 위치를 이벤트가 발생한 위치로 이동시킨다 ( 마우스위치)
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

    //onEndDrag, Ondrop의 차이점은 EndDrag는 드래그가 끝난 시점에 무조건 호출되지만, OnDrop은 드래그가 끝난 위치가 
    // 자신을 제외한 다른 슬롯 위여야만 호출된다

    public void OnEndDrag(PointerEventData eventData)
    {

        Debug.Log("OnEndDrag호출됨");
        if(DragSlot.instance.transform.localPosition.x < baseRect.xMin 
            || DragSlot.instance.transform.localPosition.x > baseRect.xMax
            || DragSlot.instance.transform.localPosition.y <baseRect.yMin
            || DragSlot.instance.transform.localPosition.y > baseRect.yMax)
        {
            Debug.Log("인벤토리 영역을 벗어났음");
            //// theWeaponManager가 플레이어 오브젝트에 붙어 있으므로, 플레이어의 위치 + 약간 forward 쪽에 생성되게 된다. Quaternion은 identity로 설정했으므로 회전값은 없다.
            //Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, theWeaponManager.transform.position + theWeaponManager.transform.forward, Quaternion.identity);
            //DragSlot.instance.dragSlot.ClearSlot(); //아이템을 생성한다음엔 버려져야 하므로 ClearSlot으로 지워준다.
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
        Debug.Log("OnDrop호출됨");
        if(DragSlot.instance.dragSlot != null) // 드래그 하고 잇는 슬롯이 있을 때만 슬롯을 교환한다.(오류방지)
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        Item _tempItem = item; //원래 슬롯에 있던 아이템들을저장 
        int _tempItemCount = itemCount;

        //drag슬롯에 있는 아이템들을 슬롯에 넣는다.
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if(_tempItem != null)//옮기려던 슬롯에 아이템이 있다면, temp에 저장했던 아이템을 옮겨주고, 원래 빈칸이었다면 그냥 원래자리를 clear gownsek. 
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}
