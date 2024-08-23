using System.Collections;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    //���� Ȱ��ȭ ����
    public static bool isActivate = false;

    private void Start()
    {

    }

    void Update()
    {
        if (isActivate)
        {
            TryAttack();
        }
    }

    protected override IEnumerator HitCoroutine()
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

    //override�� ���ؼ� �θ�Ŭ������ �����Լ��� �߰������� ������ �� �ִ�
    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon); //�θ�Ŭ������ virtual �Լ� ������ �����´� 
        isActivate = true; //�ڿ� �߰������� ���̰� ���� ������ ���� �� �ִ�
    }
}
