using System.Collections;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    //현재 활성화 여부
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

    //override를 통해서 부모클래스의 가상함수를 추가적으로 수정할 수 있다
    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon); //부모클래스의 virtual 함수 전문을 가져온다 
        isActivate = true; //뒤에 추가적으로 붙이고 싶은 내용을 붙일 수 있다
    }
}
