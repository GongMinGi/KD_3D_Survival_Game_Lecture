using UnityEngine;

public class Inventory : MonoBehaviour
{
    // true 가되면 공격이나 카메라움직임등을 막을 것이다.
    //또한 i버튼을 누르면 인벤토리가 켜지고 꺼지게 할 것
    public static bool inventoryActivated = false;

    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    //슬롯들
    private Slot[] slots;

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>(); //go_SlotParent 의 자식으로 있는 슬롯들이 모두 배열에 들어감
    }


    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if(inventoryActivated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }
    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }


    public void AcquireItem(Item _item, int _count = 1)
    {
        //아이템의 타입이 장비가 아닐 때만 있는지 검사한다.
        if(Item.ItemType.Equipment != _item.itemType)
        {
            //인벤토리 내부에 아이템 이름이 일치하는 슬롯이 있다면 ( 아이템을 이미 가지고 있다면)
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null) //슬롯의 아이템의 null 이 아닐 때만 비교하도록 한다 안그럼 nullReference 오류 뜬다.
                {

                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count); // 아이템의 개수를 증가(혹은 감소) 시킨다
                        return;
                    }
                }

            }
        }


        //확인해봤는데 아이템이 없다면 
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) //빈칸을 찾아서 
            {
                slots[i].AddItem(_item, _count); //아이템을 추가한다.
                return;
            }
        }
    }
}
