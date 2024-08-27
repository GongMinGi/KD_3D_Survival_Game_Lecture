using UnityEngine;

public class CloseWeapon : MonoBehaviour
{

    public string closeWeaponName; //너클이나 맨손을 구분하기 위해 설정한 변수

    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;


    public float range; //공격 범위
    public int damage; // 공격력
    public float workSpeed; // 작업속도
    public float attackDelay; // 공격 딜레이
    public float attackDelayA; // 공격 활성화 시점. 주먹을 뻗어나갈 때까지의 시간 이 사이에 주먹과 츙돌이 일어나면 데미지 처리
    public float attackDelayB; // 공격 비활성화 시점. 주먹을 회수하는데 걸리는 시간 이 사이에는 공격버튼을 눌러도 공격이 나가지 않음

    //일반적인 공격과 나무를 밸떄 (도끼)의 애니메이션 프레임이 다름으로 딜레이를 다르게 줄 필요가 있다.

    public float workDelay; // 작업 딜레이
    public float workDelayA; // 작업 활성화 시점. 주먹을 뻗어나갈 때까지의 시간 이 사이에 주먹과 츙돌이 일어나면 데미지 처리
    public float workDelayB; // 작업 비활성화 시점. 주먹을 회수하는데 걸리는 시간 이 사이에는 공격버튼을 눌러도 공격이 나가지 않음


    public Animator anim; 
    // 손에 박스콜라이더를 달아서 충돌 데미지 처리를 할 수 도 있지만 게임 시점이 1인칭이고 적용은 3인칭에서 되기 때문에, 
    // 맞은 것처럼 보이지만 맞지 않거나 맞지 않은 것처럼 보이지만 맞은 경우가 발생할 수 있기 떄문에, 콜라이더는 사용하지 않는다.
    //public BoxCollider 




    //void Start()
    //{
    //    
    //}


    //void Update()
    //{
    //    
    //}
}
