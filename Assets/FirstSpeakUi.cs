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

    [SerializeField] private float darkExpandSeconds = 0.4f; // 拡大にかける時間（秒）


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
    [SerializeField] private int DarkExpandGenerate = 6; // ダークの拡大開始のカウント

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

        GetAudio(); // 音声の取得と設定
    }

    private void GetAudio()
    {

        // AudioSource取得（もし未取得なら）
        if (SundaAoAudio == null) SundaAoAudio = SundaAo.GetComponent<AudioSource>();
        if (SeimeiAudio == null) SeimeiAudio = Seimei.GetComponent<AudioSource>();
        if (JyonetuAudio == null) JyonetuAudio = Jyonetu.GetComponent<AudioSource>();
            if (MititaIroAudio == null) MititaIroAudio = MititaIro.GetComponent<AudioSource>();

        // PlayerPrefsから音量を取得（なければ0.5）
        float savedVolume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);

        // AudioSourceに反映
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
            Debug.Log("赤UIをフェードインします");
            StartCoroutine(ColorFadeIn(RedUI, new Color(1f, 0f, 0f)));

        }
        if (NowDialogFirstCount == BlueUIGenerate)
        {
            Debug.Log("青UIをフェードインします");
            StartCoroutine(ColorFadeIn(BlueUI, new Color(0f, 0f, 1f)));

        }
        if (NowDialogFirstCount == GreenUIGenerate)
        {
            Debug.Log("緑UIをフェードインします");
            StartCoroutine(ColorFadeIn(GreenUI, new Color(0f, 1f, 0f)));

        }
        if (NowDialogFirstCount == MititaIroGenerate)
        {
            Debug.Log("満ちた色をフェードインします");
            StartCoroutine(FadeInImage(MititaIro));
            PlayAndStop(MititaIroAudio, 0.7f); // 満ちた色の音声を再生
        }
        if (NowDialogFirstCount == SundaAoGenerate)
        {
            Debug.Log("澄んだ青をフェードインします");
            StartCoroutine(FadeInImage(SundaAo));
            PlayAndStop(SundaAoAudio, 0.7f); // 済んだ青野音量
        }
        if (NowDialogFirstCount == SeimeiGenerate)
        {
            Debug.Log("生命をフェードインします");
            StartCoroutine(FadeInImage(Seimei));
            PlayAndStop(SeimeiAudio, 0.7f); // 生命の音声を再生
        }
        if (NowDialogFirstCount == JyonetuGenerate)
        {
            Debug.Log("情熱をフェードインします");
            StartCoroutine(FadeInImage(Jyonetu));
            PlayAndStop(JyonetuAudio, 0.7f); // 情熱の音声を再生
        }
        if (NowDialogFirstCount == DarkGenerate)
        {
            Debug.Log("ダークをフェードインします");
            StartCoroutine(FadeInImage(Dark));
        }

        if (NowDialogFirstCount == DarkExpandGenerate)
        {
            Debug.Log("ダークを拡大します");
            StartCoroutine(ExpandDarkToFullScreen());
        }

        if (NowDialogFirstCount == DestroyThreeColorBackground)
        {
            StartCoroutine(FadeOutThreeColorsBackground());
        }
        if (NowDialogFirstCount == DestroyMititaIro)
        {
            Debug.Log("満ちた色を非表示にします");

            MititaIro.SetActive(false); // 青い色のフェードインの為即非表示
        }
        if (NowDialogFirstCount == DestroyFirstDark)
        {
            Debug.Log("ダークをフェードアウトします");
            StartCoroutine(FadeOutImage(Dark));
        }

        if (NowDialogFirstCount == DestoryThreeColors)
        {
            Debug.Log("三色UIをフェードアウトします");
            StartCoroutine(FadeOutImage(RedUI));
            StartCoroutine(FadeOutImage(BlueUI));
            StartCoroutine(FadeOutImage(GreenUI));
        }

        if (NowDialogFirstCount == AppearanceThreeColors)
        {
            Debug.Log("三色UIを順番にフェードインします");
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
        PlayAndStop(SeimeiAudio, 0.7f); // 生命の音声を再生
        yield return StartCoroutine(FadeInImage(GreenUI));

        yield return StartCoroutine(WaitSeconds(0.05f));
        PlayAndStop(SundaAoAudio, 0.7f); // 澄んだ青の音声を再生
        yield return StartCoroutine(FadeInImage(BlueUI));

        yield return StartCoroutine(WaitSeconds(0.05f));
        PlayAndStop(JyonetuAudio, 0.7f);
        yield return StartCoroutine(FadeInImage(RedUI));

    }

    private IEnumerator FadeOutThreeColorsBackground()
    {
        yield return new WaitForSeconds(0.3f); // 黒が広がるのを少し待つ

        Coroutine fade1 = StartCoroutine(FadeOutImage(SundaAo));
        Coroutine fade2 = StartCoroutine(FadeOutImage(Seimei));
        Coroutine fade3 = StartCoroutine(FadeOutImage(Jyonetu));

    }

    [SerializeField] private float DarkexpandScale = 10f;  // 拡大終了スケール（例：10倍）

    private IEnumerator ExpandDarkToFullScreen()
    {
        Dark.SetActive(true);
        RectTransform rt = Dark.GetComponent<RectTransform>();

        Vector3 startScale = new Vector3(2f, 2f, 1f);  // 開始スケール
        Vector3 endScale = new Vector3(10f, 7f, 1f);   // 終了スケール

        rt.localScale = startScale;

        float elapsed = 0f;

        while (elapsed < darkExpandSeconds)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / darkExpandSeconds);
            rt.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        rt.localScale = endScale;  // 最終スケールで確定
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

        yield return null; // 有効化反映待ち
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
        Debug.Log($"StopAudioAfterSeconds: {delay}秒後に停止予定。");
        yield return new WaitForSeconds(delay);

        if (source != null)
        {
            if (source.isPlaying)
            {
                Debug.Log($"StopAudioAfterSeconds: AudioSource '{source.gameObject.name}' を停止します。");
                source.Stop();
            }
            else
            {
                Debug.Log($"StopAudioAfterSeconds: AudioSource '{source.gameObject.name}' は既に停止しています。");
            }
        }
        else
        {
            Debug.LogWarning("StopAudioAfterSeconds: AudioSourceが破棄されています。");
        }
    }



    /*
    Element 1 世界は、かつて色で満ちていた。

//青、赤、緑が混ざった画像表示　

Element 2 空は澄んだ青、

//混ざった画像即消し
//左側に青 FadeIN

Element 3 森は生命の緑、

//真ん中に緑 F IN

Element 4 大地は情熱の赤に燃えていた。

//右に赤を  IN

Element 5 だが、ある時――黒が現れた。

//真ん中に丸い黒

Element 6 それは色を喰らい、

//黒が画面全体に広がる  徐々に

Element 7 意味を奪い、

//No Action

Element 8 世界を"単調"に染め上げていった。

//画面の画像を全て消して真っ黒にする  

Element 9 三原色と呼ばれる存在たちは、黒に抗おうとした。

//一斉に青、赤、緑の正方形がフィードイン

Element 10 だが力及ばず、彼らは封印され、

//青、赤、緑が消える

Element 11 世界は静寂と虚無に覆われる。

//真っ黒

Element 12 …そして今、忘れられた色たちが、再び目を覚ます。

//青が、左側にフィードイン
//0.1待機
//赤が真ん中　0.2,,,,
//0.1待機
//緑

Element 13 争うためか、取り戻すためか。
    */

}

