using UnityEngine;



[System.Serializable]
[SerializeField]
public class ItemEffect
{
    public string itemName; // 아이템의 이름. (키값)
    [Tooltip("HP , SP, DP, HUNGRY , THIRSTY, SATISFY 만 가능합니다.")]
    //하나의 아이템이 여러가지 수치에 영향을 미칠 수 있기 때문에 배열로 만든다.
    public string[] part; //부위 (어떤부뷴을 회복시키거나 깎을건지)
    public int[] num; // 수치 
    

}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    //필요한 컴포넌트
    [SerializeField]
    private StatusController thePlayerStatus;
    [SerializeField]
    private WeaponManager theWeaponManager;


    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";


    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            //장착
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }


        if (_item.itemType == Item.ItemType.Used) //파라미터로 넘어 온 아이템이 소모품이라면, 
        {
            for(int x = 0; x<itemEffects.Length ; x++)// 맞을경우, 아이템이 효과종류만큼 반복
            {
                if (itemEffects[x].itemName == _item.itemName)//넘어온 아이템의 이름과 아이템 효과이 아이템이름이 일치하는지 확인
                {
                    //일치하는 게 있다면 특정 부위에 효과 적용
                    // x번째 아이템 이펙트의 part 개수(길이)만큼 반복
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
                                Debug.Log("잘못된 Status 부위. HP , SP, DP, HUNGRY , THIRSTY, SATISFY 만 가능합니다.");
                                break;
                        }
                        Debug.Log(_item.itemName + " 을 사용했습니다.");

                    }
                    return;

                }
            }
            Debug.Log("ItemEffectDatabase 에 일치하는 ItemName이 없습니다.");
        }
    }
}
