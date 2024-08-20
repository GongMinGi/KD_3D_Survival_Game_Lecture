using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class WeaponSway : MonoBehaviour
{

    //���� ��ġ
    private Vector3 originPos;

    //���� ��ġ
    private Vector3 currentPos;

    //sway �Ѱ� 
    [SerializeField]
    private Vector3 limitPos;

    //������ sway �Ѱ�
    [SerializeField]
    private Vector3 fineSightLimitPos;

    //������ �ε巯�� ����
    [SerializeField]
    private Vector3 smoothSway;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private GunController theController;

    void Start()
    {
        originPos = this.transform.localPosition; //�� ��ũ��Ʈ�� �پ��ִ� ������Ʈ�� ���� ������ ���� ���� ��ġ ���ϱ� ( ���� �ڽ��� ��ġ��)
    }


    void Update()
    {
        TrySway();
    }

    private void TrySway()
    {
        // ���콺 �����¿찡 �������� �� swaying�� �ߵ��ϰ�, �׷��� ������ ���� ��ġ�� ���ƿ´�.
        if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) 
        {
            Swaying();
        }
        else
        {
            BackToOriginPos();
        }
    }

    //���콺�� �����̴� ������ �ѱⰡ õõ�� ����
    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");
        //�ѱ��� ��鸲�� �����ؾ��ϴ� ���̹Ƿ� ���콺�� ������ �� �ѱⰡ ���콺�� �������� ������ ���� �Ѵ�. 
        //�׷����� ���콺�� ������ �� ���콺 �������� �ݴ�������� �ѱ⸦ �̵���Ű��, ���콺�� �������� ���� �� �ٽ� ������ �ڸ���
        //���ƿ��� �������Ѵ� �׷��� _moveX�� -�� �پ�� �ϴ� ���̴�. 
        if(!theController.isFineSightMode)// ������ ���°� �ƴ� ���� ��鸲
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -limitPos.y, limitPos.y),
                originPos.z);
        }
        else // ������ ���¿����� ��鸲
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -fineSightLimitPos.x, fineSightLimitPos.x),
                Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                originPos.z);
        }

        transform.localPosition = currentPos;
    }

    // ���콺�� ���߸� �ѱⰡ ����ġ��.
    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;

    }



}
