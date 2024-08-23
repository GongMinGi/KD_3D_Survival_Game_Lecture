using System.Collections;
using UnityEngine;


//미완성된 추상 코루틴을 가지고 있으므로 클래스 또한 미완성인 abstract 클래스가 되어야 한다.
//추상 클래스는 미완성이 클래스 이기 때문에 컴포넌트로 추가할 수 없고 따라서 Update함수가 실행될 수 없다.
public abstract class CloseWeaponController : MonoBehaviour
{



    //현재 장착된 HAnd 형 타입 무기 
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    protected bool isAttack = false;
    protected bool isSwing = false; //팔을 휘두르고 있는지 아닌지

    //레이저를 쏴서 닿은 오브젝트의 정보가 해당 변수에 담기게 된다. 
    protected RaycastHit hitInfo;



    protected void TryAttack()
    {
        //Fire1에는 마우스 좌클릭 말고도 leftControl이 할당되어 있기 때문에 edit-projectSetting-inputmanager에서 지워준다. 
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                //코루틴 실행
                StartCoroutine(AttackCoroutine());
            }
        }
    }


    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;

        //currentHand에 있는 animator의 Attack이라는 트리거를 발동시킨다
        //anim 은 Hand class에서 정의했다.
        currentCloseWeapon.anim.SetTrigger("Attack");

        //팔을 뻗는 대 걸리는 시간에 대한 딜레이 적용 
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        //공격 활성화 시점
        StartCoroutine(HitCoroutine());

        //팔을 접는 대 걸리는 시간에 대한 딜레이 적용
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        //팔을 접은 이후 다음 공격을 위한 딜레이 시간 
        //전체 attackdelay에서 딜레이 a,b를 빼주면 된다. 
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);

        isAttack = false;
    }


    //HitCoroutine은 상속받는 자식 클래스의 무기 종류의 따라서 역할이 달라져야 하므로 
    // abstract 를 이용하여 추상 코루틴으로 만들고 자식 클래스에서 구현하게 해야 한다.
    protected abstract IEnumerator HitCoroutine();



    protected bool CheckObject()
    {
        //raycast를 이용해 충돌체를 반환하고 해당 오브젝트에 공격을 적용하게 끔 트루를 리턴하는 방식 
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            Debug.Log("Swign");

            return true;
        }


        return false;
    }

    //가상 함수(완성된 함수 이지만, 추가 편집이 가능한 함수)
    //해당 함수는 자식 클래스에서 호출해서 추가적으로 수정이 가능하다
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);

    }
}
