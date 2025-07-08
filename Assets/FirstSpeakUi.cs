using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class FirstSpeakUi : MonoBehaviour
{
    [SerializeField] private GameObject First;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject Panel;
    [SerializeField] private FirstTalkcs firstTalkcs;
    [SerializeField] private Image panelImage;
    [SerializeField] private GameObject GameMain;

    [SerializeField] private float darkExpandSeconds = 0.4f; //黒が中央から指定スケールまで広がる時間


    [SerializeField] private GameObject RedUI;
    [SerializeField] private GameObject BlueUI;
    [SerializeField] private GameObject GreenUI;

    [SerializeField] private GameObject MititaIro;
    [SerializeField] private GameObject SundaAo;
    [SerializeField] private GameObject Seimei;
    [SerializeField] private GameObject Jyonetu;

    [SerializeField] private GameObject Dark;

    [SerializeField] private int RedUIGenerate;
    [SerializeField] private int BlueUIGenerate;
    [SerializeField] private int GreenUIGenerate;

    [SerializeField] private int MititaIroGenerate = 1;
    [SerializeField] private int SundaAoGenerate = 2;
    [SerializeField] private int SeimeiGenerate = 3;
    [SerializeField] private int JyonetuGenerate = 4;
    [SerializeField] private int DarkGenerate = 5;
    [SerializeField] private int DarkExpandGenerate = 6; // �_�[�N�̊g��J�n�̃J�E���g

    [SerializeField] private int DestroyThreeColorBackground = 6;
    [SerializeField] private int DestroyMititaIro = 2;
    [SerializeField] private int DestroyFirstDark = 8;
    [SerializeField] private int DestorySecondDark = 10;
    [SerializeField] private int DestoryThreeColors = 10;
    [SerializeField] private int AppearanceThreeColors = 11;

    AudioSource SundaAoAudio;
    AudioSource SeimeiAudio;
    AudioSource JyonetuAudio;
    AudioSource MititaIroAudio;

    public int NowDialogFirstCount = 0;

    private bool hasFaded = false;

    void Start()
    {
        if (Panel != null)
        {
            panelImage = Panel.GetComponent<Image>();
        }

        if (panelImage != null)
        {
            panelImage.color = new Color(0.5f, 0.5f, 0.5f, 0f);
        }

        RedUI?.SetActive(false);
        BlueUI?.SetActive(false);
        GreenUI?.SetActive(false);

        MititaIro?.SetActive(false);
        SundaAo?.SetActive(false);
        Seimei?.SetActive(false);
        Jyonetu?.SetActive(false);
        Dark?.SetActive(false);

        GetAudio(); // �����̎擾�Ɛݒ�
    }

    private void GetAudio()
    {

        // AudioSource�擾�i�������擾�Ȃ�j
        if (SundaAoAudio == null) SundaAoAudio = SundaAo.GetComponent<AudioSource>();
        if (SeimeiAudio == null) SeimeiAudio = Seimei.GetComponent<AudioSource>();
        if (JyonetuAudio == null) JyonetuAudio = Jyonetu.GetComponent<AudioSource>();
            if (MititaIroAudio == null) MititaIroAudio = MititaIro.GetComponent<AudioSource>();

        // PlayerPrefs���特�ʂ��擾�i�Ȃ����0.5�j
        float savedVolume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);

        // AudioSource�ɔ��f
        if (SundaAoAudio != null) SundaAoAudio.volume = savedVolume;
        if (SeimeiAudio != null) SeimeiAudio.volume = savedVolume;
        if (JyonetuAudio != null) JyonetuAudio.volume = savedVolume;
        if (MititaIroAudio != null) MititaIroAudio.volume = savedVolume;
    }

    void Update()
    {
        if (firstTalkcs == null || panelImage == null) return;

        if (!firstTalkcs.IsFnFirstSpeak && !hasFaded)
        {
            First.SetActive(true);
            canvas.gameObject.SetActive(true);
            GameMain.SetActive(false);

            StartCoroutine(FadeToBlackCoroutine(0.5f));
            hasFaded = true;
        }

        if (firstTalkcs.IsFnFirstSpeak)
        {
            canvas.gameObject.SetActive(false);
            panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, 0f);
            GameMain.SetActive(true);
        }

        UiEventGenerator();
    }

    private IEnumerator FadeToBlackCoroutine(float duration)
    {
        float elapsed = 0f;
        Color startColor = new Color(0.5f, 0.5f, 0.5f, 0f);
        Color endColor = new Color(0f, 0f, 0f, 1f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panelImage.color = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }

        panelImage.color = endColor;
    }

    private int lastDialogFirstCount = -1;

    private void UiEventGenerator()
    {
        if (NowDialogFirstCount == lastDialogFirstCount) return;
        lastDialogFirstCount = NowDialogFirstCount;

        if (NowDialogFirstCount == RedUIGenerate)
        {
            Debug.Log("��UI���t�F�[�h�C�����܂�");
            StartCoroutine(ColorFadeIn(RedUI, new Color(1f, 0f, 0f)));

        }
        if (NowDialogFirstCount == BlueUIGenerate)
        {
            Debug.Log("��UI���t�F�[�h�C�����܂�");
            StartCoroutine(ColorFadeIn(BlueUI, new Color(0f, 0f, 1f)));

        }
        if (NowDialogFirstCount == GreenUIGenerate)
        {
            Debug.Log("��UI���t�F�[�h�C�����܂�");
            StartCoroutine(ColorFadeIn(GreenUI, new Color(0f, 1f, 0f)));

        }
        if (NowDialogFirstCount == MititaIroGenerate)
        {
            Debug.Log("�������F���t�F�[�h�C�����܂�");
            StartCoroutine(FadeInImage(MititaIro));
            PlayAndStop(MititaIroAudio, 0.7f); // �������F�̉������Đ�
        }
        if (NowDialogFirstCount == SundaAoGenerate)
        {
            Debug.Log("���񂾐��t�F�[�h�C�����܂�");
            StartCoroutine(FadeInImage(SundaAo));
            PlayAndStop(SundaAoAudio, 0.7f); // �ς񂾐쉹��
        }
        if (NowDialogFirstCount == SeimeiGenerate)
        {
            Debug.Log("�������t�F�[�h�C�����܂�");
            StartCoroutine(FadeInImage(Seimei));
            PlayAndStop(SeimeiAudio, 0.7f); // �����̉������Đ�
        }
        if (NowDialogFirstCount == JyonetuGenerate)
        {
            Debug.Log("��M���t�F�[�h�C�����܂�");
            StartCoroutine(FadeInImage(Jyonetu));
            PlayAndStop(JyonetuAudio, 0.7f); // ��M�̉������Đ�
        }
        if (NowDialogFirstCount == DarkGenerate)
        {
            Debug.Log("�_�[�N���t�F�[�h�C�����܂�");
            StartCoroutine(FadeInImage(Dark));
        }

        if (NowDialogFirstCount == DarkExpandGenerate)
        {
            Debug.Log("�_�[�N���g�債�܂�");
            StartCoroutine(ExpandDarkToFullScreen());
        }

        if (NowDialogFirstCount == DestroyThreeColorBackground)
        {
            StartCoroutine(FadeOutThreeColorsBackground());
        }
        if (NowDialogFirstCount == DestroyMititaIro)
        {
            Debug.Log("�������F���\���ɂ��܂�");

            MititaIro.SetActive(false); // ���F�̃t�F�[�h�C���̈ב���\��
        }
        if (NowDialogFirstCount == DestroyFirstDark)
        {
            Debug.Log("�_�[�N���t�F�[�h�A�E�g���܂�");
            StartCoroutine(FadeOutImage(Dark));
        }

        if (NowDialogFirstCount == DestoryThreeColors)
        {
            Debug.Log("�O�FUI���t�F�[�h�A�E�g���܂�");
            StartCoroutine(FadeOutImage(RedUI));
            StartCoroutine(FadeOutImage(BlueUI));
            StartCoroutine(FadeOutImage(GreenUI));
        }

        if (NowDialogFirstCount == AppearanceThreeColors)
        {
            Debug.Log("�O�FUI�����ԂɃt�F�[�h�C�����܂�");
            StartCoroutine(ShowThreeColorsWithWaits());
        }

    }

    IEnumerator ColorFadeIn(GameObject ui, Color targetColor)
    {
        ui.SetActive(true);
        Image img = ui.GetComponent<Image>();
        img.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0f);
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            img.color = new Color(targetColor.r, targetColor.g, targetColor.b, Mathf.Clamp01(elapsed));
            yield return null;
        }
    }

    IEnumerator FadeInImage(GameObject obj)
    {
        obj.SetActive(true);
        Image img = obj.GetComponent<Image>();
        Color col = img.color;
        col.a = 0f;
        img.color = col;

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            col.a = Mathf.Clamp01(elapsed);
            img.color = col;
            yield return null;
        }
    }

    IEnumerator FadeOutImage(GameObject obj)
    {
        if (!obj.activeSelf) yield break;
        Image img = obj.GetComponent<Image>();
        Color col = img.color;

        float elapsed = 0f;
        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            col.a = 1f - Mathf.Clamp01(elapsed / 0.3f);
            img.color = col;
            yield return null;
        }
        obj.SetActive(false);
    }

    IEnumerator WaitSeconds(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

    }

    private IEnumerator ShowThreeColorsWithWaits()
    {
        PlayAndStop(SeimeiAudio, 0.7f); // �����̉������Đ�
        yield return StartCoroutine(FadeInImage(GreenUI));

        yield return StartCoroutine(WaitSeconds(0.05f));
        PlayAndStop(SundaAoAudio, 0.7f); // ���񂾐̉������Đ�
        yield return StartCoroutine(FadeInImage(BlueUI));

        yield return StartCoroutine(WaitSeconds(0.05f));
        PlayAndStop(JyonetuAudio, 0.7f);
        yield return StartCoroutine(FadeInImage(RedUI));

    }

    private IEnumerator FadeOutThreeColorsBackground()
    {
        yield return new WaitForSeconds(0.3f); // �����L����̂������҂�

        Coroutine fade1 = StartCoroutine(FadeOutImage(SundaAo));
        Coroutine fade2 = StartCoroutine(FadeOutImage(Seimei));
        Coroutine fade3 = StartCoroutine(FadeOutImage(Jyonetu));

    }

    [SerializeField] private float DarkexpandScale = 10f;  // �g��I���X�P�[���i��F10�{�j

    private IEnumerator ExpandDarkToFullScreen()
    {
        Dark.SetActive(true);
        RectTransform rt = Dark.GetComponent<RectTransform>();

        Vector3 startScale = new Vector3(2f, 2f, 1f);  // �J�n�X�P�[��
        Vector3 endScale = new Vector3(10f, 7f, 1f);   // �I���X�P�[��

        rt.localScale = startScale;

        float elapsed = 0f;

        while (elapsed < darkExpandSeconds)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / darkExpandSeconds);
            rt.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        rt.localScale = endScale;  // �ŏI�X�P�[���Ŋm��
    }

    private void PlayAndStop(AudioSource source, float stopAfterSeconds)
    {
        if (source == null)
        {
            Debug.LogError("AudioSource is null.");
            return;
        }

        if (!source.gameObject.activeInHierarchy)
        {
            source.gameObject.SetActive(true);
        }

        StartCoroutine(PlayRoutine(source, stopAfterSeconds));
    }

    private IEnumerator PlayRoutine(AudioSource source, float delay)
    {
        if (!source.enabled) source.enabled = true;

        yield return null; // �L�������f�҂�
        source.Stop();
        source.Play();

        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
            if (source.isPlaying) source.Stop();
        }
    }



    private IEnumerator StopAudioAfterSeconds(AudioSource source, float delay)
    {
        Debug.Log($"StopAudioAfterSeconds: {delay}�b��ɒ�~�\��B");
        yield return new WaitForSeconds(delay);

        if (source != null)
        {
            if (source.isPlaying)
            {
                Debug.Log($"StopAudioAfterSeconds: AudioSource '{source.gameObject.name}' ���~���܂��B");
                source.Stop();
            }
            else
            {
                Debug.Log($"StopAudioAfterSeconds: AudioSource '{source.gameObject.name}' �͊��ɒ�~���Ă��܂��B");
            }
        }
        else
        {
            Debug.LogWarning("StopAudioAfterSeconds: AudioSource���j������Ă��܂��B");
        }
    }



    /*
    Element 1 ���E�́A���ĐF�Ŗ����Ă����B

//�A�ԁA�΂����������摜�\���@

Element 2 ��͐��񂾐A

//���������摜������
//�����ɐ� FadeIN

Element 3 �X�͐����̗΁A

//�^�񒆂ɗ� F IN

Element 4 ��n�͏�M�̐ԂɔR���Ă����B

//�E�ɐԂ�  IN

Element 5 �����A���鎞�\�\�������ꂽ�B

//�^�񒆂Ɋۂ���

Element 6 ����͐F����炢�A

//������ʑS�̂ɍL����  ���X��

Element 7 �Ӗ���D���A

//No Action

Element 8 ���E��"�P��"�ɐ��ߏグ�Ă������B

//��ʂ̉摜��S�ď����Đ^�����ɂ���  

Element 9 �O���F�ƌĂ΂�鑶�݂����́A���ɍR�����Ƃ����B

//��ĂɐA�ԁA�΂̐����`���t�B�[�h�C��

Element 10 �����͋y�΂��A�ނ�͕��󂳂�A

//�A�ԁA�΂�������

Element 11 ���E�͐Î�Ƌ����ɕ�����B

//�^����

Element 12 �c�����č��A�Y���ꂽ�F�������A�Ăіڂ��o�܂��B

//���A�����Ƀt�B�[�h�C��
//0.1�ҋ@
//�Ԃ��^�񒆁@0.2,,,,
//0.1�ҋ@
//��

Element 13 �������߂��A���߂����߂��B
    */

}

