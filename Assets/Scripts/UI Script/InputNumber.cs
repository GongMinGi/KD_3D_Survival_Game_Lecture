using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    private bool activiated;

    [SerializeField]
    private Text text_Preview;
    [SerializeField]
    private Text text_Input;
    [SerializeField]
    private InputField if_text;

    [SerializeField]
    private GameObject go_Base;


    [SerializeField]
    private ActionController thePlayer;

    void Update()
    {
        if (activiated)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OK();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cancel();
            }
        }

    }

    public void Call()
    {
        go_Base.SetActive(true);
        activiated = true;
        if_text.text = ""; //호출할때마다 InputField의 텍스트를 초기화시킴
        text_Preview.text = DragSlot.instance.dragSlot.itemCount.ToString(); //함수를 실행하면 textpreview에 아이템의 최대개수를 적어준다.
    }

    public void Cancel()
    {
        activiated = false;
        go_Base.SetActive(false);
        DragSlot.instance.SetColor(0); //취소를 눌렀을 때에도 드래그슬롯이 투명화되고 null이 될 수 있도록 처리
        DragSlot.instance.dragSlot = null;
    }

    public void OK()
    {
        DragSlot.instance.SetColor(0);
        //int를 캐릭터로 강제 형변환 
        int num;
        if (text_Input.text != "") // 여백이 아니어야 한다. 버릴 아이템의 개수가 없으면 안됀다는 뜻
        {
            if (CheckNumber(text_Input.text))// 텍스트가 숫자가 아니라면 이상한 값이 나오기 때문에 숫자인지 먼저 확인한다.
            {
                num = int.Parse(text_Input.text); //int로 형변환
                if (num > DragSlot.instance.dragSlot.itemCount)
                    num = DragSlot.instance.dragSlot.itemCount; // 최대 아이템 개수보다 많이 버리려고 하면 현재가지고 있는 아이템의 최대치로 버리게끔 설정한다.
            }
            else //숫자가 아닌 문자가 들어왔을 경우
            {
                num = 1;// 숫자가 아니면 그냥 한 개만 버리게끔 설정
            }
        }
        else //아무것도 적지 않았을때는
        {
            //text_Preview에는 아이템의 최대개수만 적어줄 것이다.
            num = int.Parse(text_Preview.text); //가지고 있던 최대개수를 떨군다.
        }

        StartCoroutine(DropItemCoroutine(num));
    }


    //버리기는 코루틴을 이용하여 약간의 대기를 걸어줄 것이다.
    //한번에 여러개를 버리면 콜라이더가 겹치면서 터져나갈텐데 코루틴을 이용해서 빠르게 하나씩 버리면
    //그런 폭발을 방지할 수 있을 것.

    IEnumerator DropItemCoroutine(int _num)
    {


        for (int i = 0; i < _num; i++)
        {
            //아이템을 버릴때 생성할 아이템의 프리팹이 할당되어 있을대만 생성하고 그렇지 않으면 그냥 파괴한다.
            if(DragSlot.instance.dragSlot.item.itemPrefab != null)
            {
                Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, thePlayer.transform.position + thePlayer.transform.forward, Quaternion.identity);
            }
            DragSlot.instance.dragSlot.SetSlotCount(-1);
            yield return new WaitForSeconds(0.05f); // 아이템 하나당 0.05초의 딜레이로 버린다.
        }

        DragSlot.instance.dragSlot = null; //아이템을 모두 드랍하면 드래그 슬롯을 비워준다.
        go_Base.SetActive(false);
        activiated = false; // 모든 과정이 끝나고 활성화 상태 해제
    }

  

    private bool CheckNumber(string _argString)
    {
        //ToCharArray()를 사용하면 string을 한글자씩 배열에 넣어준다. 
        char[] _tempCharArray = _argString.ToCharArray();
        bool isNumber = true;// 확인하기 전에 일단 숫자라고 간주

        for (int i = 0; i < _tempCharArray.Length; i++)
        {
            //유니코드에서 0은 0030 이고, 16진수이기 때문에 10진법으로 48~57까지가 0~9이다
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57)
            {
                continue; //48~57사이라면 숫자이므로 continue를 통해 다음 단어 조사 
            }
            isNumber = false;// 만약 숫자가 아니라면 isNumber를 false로 바꾼다.

        }

        return isNumber;
    }
}
