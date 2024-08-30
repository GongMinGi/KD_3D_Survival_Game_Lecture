using UnityEngine;

//ScriptableObject: ���� ������Ʈ�� ���� �ʿ� ���� ��ũ��Ʈ�� ����� ���� ���
//
[CreateAssetMenu(fileName ="New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{

    public string itemName; // �������� �̸�
    public ItemType itemType; //�������� ����
    public Sprite itemImage; //�������� �̹���

    //�̹����� ��������Ʈ�� ����
    //�̹����� ĵ���� ���ο����� ǥ���� �� �մ� �Ͱ� �޸� ��������Ʈ�� �׳� ����󿡼��� �̹����� ǥ���� �� �յ�. (ex: ��ǻ�� ������Ʈ ����� sprite�� ����)

    public GameObject itemPrefab; //�������� ������. �������� ���� �� ������ ������ ���� ��ü

    public string weaponType; // ���⸦ ��ü�� �� �ش� ������ �̿��ؼ� �ϵ��� ������ ��.

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

}
