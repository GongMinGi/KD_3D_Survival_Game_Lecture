using UnityEngine;

//ScriptableObject: 게임 오브젝트에 붙일 필요 없는 스크립트를 만들기 위해 사용
//
[CreateAssetMenu(fileName ="New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{

    public string itemName; // 아이템의 이름
    public ItemType itemType; //아이템의 유형
    public Sprite itemImage; //아이템의 이미지

    //이미지와 스프라이트의 차이
    //이미지는 캔버스 내부에서만 표시할 수 잇는 것과 달리 스프라이트는 그냥 월드상에서도 이미지를 표시할 수 잇따. (ex: 컴퓨터 오브젝트 만들고 sprite를 띄운다)

    public GameObject itemPrefab; //아이템의 프리팹. 아이템을 월드 상에 떨굴떄 실제로 떨굴 실체

    public string weaponType; // 무기를 교체할 때 해당 변수를 이요해서 하도록 수정할 것.

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

}
