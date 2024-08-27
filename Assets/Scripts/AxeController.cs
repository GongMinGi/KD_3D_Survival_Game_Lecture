using System.Collections;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    //���� Ȱ��ȭ ����
    public static bool isActivate = false;

    private void Start()
    {
        thePlayerController = FindAnyObjectByType<PlayerController>();

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
                if(hitInfo.transform.tag == "Grass")
                {
                    hitInfo.transform.GetComponent<Grass>().Damage();
                }
                else if (hitInfo.transform.tag == "Tree")
                {
                    //hitinfo.point:�����ɽ�Ʈ�� �ε��� ���� ���� ���� ���� ��ǥ
                    hitInfo.transform.GetComponent<TreeComponent>().Chop(hitInfo.point, transform.eulerAngles.y);
                }


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
