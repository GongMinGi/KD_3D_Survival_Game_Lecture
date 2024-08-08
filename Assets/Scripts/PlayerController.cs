using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //���ǵ� ���� ���� 
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;


    //���� ����
    private bool isRun = false;
    private bool isGround = false;
    private bool isCrouch = false;


    //�ɾ��� �� �󸶳� ���� �� �����ϴ� ����. 
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;


    //�� ���� ���θ� Ȯ���ϱ� ���� ����
    private CapsuleCollider capsuleCollider;


    //ī�޶��� �ΰ���
    [SerializeField]
    private float lookSensitivity;


    //ī�޶� �����Ѱ� 

    //ī�޶� ���� ���� �ø��� ���� �� �Ѱ� ������ �����ϱ� ���� ���� 
    [SerializeField]
    private float cameraRotationLimit;
    //ī�޶� �ٶ󺸴� ���� ���� 
    [SerializeField]
    private float currentCameraRotaionX = 0;


    // �ʿ��� ������Ʈ�� 
    [SerializeField]
    private Camera theCamera;

    // ������Ʈ�� �������� �����ִ� ������ ��
    [SerializeField]
    private Rigidbody myRigid;



    void Start()
    {
        //Hierarchy�� ������Ʈ�� �� ������ type�� ī�޶��� ������Ʈ�� �������� �޼��� 
        //ī�޶� ������ �ִ� ��� ���ϴ� ī�޶� ������ �� ���� ������ ����
        //theCamera = FindObjectOfType<Camera>();


        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        capsuleCollider = GetComponent<CapsuleCollider>();
        //���� ī�޶�� player������Ʈ ������ �ֱ� ������ localposition���� ��ǥ�� �����;� �Ѵ�.
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }


    //�� �����Ӹ��� ȣ��Ǵ� �Լ� �뷫 1�ʿ� 60��
    void Update()
    {
        //�ٰ� �ִ��� �Ȱ� �մ��� �Ǵ��Ͽ� �̵��� ������ ���̱� ������ �ݵ�� Move ���� �־�� �Ѵ�. 
        TryRun();
        Move();
        CameraRotation();
        CharacterRotation();
        TryJump();
        IsGround();
        TryCrouch();
    }


    //�ɱ� �õ�
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //�ɱ�
    private void Crouch()
    {
        //isCrouch�� true�̸� false�� false�̸� true�� ��ȯ.
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



    //�Լ��� ���� �߿� �ڷ�ƾ�� ������ �Ǹ�, �������� �Լ��� �ڷ�ƾ�� �Դٰ��� �ϴ� ������� ����ó���� �����ϰ� �ȴ�. 
    // �̸� �̿��ؼ� �Լ��� �����߿� waitforsecond���� �̿��ؼ� �����̸� ���� �� �ִ�. 
    //�ε巯�� �ɱ� ����
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        // y���� ���ϴ� ���� �� ������ �ݺ� ��Ų��. 
        while(_posY != applyCrouchPosY)
        {
            count++;
            //����: ó������ ������ �پ��ų� Ŀ���ٰ� �������� ������� ���� ������ �����ϰų� �پ��
            //Mathf.Lerp(1, 2, 0.5f); => 1���� 2���� 0.5�� �����Ѵٴ��ǹ� 
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;

            yield return null; // �� �����Ӹ�ŭ ��ٸ���� ���̴�
       
        }
        //������ �̿��ϸ� ���� �ε巴�� ��ȯ��ų �� ������, ��ǥ���� ������ ������� �� ��ǥ���� ���� ������ ���Ѵ�. 
        //���� �ڷ�ƾ�� ���� ���� Ƚ�� �ݺ��� ������ �Ʒ��Ͱ��� ��ǥ���� ���� ������ �ִ� ���� ����. 
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);

        //yield return new WaitForSeconds(1f);
    }


    private void IsGround()
    {
        //ĸ�� �ݶ��̴��� ������. �ݸ�ŭ. y�� ��ŭ Vector3�� down �������� transform.position���� �������� ���� �ǹ�.
        // 0.1f �� �����̳� �밢���� ������ ��츦 ����ؼ� �־��� ������.
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    //���� �õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }



    //����
    private void Jump()
    {
        //�ɾ��ִ� ���¿��� ������ �õ��� ��, Crouch�� ȣ���Ͽ� ���� ���¸� Ǯ���ش�. 
        if (isCrouch)
            Crouch();

        //rigidbody�� �Ӽ��� linearVelocity �� ���������� ��ȭ��Ű�� ��� 
        myRigid.linearVelocity = transform.up * jumpForce;
    }

    //�޸��� �õ� 
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



    //Move �Լ��� walkSpeed ������ applySpped�� �ٲٰ� boolŸ�� isRun ������ ���� 
    //isRun ������ ���¿� ���� applySpeed�� walkspeed, runspeed�� �ٲ��ִ� ����
    //�޸���
    private void Running()
    {
        if (isCrouch)
            Crouch();
        isRun = true;
        applySpeed = runSpeed;

    }

    //�޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
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

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
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

