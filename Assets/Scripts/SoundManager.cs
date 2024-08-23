using UnityEngine;



// MonoBehaviour�� ��ӹ��� �ʾұ� ������ ������Ʈ�� ���� �� ����.
[System.Serializable] // ������ ����ȭ. Ŭ���� ��ü�� ����ȭ��Ű�� ��. �̷��� �ؾ� �ν����� â���� ���� �ִ�.
public class Sound
{
    public string name; //���� �̸�
    public AudioClip clip; //��

}



//�̱���. singleton. 1�� => ������Ʈ �ȿ��� ��� �ϳ��� ������Ű�� ��.
// ������Ʈ ���ο��� �� �̵��� �̷����� ���� ���� �ִ� ������Ʈ�� �ı� �ǰ� �ȴ�. ���� �Ŵ����� �ı��Ǳ⶧���� �� �̵��� ����� �� ����.
//���� �� �̵��� �߻��ص� ���� �Ŵ����� �ı��Ǹ� �ȉ´�.
//����, �Ŵ����� �ı����� �ʰԵǸ�, �ٸ� ������ �̵� �� ���� ������ ���ƿ����� �Ȱ��� �Ŵ����� 2�� �����ǰ� �ȴ�. 
// �̷� ��Ȳ���� �����ϱ� ���ؼ� singleton�� ����ؾ� �Ѵ�. 

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance; //�ڱ��ڽ��� �ν��Ͻ��� ����, ���� static�̹Ƿ� ��𼭵� ���ٰ����� ���� �ڿ�



    //awake: ��ü ������ ���� ���� (������Ʈ�� ������ �� �� �ѹ��� �����) 
    //statrt: �Ź� Ȱ��ȭ�� ������ ���� (setActive.true�ɶ����� ����ȴٴ� ���) 
    //�̱����� ��ü�� ������ �� �� �ѹ��� ����Ǹ� �ǹǷ� Awake���ٰ� ������ ��.
    //1���� �ƴ϶� n���� ������Ű�� ������ ���� �����Ѵ�. 

    #region singleton
    private void Awake()
    {
        if (instance == null) //�簳 �ƹ��͵� ������
        {
            instance = this; //���� ����� �ڱ� �ڽ��� �־���
            DontDestroyOnLoad(gameObject); //�ڱ��ڽ��� �ı���Ű�� ����� �޼���.
        }
        else
            Destroy(this.gameObject); //���� �� �̵����� ���� ���ο� ����Ŵ����� �����Ƿ��� �� ������ �ν��Ͻ��� �ִٸ� �ٷ� �ı���Ų��.
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;


    //Sound�� ������Ʈ�� ���� �� ���� ������ �̷� ������ ����ȭ ���Ѽ� ����ؾ� �Ѵ�.
    public Sound[] effectsSounds;
    public Sound[] bgmSound;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }


    public void PlaySE(string _name) //name�� ��ġ�ϴ� ���� �ִ��� Sound[]���� ã�� ã���� audioSorce�� �ִ´�.
    {
        for (int i = 0; i < effectsSounds.Length; i++)
        {
            if(_name == effectsSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++) //��� ������ ���� audioSource�� ã�� �ݺ�����
                {
                    if (!audioSourceEffects[j].isPlaying) //j ��° audioSource�� ��������� �ʴٸ� �ش� ����� �ҽ����� ��� ����
                    {
                        playSoundName[j] = effectsSounds[i].name; //�����Ű���� ������ ������ j��° ĭ�� �ִ´�. j��°�������� ������ҽ��� ����Ǵ� �ε����� ������ϱ� ����.
                        audioSourceEffects[j].clip = effectsSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
        
                }
                Debug.Log("��� ���� ����� �ҽ��� ������Դϴ�");
            }
        }

        Debug.Log(_name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
    }


    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++) //��� �� ��� ����
        {
            audioSourceEffects[i].Stop();
        }
        
    }


    //Ư���� �� ��� ����
    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++) //��� �� ��� ����
        {
            if (playSoundName[i] == _name)
            {
               audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("��� ����" + _name + "���尡 �����ϴ�.");
    }

    ////�Ź� Ȱ��ȭ�Ǹ� ����. ������ staart�� �޸� �ڷ�ƾ ������ �Ұ����ϴ�.
    //private void OnEnable()
    //{

    //}

}
