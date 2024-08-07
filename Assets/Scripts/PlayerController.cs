using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    //ī�޶��� �ΰ���
    [SerializeField]
    private float lookSensitivity;

    // ������Ʈ�� �������� �����ִ� ������ ��
    [SerializeField]
    private Rigidbody myRigid;

    //ī�޶� ���� ���� �ø��� ���� �� �Ѱ� ������ �����ϱ� ���� ���� 
    [SerializeField]
    private float cameraRotationLimit;
    //ī�޶� �ٶ󺸴� ���� ���� 
    [SerializeField]
    private float currentCameraRotaionX = 0;

    [SerializeField]
    private Camera theCamera;

    void Start()
    {
        //Hierarchy�� ������Ʈ�� �� ������ type�� ī�޶��� ������Ʈ�� �������� �޼��� 
        //ī�޶� ������ �ִ� ��� ���ϴ� ī�޶� ������ �� ���� ������ ����
        //theCamera = FindObjectOfType<Camera>();


        myRigid = GetComponent<Rigidbody>();
    }


    //�� �����Ӹ��� ȣ��Ǵ� �Լ� �뷫 1�ʿ� 60��
    void Update()
    {
        Move();
        CameraRotation();
        CharacterRotation();
    }



    private void Move()
    {
        // �¿� ����Ű�� ������ x ���� 1, -1�� �ٲ��ִ� �޼���
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        // 3D������ z���� ���� �̵��̴�. 
        float _moveDirZ= Input.GetAxisRaw("Vertical");

        // transform.right�� Vector3 (1,0,0)�� �ǹ��Ѵ�.
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        // transform.forward�� Vector3 (0,0,1)�� �ǹ��Ѵ�.
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;
        // (1,0,0) + (0,0,1) = (1, 0, 1) = 2 
        // ���� ������ ���� 1�� �������� normalized�� ���� ����ȭ�����ָ� 
        // ����Ƽ������ ����ϱ� ����, ���α׷��ӿ��Ե� 1�ʿ� ������Ʈ�� �󸶳� �̵���ų ������ ����ϱ� ���ϴ�.

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
        //time.deltatime: �̵��ϴ� ���� �� 3�̶�� ġ�� 1�ʵ��� 3��ŭ �����̰� ���ش�.
        //�̸� ���� ������Ʈ�� �����̵��ϴ� ��ó�� ���̴� ������ ���� �� �ִ�. 
        // deltatime�� ���� �� 0.016 �̴�. ��, �ش� ������ �������� �ɰ��°�

     

    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;


        //ī�޶� �����̼��� �����ϸ� �ݴ��ȸ������ �������ν� ���콺 ���� ȿ���� �ذ��� �� �ִ�.
        // +=�� ���� ���� �ְ� �Ǹ� ���콺 �����Ӱ� ī�޶� �������� �ݴ밡 �ȴ�. 
        currentCameraRotaionX -= _cameraRotationX;

        //Mathf.Clamp�� ���� currentCameraRotationX���� +45�� ~-45�����̿����� �����̵��� ������ �� �ִ�.
        // �Ű�������( ���Ѱ�, �ּҰ�, �ִ밪) ���·� �Է��Ͽ� �����Ѵ�. 
        //�ִ� �ּҸ� �Ѵ� ���� ���´ٸ� ���� �ִ� �ּҰ����� ����ȴ�. 
        currentCameraRotaionX = Mathf.Clamp(currentCameraRotaionX, -cameraRotationLimit, cameraRotationLimit);

        //eulerAngle:���ʹϿ��� 4���� ������� ǥ���ϴ� �Լ�. ����� x,y,z���� ��Ÿ���ٰ� �����ϸ� �ȴ�. 
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotaionX, 0f, 0f);

    }


    //�¿� ĳ���� ȸ��
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");

        Vector3 _CharacterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        //MoveRotation�� �Ű������� quaternion�� �ޱ� ������ Euler�Լ��� �̿��ؼ� 
        //���� ���Ͱ��� ���ʹϾ����� �ٲ��־�� �Ѵ�. 
        myRigid.MoveRotation(myRigid.rotation*Quaternion.Euler(_CharacterRotationY));


        Debug.Log(myRigid.rotation);
        Debug.Log(myRigid.rotation.eulerAngles);
    }
}

