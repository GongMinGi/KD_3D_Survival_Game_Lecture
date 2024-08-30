using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    public Item item; // 획득한 아이템
    public int itemCount; //획득한 아이템의 개수
    public Image itemImage; //아이템의 이미지


    //필요한 컴포넌트
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;


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
        item = null;
        itemCount = 0;
        itemImage = null;
        SetColor(0);
        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

}
