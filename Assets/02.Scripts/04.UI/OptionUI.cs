using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [FoldoutGroup("��� �ٿ�"),SerializeField]
    [LabelText("�ػ� ��Ӵٿ�")] private TMP_Dropdown resolutionDropdown;
    private List<Resolution> uniqueResolutions;
    [FoldoutGroup("��� �ٿ�"), SerializeField]
    [LabelText("������ ��Ӵٿ�")] private TMP_Dropdown frameRateDropdown;
    private readonly List<int> frameRates = new List<int> { 30, 45, 60, 75, 120, 144, 240 };
    [FoldoutGroup("��� �ٿ�"), SerializeField]
    [LabelText("â��� ��Ӵٿ�")] private TMP_Dropdown ScreenModeDropdown;

    public enum ScreenMode
    {
        FullScreenWindow = 0,
        Window = 1
    }

    int resolutionIndex;
    int frameRateIndex;
    int screenModeIndex;

    //==========================================================

    void Start()
    {
        ClearResolution();
        ClearFrame();
        ClearScreenMode();

        SetInitialResolution();
        SaveOption();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveBtn();
        }
    }
    //==========================================================

    #region Resolution

    void ClearResolution()
    {
        Resolution[] allResolutions = Screen.resolutions;
        uniqueResolutions = allResolutions.Distinct(new ResolutionComparer()).ToList();
        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            string option = uniqueResolutions[i].width + " x " + uniqueResolutions[i].height;
            resolutionOptions.Add(option);

            if (uniqueResolutions[i].width == Screen.currentResolution.width &&
                uniqueResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = PlayerPrefs.GetInt("resolution", currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        // �ػ� �� ���� �� ����
        Resolution resolution = uniqueResolutions[resolutionIndex];
        this.resolutionIndex = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void SetInitialResolution()
    {
        // ����� �ػ� ���� ������ �� ������ �����ϰ�, ������ ���� �ػ󵵷� ����
        int savedResolutionIndex = PlayerPrefs.GetInt("resolution", -1);

        if (savedResolutionIndex == -1)
        {
            // ����� ���� ���� ��� ���� ����� �ػ󵵷� ����
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);

            // ���� �ػ󵵸� PlayerPrefs�� ����
            for (int i = 0; i < uniqueResolutions.Count; i++)
            {
                if (uniqueResolutions[i].width == currentResolution.width && uniqueResolutions[i].height == currentResolution.height)
                {
                    this.resolutionIndex = i;
                    break;
                }
            }
        }
        else
        {
            // ����� ���� ���� ��� �� ������ ����
            SetResolution(savedResolutionIndex);
        }
    }

    #endregion

    #region FrameRate
    void ClearFrame()
    {
        frameRateDropdown.ClearOptions();
        List<string> frameRateOptions = frameRates.ConvertAll(rate => rate + " FPS");
        frameRateDropdown.AddOptions(frameRateOptions);

        int savedFrameRateIndex = PlayerPrefs.GetInt("frameRate", -1);
        if (savedFrameRateIndex == -1)
        {
            // ����� ������ ����Ʈ ���� ������ �⺻������ 60������ ����
            savedFrameRateIndex = frameRates.IndexOf(60);
            frameRateIndex = savedFrameRateIndex;
            //PlayerPrefs.Save();
        }

        frameRateDropdown.value = savedFrameRateIndex;
        frameRateDropdown.RefreshShownValue();

        frameRateDropdown.onValueChanged.AddListener(SetFrameRate);

        SetFrameRate(savedFrameRateIndex);
    }

    public void SetFrameRate(int frameRateIndex)
    {
        // ������ ����Ʈ �� ���� �� ����
        int frameRate = frameRates[frameRateIndex];
        this.frameRateIndex = frameRateIndex;

        Application.targetFrameRate = frameRate;
    }
    #endregion

    #region ScreenMode
    void ClearScreenMode()
    {
        List<string> options = new List<string> { "��üȭ��", "â���" };

        ScreenModeDropdown.ClearOptions();
        ScreenModeDropdown.AddOptions(options);
        ScreenModeDropdown.onValueChanged.AddListener(index => ChangeFullScreenMode((ScreenMode)index));

        int screenModeIndex = PlayerPrefs.GetInt("screenMode", -1);
        if (screenModeIndex == -1)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            switch (screenModeIndex)
            {
                case 0:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
                    break;
                case 1:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.Windowed);
                    break;
            }
        }

    }

    private void ChangeFullScreenMode(ScreenMode mode)
    {
        switch (mode)
        {
            case ScreenMode.FullScreenWindow:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                screenModeIndex = (int)ScreenMode.FullScreenWindow;
                break;
            case ScreenMode.Window:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                screenModeIndex = (int)ScreenMode.Window;
                break;
        }
    }
    #endregion

    #region Btn
    public void SaveBtn()
    {
        SaveOption();
        UIManager.Instance.OptionUISet(false);
    }

    public void ResetBtn()
    {
        ResetOption();
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
    #endregion
    //==========================================================

    void SaveOption()
    {
        PlayerPrefs.SetInt("resolution", resolutionIndex);
        PlayerPrefs.SetInt("frameRate", frameRateIndex);
        PlayerPrefs.SetInt("screenMode", screenModeIndex);
        PlayerPrefs.Save();
    }

    void ResetOption()
    {
        resolutionIndex = PlayerPrefs.GetInt("resolution");
        frameRateIndex = PlayerPrefs.GetInt("frameRate");
        screenModeIndex = PlayerPrefs.GetInt("screenMode");

        SetResolution(resolutionIndex);
        resolutionDropdown.value = resolutionIndex;

        SetFrameRate(frameRateIndex);
        frameRateDropdown.value = frameRateIndex;

        ChangeFullScreenMode((ScreenMode)screenModeIndex);
        ScreenModeDropdown.value = screenModeIndex;
    }
}

//==========================================================

public class ResolutionComparer : IEqualityComparer<Resolution>
{
    public bool Equals(Resolution x, Resolution y)
    {
        return x.width == y.width && x.height == y.height;
    }

    public int GetHashCode(Resolution obj)
    {
        return obj.width.GetHashCode() ^ obj.height.GetHashCode();
    }
}