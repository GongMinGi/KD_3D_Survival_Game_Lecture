using System.Collections;
using UnityEngine;


//�̿ϼ��� �߻� �ڷ�ƾ�� ������ �����Ƿ� Ŭ���� ���� �̿ϼ��� abstract Ŭ������ �Ǿ�� �Ѵ�.
//�߻� Ŭ������ �̿ϼ��� Ŭ���� �̱� ������ ������Ʈ�� �߰��� �� ���� ���� Update�Լ��� ����� �� ����.
public abstract class CloseWeaponController : MonoBehaviour
{



    //���� ������ HAnd �� Ÿ�� ���� 
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    protected bool isAttack = false;
    protected bool isSwing = false; //���� �ֵθ��� �ִ��� �ƴ���

    //�������� ���� ���� ������Ʈ�� ������ �ش� ������ ���� �ȴ�. 
    protected RaycastHit hitInfo;



    protected void TryAttack()
    {
        //Fire1���� ���콺 ��Ŭ�� ���� leftControl�� �Ҵ�Ǿ� �ֱ� ������ edit-projectSetting-inputmanager���� �����ش�. 
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                //�ڷ�ƾ ����
                StartCoroutine(AttackCoroutine());
            }
        }
    }


    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;

        //currentHand�� �ִ� animator�� Attack�̶�� Ʈ���Ÿ� �ߵ���Ų��
        //anim �� Hand class���� �����ߴ�.
        currentCloseWeapon.anim.SetTrigger("Attack");

        //���� ���� �� �ɸ��� �ð��� ���� ������ ���� 
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        //���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        //���� ���� �� �ɸ��� �ð��� ���� ������ ����
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        //���� ���� ���� ���� ������ ���� ������ �ð� 
        //��ü attackdelay���� ������ a,b�� ���ָ� �ȴ�. 
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);

        isAttack = false;
    }


    //HitCoroutine�� ��ӹ޴� �ڽ� Ŭ������ ���� ������ ���� ������ �޶����� �ϹǷ� 
    // abstract �� �̿��Ͽ� �߻� �ڷ�ƾ���� ����� �ڽ� Ŭ�������� �����ϰ� �ؾ� �Ѵ�.
    protected abstract IEnumerator HitCoroutine();



    protected bool CheckObject()
    {
        //raycast�� �̿��� �浹ü�� ��ȯ�ϰ� �ش� ������Ʈ�� ������ �����ϰ� �� Ʈ�縦 �����ϴ� ��� 
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            Debug.Log("Swign");

            return true;
        }


        return false;
    }

    //���� �Լ�(�ϼ��� �Լ� ������, �߰� ������ ������ �Լ�)
    //�ش� �Լ��� �ڽ� Ŭ�������� ȣ���ؼ� �߰������� ������ �����ϴ�
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
