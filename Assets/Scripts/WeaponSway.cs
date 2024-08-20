using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class WeaponSway : MonoBehaviour
{

    //기존 위치
    private Vector3 originPos;

    //현재 위치
    private Vector3 currentPos;

    //sway 한계 
    [SerializeField]
    private Vector3 limitPos;

    //정조준 sway 한계
    [SerializeField]
    private Vector3 fineSightLimitPos;

    //움직임 부드러운 정도
    [SerializeField]
    private Vector3 smoothSway;

    //필요한 컴포넌트
    [SerializeField]
    private GunController theController;

    void Start()
    {
        originPos = this.transform.localPosition; //이 스크립트가 붙어있는 오브젝트의 로컬 포지션 으로 기존 위치 정하기 ( 현재 자신의 위치값)
    }


    void Update()
    {
        TrySway();
    }

    private void TrySway()
    {
        // 마우스 상하좌우가 움직였을 떄 swaying을 발동하고, 그렇지 않으면 원래 위치로 돌아온다.
        if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) 
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }

    //마우스가 움직이는 방향대로 총기가 천천히 따라감
    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");
        //총기의 흔들림을 구현해야하는 것이므로 마우스를 움직일 때 총기가 마우스에 끌려오는 느낌을 내야 한다. 
        //그러러면 마우스가 움직일 때 마우스 움직임의 반대방향으로 총기를 이동시키고, 마우스의 움직임이 없을 때 다시 원래의 자리로
        //돌아오게 만들어야한다 그래서 _moveX에 -가 붙어야 하는 것이다. 
        if(!theController.isFineSightMode)// 정조준 상태가 아닐 때의 흔들림
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -limitPos.y, limitPos.y),
                originPos.z);
        }
        else // 정조준 상태에서의 흔들림
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -fineSightLimitPos.x, fineSightLimitPos.x),
                Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                originPos.z);
        }

        transform.localPosition = currentPos;
    }

    // 마우스가 멈추면 총기가 원위치함.
    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;

    }



}
