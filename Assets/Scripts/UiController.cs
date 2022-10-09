using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _gamePanel;
    private MainCharacterController _characterController;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _forceSlider;
    [SerializeField] private TextMeshProUGUI _bestKillsText;
    [SerializeField] private TextMeshProUGUI _killsText;
    [SerializeField] private Button _ultaButton;
    private int _bestKills;
    private int _kills;
    public static bool IsPaused;
    [Inject]
    private void Construct([Inject(Id = "characterController")] MainCharacterController characterController)
    {
        _characterController = characterController;
    }
    private void Start()
    {
        _bestKills = PlayerPrefs.GetInt("kills");
        _bestKillsText.text = _bestKills.ToString();
        _killsText.text = _kills.ToString();
        Play();
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0;
    }
    public void Play()
    {
        IsPaused = false;
        Time.timeScale = 1;
    }
    public void ForceUpdate(float force)
    {
        if (force == 100)
        {
            _ultaButton.interactable = true;
            force = 0;
        }
        else
        {
            _ultaButton.interactable = false;

        }
        _forceSlider.value = force;
    }
    public void HealthUpdate(float health)
    {
        _hpSlider.value = health;
    }
    public void KillsUpdate(int kill)
    {
        _kills += kill;
        if (_bestKills< _kills)
        {
            _bestKills = _kills;
            _bestKillsText.text = _bestKills.ToString();
            SaveKills();
        }
        _killsText.text = _kills.ToString();
    }
    public void LoseUI()
    {
        _losePanel.SetActive(true);
        _gamePanel.SetActive(false);
        Pause();
    }
    public void ShootUi()
    {
        _characterController.Shoot();
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void SaveKills()
    {
        PlayerPrefs.SetInt("kills", _bestKills);
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveKills();
        }
    }
}
