using UnityEngine;



[System.Serializable]
[SerializeField]
public class ItemEffect
{
    public string itemName; // �������� �̸�. (Ű��)
    [Tooltip("HP , SP, DP, HUNGRY , THIRSTY, SATISFY �� �����մϴ�.")]
    //�ϳ��� �������� �������� ��ġ�� ������ ��ĥ �� �ֱ� ������ �迭�� �����.
    public string[] part; //���� (��κ��� ȸ����Ű�ų� ��������)
    public int[] num; // ��ġ 
    

}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private StatusController thePlayerStatus;
    [SerializeField]
    private WeaponManager theWeaponManager;


    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";


    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            //����
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }


        if (_item.itemType == Item.ItemType.Used) //�Ķ���ͷ� �Ѿ� �� �������� �Ҹ�ǰ�̶��, 
        {
            for(int x = 0; x<itemEffects.Length ; x++)// �������, �������� ȿ��������ŭ �ݺ�
            {
                if (itemEffects[x].itemName == _item.itemName)//�Ѿ�� �������� �̸��� ������ ȿ���� �������̸��� ��ġ�ϴ��� Ȯ��
                {
                    //��ġ�ϴ� �� �ִٸ� Ư�� ������ ȿ�� ����
                    // x��° ������ ����Ʈ�� part ����(����)��ŭ �ݺ�
                    for(int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        switch (itemEffects[x].part[y])
                        {
                            case HP:
                                thePlayerStatus.IncreaseHP(itemEffects[x].num[y]);
                                break;
                            case SP:
                                thePlayerStatus.IncreaseSP(itemEffects[x].num[y]);
                                break;
                            case DP:
                                thePlayerStatus.IncreaseDP(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerStatus.IncreaseThirsty(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerStatus.IncreaseHungry(itemEffects[x].num[y]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("�߸��� Status ����. HP , SP, DP, HUNGRY , THIRSTY, SATISFY �� �����մϴ�.");
                                break;
                        }
                        Debug.Log(_item.itemName + " �� ����߽��ϴ�.");

                    }
                    return;

                }
            }
            Debug.Log("ItemEffectDatabase �� ��ġ�ϴ� ItemName�� �����ϴ�.");
        }
    }
}
