using System.Collections;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class TreeComponent : MonoBehaviour
{
    //깎일 나무 조각들
    [SerializeField]
    private GameObject[] go_treePieces;
    [SerializeField]
    private GameObject go_treeCenter;

    //통나무
    [SerializeField]
    private GameObject go_Log_Prefabs;


    //쓰러질 때 랜덤으로 가해질 힘의 세기 
    [SerializeField]
    private float force;


    //자식 트리
    [SerializeField]
    private GameObject go_ChildTree;



    //부모 트리 파괴되면, 캡슐 콜라이더 제거.
    [SerializeField]
    private CapsuleCollider parentCol;

    //자식 트리 쓰러질 때 필요한 컴포넌트 활성화 및 중력 활성화.
    [SerializeField]
    private CapsuleCollider childCol;
    [SerializeField]
    private Rigidbody childRigid;


    // 파편
    [SerializeField]
    private GameObject go_hit_effect_prefab;

    //파편 제거 시간 
    [SerializeField]
    private float debrisDestroyTime;

    //나무 제거 시간 
    [SerializeField]
    private float DestroyTime;

    //필요한 사운드
    [SerializeField]
    private string chop_sound;
    [SerializeField]
    private string falldown_sound;
    [SerializeField]
    private string logChange_sound;

    public void Chop(Vector3 _pos, float angleY) //도끼에서 파편이 튀게 하기 위해서 플레이어의 위치와 방향을 매개변수로 받는다.
    {
        Hit(_pos);
        //플레이어가 도끼질한 위치에 따라서 깎일 나무 조각의 위치가 달라져야 함
        AngleCalc(angleY);

        if (CheckTreePieces())
            return;

        FallDownTree();
    }


    //적중 이펙트 
    private void Hit(Vector3 _pos)
    {
        SoundManager.instance.PlaySE(chop_sound);



        GameObject clone = Instantiate(go_hit_effect_prefab, _pos, Quaternion.Euler(Vector3.zero));
        Destroy(clone, debrisDestroyTime);

    }

    private void AngleCalc(float _angleY)
    {
        Debug.Log(_angleY);
        if (0 <= _angleY && _angleY <= 70)
            DestroyPiece(2);
        else if (70 <= _angleY && _angleY <= 140)
            DestroyPiece(3);
        else if (140 <= _angleY && _angleY <= 210)
            DestroyPiece(4);
        else if (210 <= _angleY && _angleY <= 280)
            DestroyPiece(0);
        else if (280 <= _angleY && _angleY <= 3600)
            DestroyPiece(1);
    }


    private void DestroyPiece(int _num)
    {
        if (go_treePieces[_num].gameObject != null)
        {
            GameObject clone = Instantiate(go_hit_effect_prefab, go_treePieces[_num].transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(clone, debrisDestroyTime);
            Destroy(go_treePieces[_num].gameObject);
        }
    }

    private bool CheckTreePieces()
    {
        for (int i = 0; i < go_treePieces.Length; i++)
        {
            if(go_treePieces[i].gameObject !=null)
            {
                return true;
            }    

        }
        return false;
    }


    private void FallDownTree()
    {
        SoundManager.instance.PlaySE(falldown_sound);
        Destroy(go_treeCenter);

        parentCol.enabled = false;
        childCol.enabled = true;
        childRigid.useGravity = true;

        childRigid.AddForce(Random.Range(-force, force), 0f, Random.Range(-force, force));



        StartCoroutine(LogCoroutine()); //트리가 사라진 다음에 통나무가 생성되어야 하므로 DestroyTime동안 기다려야함 == 코루틴필요
    }

    IEnumerator LogCoroutine()
    {
        yield return new WaitForSeconds(DestroyTime);

        SoundManager.instance.PlaySE(logChange_sound);

        Instantiate(go_Log_Prefabs, go_ChildTree.transform.position + (go_ChildTree.transform.up * 3f), Quaternion.LookRotation(go_ChildTree.transform.up));
        Instantiate(go_Log_Prefabs, go_ChildTree.transform.position + (go_ChildTree.transform.up * 6f), Quaternion.LookRotation(go_ChildTree.transform.up));
        Instantiate(go_Log_Prefabs, go_ChildTree.transform.position + (go_ChildTree.transform.up * 9f), Quaternion.LookRotation(go_ChildTree.transform.up));


        Destroy(go_ChildTree.gameObject);
    }

    public Vector3 GetTreeCenterPosition()
    {
        return go_treeCenter.transform.position;
    }

}
