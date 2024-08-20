using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //���� Ȱ��ȭ ����
    public static bool isActivate = false;


    //���� ������ HAnd �� Ÿ�� ���� 
    [SerializeField]
    private Hand currentHand;

    private bool isAttack = false;
    private bool isSwing = false; //���� �ֵθ��� �ִ��� �ƴ���

    //�������� ���� ���� ������Ʈ�� ������ �ش� ������ ���� �ȴ�. 
    private RaycastHit hitInfo;





    void Update()
    {
        if(isActivate)
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        //Fire1���� ���콺 ��Ŭ�� ���� leftControl�� �Ҵ�Ǿ� �ֱ� ������ edit-projectSetting-inputmanager���� �����ش�. 
        if (Input.GetButton("Fire1"))
        {
            if(!isAttack)
            {
                //�ڷ�ƾ ����
                StartCoroutine(AttackCoroutine());
            }
        }
    }


    IEnumerator AttackCoroutine()
    {
        isAttack = true;

        //currentHand�� �ִ� animator�� Attack�̶�� Ʈ���Ÿ� �ߵ���Ų��
        //anim �� Hand class���� �����ߴ�.
        currentHand.anim.SetTrigger("Attack");

        //���� ���� �� �ɸ��� �ð��� ���� ������ ���� 
        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;
   
        //���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        //���� ���� �� �ɸ��� �ð��� ���� ������ ����
        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        //���� ���� ���� ���� ������ ���� ������ �ð� 
        //��ü attackdelay���� ������ a,b�� ���ָ� �ȴ�. 
        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);

        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {

        //isSwing�� false�� �� ������ �ݺ� 
        while (isSwing)
        {

            if (CheckObject())
            {
                //�浹�� 
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            //while�� �ϳ� ���� ���� 1�����Ӿ� ���
            yield return null;
        }
    }


    private bool CheckObject()
    {
        //raycast�� �̿��� �浹ü�� ��ȯ�ϰ� �ش� ������Ʈ�� ������ �����ϰ� �� Ʈ�縦 �����ϴ� ��� 
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            Debug.Log("Swign");

            return true;
        }


        return false;
    }

    public void HandChange(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentHand = _hand;
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero;
        currentHand.gameObject.SetActive(true);
        isActivate = true;
    }
}
