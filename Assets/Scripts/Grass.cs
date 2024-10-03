using UnityEngine;

public class Grass : MonoBehaviour
{
    //풀 체력 
    [SerializeField]
    private int hp;

    //이펙트 제거 시간
    [SerializeField]
    private float destroyTime;
    //폭발력 세기
    [SerializeField]
    private float force;
    

    //타격 효과 
    [SerializeField]
    private GameObject go_hit_effect_prefab;

    [SerializeField]
    private Item item_leaf;
    [SerializeField]
    private int leafCount; // 획득할 잎 개수


    private Inventory theInven;


    private Rigidbody[] rigidbodies;
    private BoxCollider[] boxColliders;

    [SerializeField]
    private string hit_sound;

    void Start()
    {
        theInven = FindAnyObjectByType<Inventory>();
        rigidbodies = this.transform.GetComponentsInChildren<Rigidbody>(); //grass 오브젝트의 모든 자식의 rigidbody를 가져옴 
        boxColliders = transform.GetComponentsInChildren<BoxCollider>();
    }

    public void Damage()
    {
        hp--;
        Hit();

        if(hp <=0)
        {
            Destruction();
        }

    }

    private void Hit()
    {
        SoundManager.instance.PlaySE(hit_sound);

        var clone = Instantiate(go_hit_effect_prefab, transform.position + Vector3.up, Quaternion.identity);
        Destroy(clone, destroyTime);
    }


    private void Destruction()
    {
        //leaf가 파괴될때 leafCount만큼의 leaf아이템을 인벤토리에 추가 
        theInven.AcquireItem(item_leaf, leafCount);

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].useGravity = true;
            rigidbodies[i].AddExplosionForce(force, transform.position, 1f);
            boxColliders[i].enabled = true;
        }

        Destroy(this.gameObject, destroyTime);
    }

}
