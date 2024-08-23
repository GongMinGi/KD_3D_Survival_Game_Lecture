using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //���� Ȱ��ȭ ����
    public static bool isActivate = false;

    //���� ��������
    [SerializeField]
    private Gun currentGun;
   
    //����ӵ� ���
    private float currentFireRate; //Gun ��ũ��Ʈ�� Firerate�� �����ͼ� ���ݾ� ��Ƽ� 0�� �Ǹ� ���� �߻��ϰ� ���� ������ ���ƿ��� ����

    
    //���º�����
    private bool isReload = false;
    [HideInInspector] //�ν����� â���� �������ʰ� �Ѵ�.
    public bool isFineSightMode = false; // ������ ���¸� �Ǻ��ϴ� ���°�

    //���� ������ ��.
    [SerializeField]
    private Vector3 originPos;

    //ȿ���� ���
    private AudioSource audioSource;

    //������ �浹 ���� �޾ƿ�
    private RaycastHit hitinfo;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    //�ǰ� ����Ʈ.
    [SerializeField]
    private GameObject hit_effect_prefab;

    void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrosshair = FindAnyObjectByType<Crosshair>();

           

    }


    void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }

    }



    //����ӵ� ����
    void GunFireRateCalc()
    {
        if(currentFireRate >0)
        {
            //Time.deltaTime�� �������� �ѹ� ����Ǵµ� �ɸ��� �ð��̹Ƿ� ������Ʈ�Լ����� deltatime ��ŭ ���� ��´ٴ� ���� 
            //���� �ǽð����� ��´ٴ� ���̴�. �� 1�ʰ� ����ϸ� 1�� �پ���
            // Fps�� 60�̶� �����ϸ� 1/60 �� 1�ʾȿ� 60�� ���ִ� ���̱� ������ �׷���. 
            currentFireRate -= Time.deltaTime;
        }
    }

    //�߻�õ�
    private void TryFire()
    {
        //deltaTime�� ���� ���� �پ��� currentFire�� 0�� �Ǿ����� �߻�(Fire�Լ� ȣ��)
        if(Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    
    //��Ƽ踦 ���� ���� 
    //�߻� �� ���
    private void Fire()
    {
        // isReload�� false�϶��� ����ǵ��� �����Ѵ�.
        if( !isReload)
        {
            if (currentGun.currentBulletCount > 0) // ź������ �Ѿ� ������ �ϳ��� �ִٸ� �߻�
            {
                Shoot();

            }
            else
            {
                CancelFineSight(); //�����ػ��¿��� �Ѿ��� ������ �����ؿ� �����ϰ� �������Ѵ�. 
                StartCoroutine(ReloadCoroutine());

            }
        }

    }

    //������ �Ѿ��� �߻�Ǵ� ����
    //�߻� �� ���
    private void Shoot()
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;

        // �߻� ���Ŀ��� currentFireRate�� �ʱ�ȭ �ϹǷν� ���� �ӵ� ����
        currentFireRate = currentGun.fireRate;
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();

        Hit();

        //�ѱ� �ݵ� �ڷ�ƾ ����. 
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());

    }


    private void Hit()
    {
        //raycast �ÿ�, ray�� ������ ���ϴ� transform.foward�� �ѱ��� ��Ȯ�� ( accuracy)���� vector������ �����ؼ� �����ش�. 
        //�Ѿ��� z��������� �߻�ǹǷ�, Vector�� x��, y���� �������� �ּڰ��� �ִ밪�� ������ ������ �ѱ��� ��Ȯ���� ������ �� �ִ�.
        //Random.Range: �Ű������� �ּڰ��� �ִ밪�� �޾Ƽ� �� ������ ���� ��ȯ�Ѵ�.
        //currentGun.accuracy: Gun.cs�� �����Ǿ��ִ� ���������� �ش� ���� Ŀ������ ��Ȯ���� ��������.
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward + 
            new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        0),
            out hitinfo, currentGun.range))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
            Destroy(clone, 2f);
        }
    }


    //������ �õ�
    private void TryReload()
    {
        // R��ư�� ��������, �������̾ƴϰ�, �������� �Ѿ˰������� ���� �������� �Ѿ� ������ �� �������� �����Ѵ�.
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight(); // �����ػ��¿��� ������ �õ��� �������� �����ϰ� �������Ѵ�. 
            StartCoroutine(ReloadCoroutine());
        }
    }

    //���� ��ü�� ������ ���� ��� �������� ��ҽ�Ű�� ���� �뵵.
    public void CancelReload()
    {
        if(isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    //������ �߿� ���� �߻���� �ʰ� �ϱ� ���� �ڷ�ƾ�� �̿��Ѵ�. 
    //������
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount >0)
        {
            //�ڷ�ƾ�� ���߸� �ڵ��� ��� �ٽ� Update�� �Ѿ�� ������, isReload �� false�϶��� try fire�� ȣ��ǵ���
            //�����ν� ������ �ð��� �߻�Ǵ� ���� ���� �� �ִ�. 
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            //������ ź���� ��ü ����. 
            //������ �� ź������ �����մ� �Ѿ��� ���� ���� �Ѿ˿� ���������ν� ������ �ʰ� �Ѵ�. 
            currentGun.carryBulletCount += currentGun.currentBulletCount;
            //���� ���� ���� źȯ�� ���� ������ 0���� �ʱ�ȭ���ش�.
            currentGun.currentBulletCount = 0;


            //�������Ǵ� �ð����� �ڷ�ƾ �Լ��� �����Ѵ�.
            yield return new WaitForSeconds(currentGun.reloadTime);

            //���� ������ �ִ� �Ѿ��� ������ reload�� �Ѿ˰������� ������ ź������ ���� ä���. 
            if( currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                //�������� �Ѿ��� ������ŭ ���簡�� �Ѿ˿��� ���ش�.
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                //�������� �ʿ��� �Ѿ� ���� ���� ���� �Ѿ� ������ �� ���� ��� 
                //���� ���� �Ѿ��� ź������ ��� �ְ�
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                //���� ���� �Ѿ��� 0���� �ٲ���
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
        else
        {
            Debug.Log("������ �Ѿ��� �����ϴ�.");
        }
    }

    //�����ؽõ�
    private void TryFineSight()
    {
        if(Input.GetButtonDown("Fire2") && !isReload) // ���ε�� �����غҰ��� �̰ɷ� stopallcoroutine������ �浹�� ���´�. 
        {
            FineSight();
        }
    }

    //������ ��� 
    public void CancelFineSight()
    {
        if(isFineSightMode)
        {
            FineSight();
        }
    }

    //������ ���� ����
    private void FineSight()
    {
        //����� ������ ����ġó�� ���� �ٲ��.
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode); //���� �������ִ� �ִϸ��̼� 
        theCrosshair.FineSightAnimation(isFineSightMode); //ũ�ν��� �����ִ� �ִϸ��̼�

        if(isFineSightMode)
        {
            //������ �ÿ� Lerp�� ���� ������ �����̴µ�, Lerp�� �ٻ簪�� ���ϱ� ������ while���� ���������� ���� �ڷ�ƾ��
            //������ �ʰ�, �׷��� ����Ǵ� �ڷ�ƾ�� �׿����鼭 ������ ����� �������� �ȴ�. 
            //���� �����Ҷ����� �̹� �������̴� �ڷ�ƾ�� ���� ������ �ʿ䰡 �ִ�. 
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        // ������ �߿� �������� �õ��ϸ� ������ �ڷ�ƾ�� ���� ���ܹ����� ���� ����.
        // �Ŀ� finesight �ڷ�ƾ�� ������Ű�� ����� �߰��� ��. 
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    //������ Ȱ��ȭ
    IEnumerator FineSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    //������ ��Ȱ��ȭ
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }



    //�ݵ� �ڷ�ƾ
    IEnumerator RetroActionCoroutine()
    {
        // ���� 90�� �����ֱ� ������ �� �� �ݵ��� �ٶ� z���� �ƴ϶� x���� �������� ���� �ۿ���Ѿ� �ϴ� ���� ����Ұ�.
        //�� �ΰ��� Vector�� Start �Լ����� ���� �Ű��ָ� �޸� ����ȭ�� ������ �� �ִ�. ( 3�ο��� ����) 
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z) ;
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        //finesight�� �ƴҶ� ����Ǿ��ϴµ� !�� ������..��
        if(!isFineSightMode)
        {
            //�ݵ��� Ȯ���� ǥ���ϱ� ���ؼ� ���� ó�� ��ġ�� �ʱ�ȭ �����ش�.
            currentGun.transform.localPosition = originPos;

            //�ݵ�����
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f) //Lerp�� ��ǥ���� ��ġ�ϴ� ��찡 ���� ���� �빮�� 0.02���� ������ �༭ ������ �ٻ�ž����� �ݺ����� ������������ �� ��.
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //����ġ
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;

            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            //�ݵ�����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f) //Lerp�� ��ǥ���� ��ġ�ϴ� ��찡 ���� ���� �빮�� 0.02���� ������ �༭ ������ �ٻ�ž����� �ݺ����� ������������ �� ��.
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //����ġ
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;

            }
        }
    }


    //���� ���
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }


    //private�� currentGun�� �ܺο��� ���� �ֵ��� �ϱ� ���� ���� 
    public Gun GetGun()
    {
        return currentGun;
    }

    //������ ���� ���θ� ��ȯ���ִ� �Լ�.
    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }



    public void GunChange(Gun _gun)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentGun = _gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}
