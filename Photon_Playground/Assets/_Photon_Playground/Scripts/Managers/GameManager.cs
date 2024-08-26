using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] TMP_Text _consolePrint;
    [SerializeField] SPGameManager _sPGameManager;
    [SerializeField] MPGameManager _mPGameManger;

    bool isFauseGame;
    bool isFullscreen;

    // Set up internal variables and make all of cached GetComponent calls.
    void Awake()
    {
        if (PlayerPrefs.GetInt("IsSnglePlayer") == 1)
        {
            _sPGameManager.gameObject.SetActive(true);
        }
        else
        {
            _mPGameManger.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (PlayerPrefs.GetInt("IsSnglePlayer") == 1)
        {
            _consolePrint.text = "> Press Tab to open menu < \n" +
                "> Press F to Fullscreen < \n" +
                "> The game is still under development < \n";
        }
        else
        {
            _consolePrint.text = "> Waiting for other players < \n" + 
                "> Press Tab to open menu < \n" +
                "> Press F to Fullscreen < \n" +
                "> The game is still under development < \n";
        }

        isFauseGame = true;
        isFullscreen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isFauseGame)
            {
                Cursor.lockState = CursorLockMode.None;
                _pauseMenu.SetActive(true);

                //#if !UNITY_EDITOR && UNITY_WEBGL
                // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
                // WebGLInput.stickyCursorLock = false;
                //#endif
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                _pauseMenu.SetActive(false);

                //#if !UNITY_EDITOR && UNITY_WEBGL
                // disable WebGLInput.stickyCursorLock so if the browser unlocks the cursor (with the ESC key) the cursor will unlock in Unity
                //WebGLInput.stickyCursorLock = false;
                //#endif
            }
            isFauseGame = !isFauseGame;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isFullscreen)
            {
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreen = false;
            }
            isFullscreen = !isFullscreen;
        }
    }

    // Attempts to load main menu
    public void GoMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;

        if (PlayerPrefs.GetInt("IsSnglePlayer") == 1)
        {
            SceneManager.LoadScene(0);
        }

        else
        {
            _mPGameManger.LeaveRoom();
        }
    }
}
