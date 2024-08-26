using System.Collections;
using UnityEngine;

public class Twig : MonoBehaviour
{
    [SerializeField]
    private int hp; //체력

    [SerializeField]
    private float destroyTime;

    //작은 나뭇가지 조각들
    [SerializeField]
    private GameObject go_little_Twig;


    //타격이펙트
    [SerializeField]
    private GameObject go_hit_effect_prefab;

    //회전값 변수
    private Vector3 originRot;
    private Vector3 wantedRot; //맞을때 회전되길 원하는 방향
    private Vector3 currentRot;

    //필요한 사운드 이름
    [SerializeField]
    private string hit_Sound;
    [SerializeField]
    private string broken_Sound;


    void Start()
    {
        originRot = transform.rotation.eulerAngles; // 트랜스폼의 회전값은 쿼터니언 값이기 때문에 오일러 값으로 바꿔준다. 
        currentRot = originRot;

    }
    

    public void Damage(Transform _playerTf) // 매개변수로 플레이어의 transfrom 값을 받아와서 플레이어의 위치와 반대로 나뭇가지를 휘게 한다.
    {
        hp--;

        Hit();

        StartCoroutine(HitSwayCoroutine(_playerTf));

        if( hp <= 0)
        {
            Destruction(); //파괴



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

    IEnumerator HitSwayCoroutine(Transform _target) //매개변수로 플레이어의 transform을 받는다.
    {
        // 플레이어의 위치에서 나뭇가지의 위치를 빼줌으로써 타격이 어떤 방향에서 들어왔는지 구한다.
        Vector3 direction = (_target.position - transform.position).normalized; 

        Vector3 rotationDir = Quaternion.LookRotation(direction).eulerAngles;

        CheckDirection(rotationDir);


        //나뭇가지가 타격을 받고 천천히 쓰러진다.
        while(!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.12f);

            //Quaternion.Euler : 오일러각을 쿼터니언을 바꿔주는 메서드. 만약 쿼터니언 값을 넣으면 다시 오일러각으로 바꿔준다.
            transform.rotation = Quaternion.Euler(currentRot);
            yield return null;
        }

        wantedRot = originRot; // 원래위치(회전값)을 목표위치로 설정

        //쓰러졌던 나뭇가지가 천천히 일어난다.
        while (!CheckThreshold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.08f);
            transform.rotation = Quaternion.Euler(currentRot);
            yield return null;
        }
    }


    //Lerp로 회전하며 쓰러지는 나뭇가지의 회전 값이 임계점에 가까워졌는지 아닌지를 반환해주는 함수
    //만약 목표값이 -일떄를대비해서 뺸값을 절대값을 씌워서 비교한다.
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
