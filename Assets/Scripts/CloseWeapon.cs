using UnityEngine;

public class CloseWeapon : MonoBehaviour
{

    public string closeWeaponName; //��Ŭ�̳� �Ǽ��� �����ϱ� ���� ������ ����

    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;


    public float range; //���� ����
    public int damage; // ���ݷ�
    public float workSpeed; // �۾��ӵ�
    public float attackDelay; // ���� ������
    public float attackDelayA; // ���� Ȱ��ȭ ����. �ָ��� ����� �������� �ð� �� ���̿� �ָ԰� ������ �Ͼ�� ������ ó��
    public float attackDelayB; // ���� ��Ȱ��ȭ ����. �ָ��� ȸ���ϴµ� �ɸ��� �ð� �� ���̿��� ���ݹ�ư�� ������ ������ ������ ����

    //�Ϲ����� ���ݰ� ������ �닚 (����)�� �ִϸ��̼� �������� �ٸ����� �����̸� �ٸ��� �� �ʿ䰡 �ִ�.

    public float workDelay; // �۾� ������
    public float workDelayA; // �۾� Ȱ��ȭ ����. �ָ��� ����� �������� �ð� �� ���̿� �ָ԰� ������ �Ͼ�� ������ ó��
    public float workDelayB; // �۾� ��Ȱ��ȭ ����. �ָ��� ȸ���ϴµ� �ɸ��� �ð� �� ���̿��� ���ݹ�ư�� ������ ������ ������ ����


    public Animator anim; 
    // �տ� �ڽ��ݶ��̴��� �޾Ƽ� �浹 ������ ó���� �� �� �� ������ ���� ������ 1��Ī�̰� ������ 3��Ī���� �Ǳ� ������, 
    // ���� ��ó�� �������� ���� �ʰų� ���� ���� ��ó�� �������� ���� ��찡 �߻��� �� �ֱ� ������, �ݶ��̴��� ������� �ʴ´�.
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
