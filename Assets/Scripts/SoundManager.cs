using UnityEngine;



// MonoBehaviour를 상속받지 않았기 때문에 컴포넌트로 붙일 수 없다.
[System.Serializable] // 데이터 직렬화. 클래스 자체를 직렬화시키는 것. 이렇게 해야 인스팩터 창에서 볼수 있다.
public class Sound
{
    public string name; //곡의 이름
    public AudioClip clip; //곡

}



//싱글턴. singleton. 1개 => 프로젝트 안에서 계속 하나를 유지시키는 것.
// 프로젝트 내부에서 신 이동이 이뤄지면 이전 씬에 있던 오브젝트가 파괴 되게 된다. 사운드 매니저도 파괴되기때문에 씬 이동시 사용할 수 없다.
//따라서 씬 이동이 발생해도 사운드 매니저가 파괴되면 안됀다.
//또한, 매니저를 파괴하지 않게되면, 다른 씬으로 이동 후 원래 씬으로 돌아왔을때 똑같은 매니저가 2개 생성되게 된다. 
// 이런 상황들을 방지하기 위해서 singleton을 사용해야 한다. 

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance; //자기자신을 인스턴스로 만듬, 또한 static이므로 어디서든 접근가능한 공유 자원



    //awake: 객체 생성시 최초 실행 (오브젝트가 생성될 때 단 한번만 실행됨) 
    //statrt: 매번 활성화될 때마다 실행 (setActive.true될때마다 실행된다는 얘기) 
    //싱글턴은 객체가 생성될 때 단 한번만 실행되면 되므로 Awake에다가 만들어야 함.
    //1개가 아니라 n개를 유지시키는 로직도 따로 존재한다. 

    #region singleton
    private void Awake()
    {
        if (instance == null) //든개 아무것도 없으면
        {
            instance = this; //최초 실행시 자기 자신을 넣어줌
            DontDestroyOnLoad(gameObject); //자기자신을 파괴시키지 말라는 메서드.
        }
        else
            Destroy(this.gameObject); //만약 씬 이동으로 인해 새로운 사운드매니저가 생성되려할 때 기존의 인스턴스가 있다면 바로 파괴시킨다.
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;


    //Sound는 컴포넌트로 붙일 수 없기 때문에 이런 식으로 변수화 시켜서 사용해야 한다.
    public Sound[] effectsSounds;
    public Sound[] bgmSound;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }


    public void PlaySE(string _name) //name과 일치하는 곡이 있는지 Sound[]에서 찾고 찾으면 audioSorce에 넣는다.
    {
        for (int i = 0; i < effectsSounds.Length; i++)
        {
            if(_name == effectsSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++) //재생 중이지 않은 audioSource를 찾는 반복ㅁㄴ
                {
                    if (!audioSourceEffects[j].isPlaying) //j 번째 audioSource가 재생중이지 않다면 해당 오디오 소스에서 재생 시작
                    {
                        playSoundName[j] = effectsSounds[i].name; //실행시키려는 음악의 제목을 j번째 칸에 넣는다. j번째인이유는 오디오소스와 실행되는 인덱스를 맞춰야하기 때문.
                        audioSourceEffects[j].clip = effectsSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
        
                }
                Debug.Log("모든 가용 오디오 소스가 사용중입니다");
            }
        }

        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }


    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++) //모든 곡 재생 정지
        {
            audioSourceEffects[i].Stop();
        }
        
    }


    //특정한 곡 재생 정지
    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++) //모든 곡 재생 정지
        {
            if (playSoundName[i] == _name)
            {
               audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다.");
    }

    ////매번 활성화되면 실행. 하지만 staart와 달리 코루틴 실행이 불가능하다.
    //private void OnEnable()
    //{

    //}

}
