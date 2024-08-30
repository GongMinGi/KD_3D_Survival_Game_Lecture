using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //아이템 습득 가능한 최대 거리 

    private bool pickupActivated = false; //습득 가능할 시 true

    private RaycastHit hitInfo; //충돌체 정보 저장.

    // 아이템 레이어에 대해서만 반응 시키기 위해서 만듬, 레이케스트가 아이템이 아닌 다른거에 충돌하면 안됀다는 뜻
    [SerializeField]
    private LayerMask layerMask; 

    //필요한 컴포넌트
    [SerializeField]
    private Text actionText;

    //e키가 눌리면 아이템을 습득하게 하기 위해 사용 
    void Update()
    {
        CheckItem(); //매 프레임마다 아이템이 있는지 체크
        TryAction();
    }


    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //아이템이 있는지 없는지 확인
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null) //오류 방지 
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득했습니다");
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        //transform.TransformDirection(Vector3.forward): transform.foward와 같은 의미 월드 좌표를 로컬 좌표로 바꿔주는 역할을 한다.
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            //E키는 아이템 습득 이외에도 요리, 건설 등 각종 상호작용에 사용되어야 하기 때문에 레이케스트로 체크한 오브젝트의 태그가 아이템인지도 판별해야 한다.
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
            InfoDisappear();
    }


    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득" + "<color=yellow>" + "(E)" + "</color>";

    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
