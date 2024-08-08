using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //스피드 조정 변수 
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;


    //상태 변수
    private bool isRun = false;
    private bool isGround = false;
    private bool isCrouch = false;


    //앉았을 때 얼마나 앉을 지 결정하는 변수. 
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;


    //땅 착지 여부를 확인하기 위한 변수
    private CapsuleCollider capsuleCollider;


    //카메라의 민감도
    [SerializeField]
    private float lookSensitivity;


    //카메라 각도한계 

    //카메라를 통해 고개를 올리고 내릴 때 한계 각도를 설정하기 위한 변수 
    [SerializeField]
    private float cameraRotationLimit;
    //카메라가 바라보는 정면 각도 
    [SerializeField]
    private float currentCameraRotaionX = 0;


    // 필요한 컴포넌트들 
    [SerializeField]
    private Camera theCamera;

    // 오브젝트에 물리학을 입혀주는 역할을 함
    [SerializeField]
    private Rigidbody myRigid;



    void Start()
    {
        //Hierarchy의 오브젝트를 다 뒤져서 type이 카메라인 오브젝트르 가져오는 메서드 
        //카메라가 여러개 있는 경우 원하는 카메라를 가져올 수 없는 문제가 있음
        //theCamera = FindObjectOfType<Camera>();


        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        capsuleCollider = GetComponent<CapsuleCollider>();
        //현재 카메라는 player오브젝트 하위에 있기 때문에 localposition으로 좌표를 가져와야 한다.
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }


    //매 프래임마다 호출되는 함수 대략 1초에 60번
    void Update()
    {
        //뛰고 있는지 걷고 잇는지 판단하여 이동을 제어할 것이기 때문에 반드시 Move 위에 있어야 한다. 
        TryRun();
        Move();
        CameraRotation();
        CharacterRotation();
        TryJump();
        IsGround();
        TryCrouch();
    }


    //앉기 시도
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //앉기
    private void Crouch()
    {
        //isCrouch가 true이면 false로 false이면 true로 변환.
        isCrouch = !isCrouch;
        
        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
    }



    //함수의 실행 중에 코루틴을 만나게 되면, 진행중인 함수와 코루틴을 왔다갔다 하는 방식으로 병렬처리를 진행하게 된다. 
    // 이를 이용해서 함수의 실행중에 waitforsecond등을 이용해서 딜레이를 만들 수 있다. 
    //부드러운 앉기 동작
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        // y값이 원하는 값이 될 때까지 반복 시킨다. 
        while(_posY != applyCrouchPosY)
        {
            count++;
            //보간: 처음에는 빠르게 줄어들거나 커지다가 목적지에 가까워질 수록 느리게 증가하거나 줄어듬
            //Mathf.Lerp(1, 2, 0.5f); => 1에서 2까지 0.5씩 증가한다는의미 
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;

            yield return null; // 한 프레임만큼 기다리라는 뜻이다
       
        }
        //보간을 이용하면 값을 부드럽게 변환시킬 수 있지만, 목표값에 무한히 가까워질 뿐 목표값에 도달 하지는 못한다. 
        //따라서 코루틴을 통해 일정 횟수 반복한 다음에 아래와같이 목표값을 직접 대입해 주는 것이 좋다. 
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);

        //yield return new WaitForSeconds(1f);
    }


    private void IsGround()
    {
        //캡슐 콜라이더의 영역의. 반만큼. y값 만큼 Vector3의 down 방향으로 transform.position에서 레이저를 쏘라는 의미.
        // 0.1f 는 경사면이나 대각선에 서있을 경우를 대비해서 넣어준 여유값.
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    //점프 시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }



    //점프
    private void Jump()
    {
        //앉아있는 상태에서 점프를 시도할 시, Crouch를 호출하여 앉은 상태를 풀어준다. 
        if (isCrouch)
            Crouch();

        //rigidbody의 속성인 linearVelocity 를 순간적으로 변화시키는 방식 
        myRigid.linearVelocity = transform.up * jumpForce;
    }

    //달리기 시도 
    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }



    //Move 함수의 walkSpeed 변수를 applySpped로 바꾸고 bool타입 isRun 변수를 만들어서 
    //isRun 변수의 상태에 따라 applySpeed를 walkspeed, runspeed로 바꿔주는 원리
    //달리기
    private void Running()
    {
        if (isCrouch)
            Crouch();
        isRun = true;
        applySpeed = runSpeed;

    }

    //달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }


    private void Move()
    {
        // 좌우 방향키를 누르면 x 값을 1, -1라 바꿔주는 메서드
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        // 3D에서는 z축이 정면 이동이다. 
        float _moveDirZ= Input.GetAxisRaw("Vertical");

        // transform.right는 Vector3 (1,0,0)을 의미한다.
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        // transform.forward는 Vector3 (0,0,1)을 의미한다.
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        // (1,0,0) + (0,0,1) = (1, 0, 1) = 2 
        // 따라서 벡터의 합이 1이 나오도록 normalized를 통해 정규화시켜주면 
        // 유니티에서도 계산하기 쉽고, 프로그래머에게도 1초에 오브젝트를 얼마나 이동시킬 것인지 계산하기 편하다.

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
        //time.deltatime: 이동하는 값이 약 3이라고 치면 1초동안 3만큼 움직이게 해준다.
        //이를 통해 오브젝트가 순간이동하는 것처럼 보이는 현상을 막을 수 있다. 
        // deltatime의 값은 약 0.016 이다. 즉, 해당 단위로 움직임을 쪼개는것

    
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;


        //카메라 로테이션이 증가하면 반대로회전값을 빼줌으로써 마우스 반전 효과를 해결할 수 있다.
        // +=를 통해 더해 주게 되면 마우스 움직임과 카메라 움직임이 반대가 된다. 
        currentCameraRotaionX -= _cameraRotationX;

        //Mathf.Clamp를 통해 currentCameraRotationX값이 +45도 ~-45도사이에서만 움직이도록 조정할 수 있다.
        // 매개변수에( 가둘값, 최소값, 최대값) 형태로 입력하여 조정한다. 
        //최대 최소를 넘는 값이 들어온다면 각각 최대 최소값으로 적용된다. 
        currentCameraRotaionX = Mathf.Clamp(currentCameraRotaionX, -cameraRotationLimit, cameraRotationLimit);

        //eulerAngle:쿼터니온인 4개의 사원수를 표시하는 함수. 현재는 x,y,z값만 나타낸다고 생각하면 된다. 
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotaionX, 0f, 0f);

    }


    //좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");

        Vector3 _CharacterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        //MoveRotation은 매개변수로 quaternion을 받기 때문에 Euler함수를 이용해서 
        //구한 백터값을 쿼터니언으로 바꿔주어야 한다. 
        myRigid.MoveRotation(myRigid.rotation*Quaternion.Euler(_CharacterRotationY));


        Debug.Log(myRigid.rotation);
        Debug.Log(myRigid.rotation.eulerAngles);
    }
}

