using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName; // ���� �̸�
    public float range; //�����Ÿ�
    public float accuracy; // ��Ȯ��
    public float fireRate; // ����ӵ�
    public float reloadTime; //������ �ӵ�

    public int damage; //���� ������

    public int reloadBulletCount; //���� ������ ����
    public int currentBulletCount; //���� ź������ �����ִ� �Ѿ��� ����
    public int maxBulletCount; // �ִ� ���� ���� �Ѿ� ���� 
    public int carryBulletCount; // ���� �����ϰ� �ִ� �Ѿ� ���� .

    public float retroActionForce; // �ݵ����� ( retroAction�� �ݵ��̶�� �� )
    public float retroActionFineSightForce; // ������ �ÿ� �ݵ�����

    public Vector3 fineSightOriginPos; // ������ �ÿ� ���� ��ġ
    public Animator anim;
    public ParticleSystem muzzleFlash; //���� �߻�ƶ��� ������ ǥ���� ��ƼŬ �ý���

    public AudioClip fire_Sound; // ���� �߻�� ���� �Ҹ�.



    //void Start()
    //{
    //    
    //}


    //void Update()
    //{
    //    
    //}
}
