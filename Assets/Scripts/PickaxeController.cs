using System.Collections;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    //���� Ȱ��ȭ ����
    public static bool isActivate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
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
