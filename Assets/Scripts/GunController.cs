using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate; //Gun ��ũ��Ʈ�� Firerate�� �����ͼ� ���ݾ� ��Ƽ� 0�� �Ǹ� ���� �߻��ϰ� ���� ������ ���ƿ��� ����

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
            //Time.deltaTime�� �������� �ѹ� ����Ǵµ� �ɸ��� �ð��̹Ƿ� ������Ʈ�Լ����� deltatime ��ŭ ���� ��´ٴ� ���� 
            //���� �ǽð����� ��´ٴ� ���̴�. �� 1�ʰ� ����ϸ� 1�� �پ���
            // Fps�� 60�̶� �����ϸ� 1/60 �� 1�ʾȿ� 60�� ���ִ� ���̱� ������ �׷���. 
            currentFireRate -= Time.deltaTime;
        }
    }

    private void TryFire()
    {
        //deltaTime�� ���� ���� �پ��� currentFire�� 0�� �Ǿ����� �߻�(Fire�Լ� ȣ��)
        if(Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    
    //��Ƽ踦 ���� ���� 
    private void Fire()
    {
        // �߻� ���Ŀ��� currentFireRate�� �ʱ�ȭ �ϹǷν�
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    //������ �Ѿ��� �߻�Ǵ� ����
    private void Shoot()
    {
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("�Ѿ� �߻���");
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
