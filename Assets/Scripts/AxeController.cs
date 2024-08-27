using System.Collections;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    //현재 활성화 여부
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

        //isSwing이 false가 될 때까지 반복 
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
                    //hitinfo.point:레이케스트로 부딪힌 곳의 실제 월드 상의 좌표
                    hitInfo.transform.GetComponent<TreeComponent>().Chop(hitInfo.point, transform.eulerAngles.y);
                }


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
