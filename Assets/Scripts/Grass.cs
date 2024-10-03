using UnityEngine;

public class Grass : MonoBehaviour
{
    //Ǯ ü�� 
    [SerializeField]
    private int hp;

    //����Ʈ ���� �ð�
    [SerializeField]
    private float destroyTime;
    //���߷� ����
    [SerializeField]
    private float force;
    

    //Ÿ�� ȿ�� 
    [SerializeField]
    private GameObject go_hit_effect_prefab;

    [SerializeField]
    private Item item_leaf;
    [SerializeField]
    private int leafCount; // ȹ���� �� ����


    private Inventory theInven;


    private Rigidbody[] rigidbodies;
    private BoxCollider[] boxColliders;

    [SerializeField]
    private string hit_sound;

    void Start()
    {
        theInven = FindAnyObjectByType<Inventory>();
        rigidbodies = this.transform.GetComponentsInChildren<Rigidbody>(); //grass ������Ʈ�� ��� �ڽ��� rigidbody�� ������ 
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
        //leaf�� �ı��ɶ� leafCount��ŭ�� leaf�������� �κ��丮�� �߰� 
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
