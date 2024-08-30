using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; //������ ���� ������ �ִ� �Ÿ� 

    private bool pickupActivated = false; //���� ������ �� true

    private RaycastHit hitInfo; //�浹ü ���� ����.

    // ������ ���̾ ���ؼ��� ���� ��Ű�� ���ؼ� ����, �����ɽ�Ʈ�� �������� �ƴ� �ٸ��ſ� �浹�ϸ� �ȉ´ٴ� ��
    [SerializeField]
    private LayerMask layerMask; 

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Text actionText;

    //eŰ�� ������ �������� �����ϰ� �ϱ� ���� ��� 
    void Update()
    {
        CheckItem(); //�� �����Ӹ��� �������� �ִ��� üũ
        TryAction();
    }


    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //�������� �ִ��� ������ Ȯ��
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null) //���� ���� 
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "ȹ���߽��ϴ�");
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        //transform.TransformDirection(Vector3.forward): transform.foward�� ���� �ǹ� ���� ��ǥ�� ���� ��ǥ�� �ٲ��ִ� ������ �Ѵ�.
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            //EŰ�� ������ ���� �̿ܿ��� �丮, �Ǽ� �� ���� ��ȣ�ۿ뿡 ���Ǿ�� �ϱ� ������ �����ɽ�Ʈ�� üũ�� ������Ʈ�� �±װ� ������������ �Ǻ��ؾ� �Ѵ�.
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
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + "ȹ��" + "<color=yellow>" + "(E)" + "</color>";

    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
