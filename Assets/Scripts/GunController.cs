using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //현재 활성화 여부
    public static bool isActivate = false;

    //현재 장착된총
    [SerializeField]
    private Gun currentGun;
   
    //연사속도 계산
    private float currentFireRate; //Gun 스크립트이 Firerate를 가져와서 조금씩 깎아서 0이 되면 총을 발사하고 원래 값으로 돌아오는 형식

    
    //상태변수들
    private bool isReload = false;
    [HideInInspector] //인스펙터 창에서 보이지않게 한다.
    public bool isFineSightMode = false; // 정조준 상태를 판별하는 상태값

    //본래 포지션 값.
    [SerializeField]
    private Vector3 originPos;

    //효과음 재생
    private AudioSource audioSource;

    //레이저 충돌 정보 받아옴
    private RaycastHit hitinfo;

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    //피격 이펙트.
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



    //연사속도 재계산
    void GunFireRateCalc()
    {
        if(currentFireRate >0)
        {
            //Time.deltaTime은 프레임이 한번 실행되는데 걸리는 시간이므로 업데이트함수에서 deltatime 만큼 값을 깎는다는 것은 
            //값을 실시간으로 깎는다는 뜻이다. 즉 1초가 경과하면 1이 줄어든다
            // Fps가 60이라 가정하면 1/60 을 1초안에 60번 빼주는 것이기 때문에 그렇다. 
            currentFireRate -= Time.deltaTime;
        }
    }

    //발사시도
    private void TryFire()
    {
        //deltaTime을 통해 전부 줄어들어 currentFire가 0이 되었을때 발사(Fire함수 호출)
        if(Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    
    //방아쇠를 당기기 까지 
    //발사 후 계산
    private void Fire()
    {
        // isReload가 false일때만 실행되도록 수정한다.
        if( !isReload)
        {
            if (currentGun.currentBulletCount > 0) // 탄알집에 총알 개수가 하나라도 있다면 발사
            {
                Shoot();

            }
            else
            {
                CancelFineSight(); //정조준상태에서 총알이 없을때 정조준울 해제하고 재장전한다. 
                StartCoroutine(ReloadCoroutine());

            }
        }

    }

    //실제로 총알이 발사되는 과정
    //발사 후 계산
    private void Shoot()
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;

        // 발사 이후에는 currentFireRate를 초기화 하므로써 연사 속도 재계산
        currentFireRate = currentGun.fireRate;
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();

        Hit();

        //총기 반동 코루틴 실행. 
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());

    }


    private void Hit()
    {
        //raycast 시에, ray의 방향을 정하는 transform.foward에 총기의 정확성 ( accuracy)값을 vector값으로 설정해서 더해준다. 
        //총알은 z축방향으로 발사되므로, Vector의 x값, y값에 랜덤으로 최솟값과 최대값을 설정해 놓으면 총기의 정확도를 설정할 수 있다.
        //Random.Range: 매개변수로 최솟값과 최대값을 받아서 그 사이의 값을 반환한다.
        //currentGun.accuracy: Gun.cs에 설정되어있는 변수값으로 해당 값이 커질수록 정확도가 떨어진다.
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


    //재장전 시도
    private void TryReload()
    {
        // R버튼을 눌렀을때, 재장전이아니고, 재장전할 총알개수보다 현재 장전중인 총알 개수가 더 적을때만 실행한다.
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight(); // 정조준상태에서 재장전 시도시 정조준을 해제하고 재장전한다. 
            StartCoroutine(ReloadCoroutine());
        }
    }

    //무기 교체시 재장전 중일 경우 재장전을 취소시키기 위한 용도.
    public void CancelReload()
    {
        if(isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    //재장전 중에 총이 발사되지 않게 하기 위해 코루틴을 이용한다. 
    //재장전
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount >0)
        {
            //코루틴이 멈추면 코드의 제어가 다시 Update로 넘어가기 때문에, isReload 가 false일때만 try fire가 호출되도록
            //함으로써 재장전 시간에 발사되는 일을 막을 수 있다. 
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            //전술적 탄알집 교체 구현. 
            //재장전 시 탄알집에 남아잇는 총알을 현재 가진 총알에 더해줌으로써 버리지 않게 한다. 
            currentGun.carryBulletCount += currentGun.currentBulletCount;
            //장전 중이 었던 탄환을 전부 뺐으니 0으로 초기화해준다.
            currentGun.currentBulletCount = 0;


            //재장전되는 시간동안 코루틴 함수를 정지한다.
            yield return new WaitForSeconds(currentGun.reloadTime);

            //현재 가지고 있는 총알의 개수가 reload할 총알개수보다 많으면 탄알집을 가득 채운다. 
            if( currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                //재장전한 총알의 갯수만큼 현재가진 총알에서 빼준다.
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                //재장전에 필요한 총알 보다 현재 가진 총알 개수가 더 적은 경우 
                //현재 가진 총알을 탄알집에 모두 넣고
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                //현재 가진 총알을 0으로 바꿔줌
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }
    }

    //정조준시도
    private void TryFineSight()
    {
        if(Input.GetButtonDown("Fire2") && !isReload) // 리로드시 정조준불가능 이걸로 stopallcoroutine끼리의 충돌을 막는다. 
        {
            FineSight();
        }
    }

    //정조준 취소 
    public void CancelFineSight()
    {
        if(isFineSightMode)
        {
            FineSight();
        }
    }

    //정조준 로직 가동
    private void FineSight()
    {
        //실행될 때마다 스위치처럼 값이 바뀐다.
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode); //총을 움직여주는 애니메이션 
        theCrosshair.FineSightAnimation(isFineSightMode); //크로스헤어를 없애주는 애니메이션

        if(isFineSightMode)
        {
            //정조준 시에 Lerp를 통해 시점을 움직이는데, Lerp는 근사값을 구하기 때문에 while문을 빠져나오지 못해 코루틴이
            //끝나지 않고, 그렇게 실행되는 코루틴이 쌓여가면서 정조준 기능이 망가지게 된다. 
            //따라서 실행할때마다 이미 실행중이던 코루틴은 전부 끝내줄 필요가 있다. 
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        // 재장전 중에 정조준을 시도하면 재장전 코루틴도 같이 끊겨버리는 버그 있음.
        // 후에 finesight 코루틴만 정지시키는 방안을 추가할 것. 
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    //정조준 활성화
    IEnumerator FineSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    //정조준 비활성화
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }



    //반동 코루틴
    IEnumerator RetroActionCoroutine()
    {
        // 총이 90도 꺾여있기 때문에 앞 뒤 반동을 줄때 z축이 아니라 x축을 기준으로 힘을 작용시켜야 하는 것을 명심할것.
        //이 두개의 Vector는 Start 함수에서 값을 매겨주면 메모리 단편화를 방지할 수 있다. ( 3부에서 나옴) 
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z) ;
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        //finesight가 아닐때 실행되야하는데 !를 안찍음..하
        if(!isFineSightMode)
        {
            //반동을 확실히 표현하기 위해서 총을 처음 위치로 초기화 시켜준다.
            currentGun.transform.localPosition = originPos;

            //반동시작
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f) //Lerp는 목표값과 일치하는 경우가 거의 없기 대문에 0.02정도 여유를 줘서 그정도 근사돼었으면 반복문을 빠져나오도록 한 것.
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //원위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;

            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            //반동시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f) //Lerp는 목표값과 일치하는 경우가 거의 없기 대문에 0.02정도 여유를 줘서 그정도 근사돼었으면 반복문을 빠져나오도록 한 것.
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //원위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;

            }
        }
    }


    //사운드 재생
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }


    //private인 currentGun을 외부에서 쓸수 있도록 하기 위해 만듬 
    public Gun GetGun()
    {
        return currentGun;
    }

    //정조준 상태 여부를 반환해주는 함수.
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
