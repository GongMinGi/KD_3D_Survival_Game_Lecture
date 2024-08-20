using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //현재 활성화 여부
    public static bool isActivate = false;


    //현재 장착된 HAnd 형 타입 무기 
    [SerializeField]
    private Hand currentHand;

    private bool isAttack = false;
    private bool isSwing = false; //팔을 휘두르고 있는지 아닌지

    //레이저를 쏴서 닿은 오브젝트의 정보가 해당 변수에 담기게 된다. 
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
        //Fire1에는 마우스 좌클릭 말고도 leftControl이 할당되어 있기 때문에 edit-projectSetting-inputmanager에서 지워준다. 
        if (Input.GetButton("Fire1"))
        {
            if(!isAttack)
            {
                //코루틴 실행
                StartCoroutine(AttackCoroutine());
            }
        }
    }


    IEnumerator AttackCoroutine()
    {
        isAttack = true;

        //currentHand에 있는 animator의 Attack이라는 트리거를 발동시킨다
        //anim 은 Hand class에서 정의했다.
        currentHand.anim.SetTrigger("Attack");

        //팔을 뻗는 대 걸리는 시간에 대한 딜레이 적용 
        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;
   
        //공격 활성화 시점
        StartCoroutine(HitCoroutine());

        //팔을 접는 대 걸리는 시간에 대한 딜레이 적용
        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        //팔을 접은 이후 다음 공격을 위한 딜레이 시간 
        //전체 attackdelay에서 딜레이 a,b를 빼주면 된다. 
        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);

        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {

        //isSwing이 false가 될 때까지 반복 
        while (isSwing)
        {

            if (CheckObject())
            {
                //충돌됨 
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            //while문 하나 넣을 동안 1프레임씩 대기
            yield return null;
        }
    }


    private bool CheckObject()
    {
        //raycast를 이용해 충돌체를 반환하고 해당 오브젝트에 공격을 적용하게 끔 트루를 리턴하는 방식 
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
