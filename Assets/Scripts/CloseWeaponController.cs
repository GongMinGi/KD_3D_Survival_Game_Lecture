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


    //�ʿ��� ������Ʈ
    protected PlayerController thePlayerController;



    protected void TryAttack()
    {
        //Fire1���� ���콺 ��Ŭ�� ���� leftControl�� �Ҵ�Ǿ� �ֱ� ������ edit-projectSetting-inputmanager���� �����ش�. 
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                if(CheckObject()) //�����ɽ�Ʈ�� ������ ��ü�� �ִ��� Ȯ��
                {
                    if(currentCloseWeapon.isAxe && hitInfo.transform.tag == "Tree") // ���� ������ ��� �ְ�, ��ǥ�� ������Ʈ�� �±װ� Tree���� Ȯ��
                    {
                        //�����ɽ�Ʈ�� �� Tree�� treecomponent���� GetTreeCenterPosition�� ���� �߾������� ��ġ�� �Ű������� ������.
                        StartCoroutine(thePlayerController.TreeLookCoroutine(hitInfo.transform.GetComponent<TreeComponent>().GetTreeCenterPosition()));
                        //�׷��ٸ� �Ϲ����� ������ �ƴ϶� Chop�ִϸ��̼��� ����
                        StartCoroutine(AttackCoroutine("Chop", currentCloseWeapon.workDelayA, currentCloseWeapon.workDelayB, currentCloseWeapon.workDelay));
                        return;
                    }
                }


                //�ڷ�ƾ ����
                StartCoroutine(AttackCoroutine("Attack", currentCloseWeapon.attackDelayA, currentCloseWeapon.attackDelayB, currentCloseWeapon.attackDelay));
            }
        }
    }

    //�������� ������ ���� �����̰� �� �ٸ��� ���� ������ ������ ���� �����̰� �޶��� ���� �����Ƿ� �μ��� �޾Ƽ� �����Ѵ�.
    protected IEnumerator AttackCoroutine(string swingType, float _delayA, float _delayB, float _delayC)
    {
        isAttack = true;

        //currentHand�� �ִ� animator�� Attack�̶�� Ʈ���Ÿ� �ߵ���Ų��
        //anim �� Hand class���� �����ߴ�.
        currentCloseWeapon.anim.SetTrigger(swingType);

        //���� ���� �� �ɸ��� �ð��� ���� ������ ���� 
        yield return new WaitForSeconds(_delayA);
        isSwing = true;

        //���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        //���� ���� �� �ɸ��� �ð��� ���� ������ ����
        yield return new WaitForSeconds(_delayB);
        isSwing = false;

        //���� ���� ���� ���� ������ ���� ������ �ð� 
        //��ü attackdelay���� ������ a,b�� ���ָ� �ȴ�. 
        yield return new WaitForSeconds(_delayC - _delayA - _delayB);

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
            //Debug.Log(hitInfo);

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
