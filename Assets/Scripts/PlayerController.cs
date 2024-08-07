using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    //카메라의 민감도
    [SerializeField]
    private float lookSensitivity;

    // 오브젝트에 물리학을 입혀주는 역할을 함
    [SerializeField]
    private Rigidbody myRigid;

    //카메라를 통해 고개를 올리고 내릴 때 한계 각도를 설정하기 위한 변수 
    [SerializeField]
    private float cameraRotationLimit;
    //카메라가 바라보는 정면 각도 
    [SerializeField]
    private float currentCameraRotaionX = 0;

    [SerializeField]
    private Camera theCamera;

    void Start()
    {
        //Hierarchy의 오브젝트를 다 뒤져서 type이 카메라인 오브젝트르 가져오는 메서드 
        //카메라가 여러개 있는 경우 원하는 카메라를 가져올 수 없는 문제가 있음
        //theCamera = FindObjectOfType<Camera>();


        myRigid = GetComponent<Rigidbody>();
    }


    //매 프래임마다 호출되는 함수 대략 1초에 60번
    void Update()
    {
        Move();
        CameraRotation();
        CharacterRotation();
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

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;
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

