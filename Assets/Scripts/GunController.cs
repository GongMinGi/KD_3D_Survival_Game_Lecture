using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate; //Gun 스크립트이 Firerate를 가져와서 조금씩 깎아서 0이 되면 총을 발사하고 원래 값으로 돌아오는 형식

    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }

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

    private void TryFire()
    {
        //deltaTime을 통해 전부 줄어들어 currentFire가 0이 되었을때 발사(Fire함수 호출)
        if(Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    
    //방아쇠를 당기기 까지 
    private void Fire()
    {
        // 발사 이후에는 currentFireRate를 초기화 하므로써
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    //실제로 총알이 발사되는 과정
    private void Shoot()
    {
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("총알 발사함");
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
