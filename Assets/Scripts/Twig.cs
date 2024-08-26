using System.Collections;
using UnityEngine;

public class Twig : MonoBehaviour
{
    [SerializeField]
    private int hp; //ü��

    [SerializeField]
    private float destroyTime;

    //���� �������� ������
    [SerializeField]
    private GameObject go_little_Twig;


    //Ÿ������Ʈ
    [SerializeField]
    private GameObject go_hit_effect_prefab;

    //ȸ���� ����
    private Vector3 originRot;
    private Vector3 wantedRot; //������ ȸ���Ǳ� ���ϴ� ����
    private Vector3 currentRot;

    //�ʿ��� ���� �̸�
    [SerializeField]
    private string hit_Sound;
    [SerializeField]
    private string broken_Sound;


    void Start()
    {
        originRot = transform.rotation.eulerAngles; // Ʈ�������� ȸ������ ���ʹϾ� ���̱� ������ ���Ϸ� ������ �ٲ��ش�. 
        currentRot = originRot;

    }
    

    public void Damage(Transform _playerTf) // �Ű������� �÷��̾��� transfrom ���� �޾ƿͼ� �÷��̾��� ��ġ�� �ݴ�� ���������� �ְ� �Ѵ�.
    {
        hp--;

        Hit();

        StartCoroutine(HitSwayCoroutine(_playerTf));

        if( hp <= 0)
        {
            Destruction(); //�ı�



        }

    }

    private void Hit()
    {
        SoundManager.instance.PlaySE(hit_Sound);

        GameObject clone = Instantiate(go_hit_effect_prefab,
                                        gameObject.GetComponent<BoxCollider>().bounds.center,
                                        Quaternion.identity);

        Destroy(clone, destroyTime);
    }

    IEnumerator HitSwayCoroutine(Transform _target) //�Ű������� �÷��̾��� transform�� �޴´�.
    {
        // �÷��̾��� ��ġ���� ���������� ��ġ�� �������ν� Ÿ���� � ���⿡�� ���Դ��� ���Ѵ�.
        Vector3 direction = (_target.position - transform.position).normalized; 

        Vector3 rotationDir = Quaternion.LookRotation(direction).eulerAngles;

        CheckDirection(rotationDir);


        //���������� Ÿ���� �ް� õõ�� ��������.
        while(!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.12f);

            //Quaternion.Euler : ���Ϸ����� ���ʹϾ��� �ٲ��ִ� �޼���. ���� ���ʹϾ� ���� ������ �ٽ� ���Ϸ������� �ٲ��ش�.
            transform.rotation = Quaternion.Euler(currentRot);
            yield return null;
        }

        wantedRot = originRot; // ������ġ(ȸ����)�� ��ǥ��ġ�� ����

        //�������� ���������� õõ�� �Ͼ��.
        while (!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.08f);
            transform.rotation = Quaternion.Euler(currentRot);
            yield return null;
        }
    }


    //Lerp�� ȸ���ϸ� �������� ���������� ȸ�� ���� �Ӱ����� ����������� �ƴ����� ��ȯ���ִ� �Լ�
    //���� ��ǥ���� -�ϋ�������ؼ� �A���� ���밪�� ������ ���Ѵ�.
    private bool CheckThreshold()
    {
        if (Mathf.Abs(wantedRot.x - currentRot.x) <= 0.5f && Mathf.Abs(wantedRot.z - currentRot.z) <= 0.5f)
            return true;
        return false;
    }    

    private void CheckDirection(Vector3 _rotationDir)
    {
        Debug.Log(_rotationDir);

        if( _rotationDir.y > 180)
        {
            if (_rotationDir.y > 300)
                wantedRot = new Vector3(-50f, 0f, -50f);
            else if (_rotationDir.y > 240)
                wantedRot = new Vector3(0f, 0f, -50f);
            else
                wantedRot = new Vector3(50f, 0f, -50f);
        }
        else if (_rotationDir.y <= 180)
        {
            if (_rotationDir.y <60)
                wantedRot = new Vector3(-50f, 0f, 50f);
            else if (_rotationDir.y < 120)
                wantedRot = new Vector3(0f, 0f, 50f);
            else
                wantedRot = new Vector3(50f, 0f, 50f);

        }
    }


    private void Destruction()
    {
        SoundManager.instance.PlaySE(broken_Sound);

        GameObject clone1 = Instantiate(go_little_Twig,
                                gameObject.GetComponent<BoxCollider>().bounds.center + (Vector3.up * 0.5f),
                                Quaternion.identity);

        

        GameObject clone2 = Instantiate(go_little_Twig,
                                gameObject.GetComponent<BoxCollider>().bounds.center - (Vector3.up * 0.5f),
                                Quaternion.identity);


        Destroy(this.gameObject);

        Destroy(clone1, destroyTime);
        Destroy(clone2, destroyTime);

    }
}
