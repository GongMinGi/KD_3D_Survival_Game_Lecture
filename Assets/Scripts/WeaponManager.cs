using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // static으로 설정된 변수는, 클래스 자체가 가지고 있는 변수로, 해당 클래스로 부터 나온 객체가 변수의 값을 얻기 위해
    //클래스에 접근하게 된다. 또한, 객체 각자가 가지고 있는 것이 아닌, 클래스 자체가 가지고 있는 변수이기 때문에, 
    //하나의 객체가 변수이 값을 바꾸게 된다면 모든 객체에서 해당 변수의 값이 변경되게 된다. 
    // 클래스변수, 정적 변수라고도 한다.
    //장점: 쉽게 접근할 수 있따.
    //단점: 보안성이 떨어지고 static 변수는 일반 변수와 달리 객체가 destroy되어도 남아있기 때문에 메모리를 차지한다( 메모리낭비)
    //무기 교체시 중복 교체 실행을 방지하기 위해 만들어짐
    public static bool isChangeWeapon = false;

    //현재 무기와 현재 무기의 애니메이션
    public static Transform currentWeapon; //변수타입을 트랜스폼으로 설정한 이유는 현재무기가 근접무기일 수도 총일수도, 맨손일 수도 있기 때문에
                                            // 종류 별로 변수를 만들지 않기 위해서 모든 오브젝트가 가지고 있는 Transform으로 설정해 준 것이다.
                                            // 기존 무기를 껏다 키는 역할밖에 하지 않는다.
    public static Animator currentWeaponAnim;


    //현재 무기의 타입
    [SerializeField] //예를들어 지금 총으로 정조준중인데 도끼 뽑을라면 정조준을 풀어야하니까 현재 들고 있는 무기의 타입을 알아야 함.
    private string currentWeaponType;


    //무기 교체 딜레이 
    [SerializeField]
    private float changeWeaponDelayTime;
    //무기 교체가 완전히 끝난 시점.
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //무기 종류들 전부 관리
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickaxes;

    //관리 차원에서 쉽게 무기 접근이 가능하도록 만듦.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();




    //필요한 컴포넌트
    //현재 맨손인지, 총을 들고 있는 상태인지를 판단하기 위해 만들어진 변수
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickaxeController thePickaxeController;



    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]); // Gun.cs 스크립트에 있는 gunName 스트링을 키값으로 guns[] 배열이 들고 잇는 총들을 딕셔너리에 저장한다.

        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }

    }


    void Update()
    {
        Debug.Log(isChangeWeapon);

        if (!isChangeWeapon)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                //무기 교체 실행. ( 서브머신건)
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }
            else if ( Input.GetKeyDown(KeyCode.Alpha2))
            {
                //무기 교체 실행 ( 맨손) 
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));

            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //무기 교체 실행 ( 맨손) 
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));

            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                //무기 교체 실행 ( 맨손) 
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));

            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name) //현재 어떤 타입의 총인지 (혹은 맨손인지), 이름이 뭔지 매개변수로 받아온다
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");
        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();

        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon=false;
    }



    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate=false;
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                PickaxeController.isActivate = false;
                break;

        }
    }


    private void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
        {
            theGunController.GunChange(gunDictionary[_name]);
        }
        else if( _type == "HAND")
        {
            theHandController.CloseWeaponChange(handDictionary[_name]);
        }
        else if (_type == "AXE")
        {
            theAxeController.CloseWeaponChange(axeDictionary[_name]);
        }
        else if (_type == "PICKAXE")
        {
            thePickaxeController.CloseWeaponChange(pickaxeDictionary[_name]);
        }
    }
}
