using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


// Switched to using PlayerPrefs as these are easier to handle currently

// Notes for testing
    // The first time you play in the inspector will be like when you open the first application window
    // The second time you play in the inspector will be like when you open the second window
    // Third time you play, like when you open a third window

    // Stopping the game and then clicking "refresh" on the GameRefresher in the InstanceLinker object will act as if you are exiting the game exe.
    // This means the next time you click play, the game will start the same way it started last time you clicked play
    // There's a bug catch for if the saved data says both other instances are active but there is only the superfluous instance running, this wipes the save data

    // Stopping the game and then clicking "clear data" on the GameRefresher in the InstanceLinker object will delete all save data for convinience





// TODO: may want to lock player controls & movement whilst refreshing is occuring








[ExecuteInEditMode]
public class GameLoader : MonoBehaviour
{
    public Transform PlayerShip;
    public SpriteRenderer PlayerShipRenderer;
    public Rigidbody2D PlayerBod;

    public Transform OtherInstanceShip;
    public SpriteRenderer OtherInstanceRenderer;
    public Rigidbody2D OtherInstanceBod;

    public Transform CameraTransform;

    public InstanceData CurrentInstance;

    [Header("dialogue parts")]
    public C_RunnerBean Instance1Start;
    public C_RunnerBean Instance2Start;
    public C_RunnerBean ExtraInstanceStart;
    public C_RunnerBean GameEndedFromShipCrash;
    public C_RunnerBean GameEndedFromOtherInstance;


    private IEnumerator _instanceLoadCoroutine;
    private bool _started = false;

    public void CloseUp()
    {
        // deactivete the instance
        switch (CurrentInstance.ID)
        {
            case 1:
                PlayerPrefs.SetInt(SaveDataLookup.Instance1_Active, ToInt(false));
                PlayerPrefs.SetInt(SaveDataLookup.Instance1_Saving, ToInt(false));

                // if the other instance isn't active, reset the GameComplete flag
                if (!ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance2_Active)))
                {
                    PlayerPrefs.SetInt(SaveDataLookup.GameComplete, ToInt(false));
                }

                break;
            case 2:
                PlayerPrefs.SetInt(SaveDataLookup.Instance2_Active, ToInt(false));
                PlayerPrefs.SetInt(SaveDataLookup.Instance2_Saving, ToInt(false));

                // if the other instance isn't active, reset the GameComplete flag
                if (!ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance1_Active)))
                {
                    PlayerPrefs.SetInt(SaveDataLookup.GameComplete, ToInt(false));
                }

                break;
            default:
                Process[] processInstances = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);

                if (processInstances.Length <= 1)
                {
                    // This has been picked up as a superfluous instance but it's the last instance being closed.
                    // As such, deleting the save data may be needed in case this data is corrupted (also it allows for testing in the editor without opening multiple editors)
                    UnityEngine.Debug.Log("Refreshing a superfluous instance but only one instance is active, deleting shared instance data");
                    PlayerPrefs.DeleteAll();
                }
                else
                {
                    UnityEngine.Debug.Log("There are more than 2 instances of the unity editor open, you may need to restart unity");
                }



                if (!ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance1_Active)) &&
                    !ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance2_Active)))
                {
                    // if the other instances are closed but higher instances are still closing, something may have messed up in the data. Wipe it so the game is playable again
                    
                }

                break;
        }
    }


   /* public void CheckInstance()
    {
        
    }*/



    // Start is called before the first frame update
    public void Start()
    {
        if (Application.isPlaying)
        {
            _started = true;
            //_currentInstance = new InstanceData();

            // start of game
            // check if shared data exists yet, if no then init
            if (!PlayerPrefs.HasKey(SaveDataLookup.Instance1_Active))
            {
                InitSaveData();
            }

            // check instance of game
            if (!ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance1_Active)))
            {
                CurrentInstance.ID = 1;
            }
            else if (!ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance2_Active)))
            {
                CurrentInstance.ID = 2;
            }
            else
            {
                CurrentInstance.ID = 3;
            }

            StartUpInstance();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (_started)
        {
            if (focus)
            {
                if (ToBool(PlayerPrefs.GetInt(SaveDataLookup.GameComplete)) && (CurrentInstance.ID == 1 || CurrentInstance.ID == 2))
                {
                    // To do: trigger a cutscene?
                    PlayerPrefs.SetInt(SaveDataLookup.GameComplete, ToInt(false));
                    
                    // make sure to disable the intro cutscenes (which may still be running) before enabling the end cutscene
                    Instance1Start.enabled = false;
                    Instance2Start.enabled = false;
                    GameEndedFromShipCrash.enabled = false;

                    GameEndedFromOtherInstance.enabled = true;
                    //Application.Quit();
                }

                LoadData();
            }
            else
            {
                PlayerBod.velocity = Vector2.zero;
                PlayerBod.angularVelocity = 0;
                OtherInstanceBod.velocity = Vector2.zero;
                OtherInstanceBod.angularVelocity = 0;

                SaveData();
            }
        }
    }


    void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("Application quit called!");
        // This will be called in the game, but it's hard to test the code if it refreshes every time I click stop game in unity
#if !UNITY_EDITOR
        CloseUp();
#endif
    }




    private void StartUpInstance()
    {
        // set up game data
        switch (CurrentInstance.ID)
        {
            case 1:
                PlayerPrefs.SetInt(SaveDataLookup.Instance1_Active, ToInt(true));
                PlayerPrefs.SetInt(SaveDataLookup.Instance1_Saving, ToInt(true));

                // set up instance 1 ship as player
                PlayerShip.position = Vector2.zero;
                PlayerShipRenderer.color = Color.red;

                OtherInstanceRenderer.color = Color.blue;

                // set up instance 1 music?

                if (ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance2_Active)))
                {
                    // Instance 2 is open, activete the ship to find and set up as instance 2 ship
                    OtherInstanceShip.position = new Vector2(
                        PlayerPrefs.GetFloat(SaveDataLookup.Instance2_XPos),
                        PlayerPrefs.GetFloat(SaveDataLookup.Instance2_YPos)
                        );

                    OtherInstanceShip.eulerAngles = new Vector3(
                        0,
                        0,
                        PlayerPrefs.GetFloat(SaveDataLookup.Instance2_Rotation)
                        );

                    // position instance 1 ship near instance 2 ship
                    PlayerShip.position = new Vector2(OtherInstanceShip.position.x - 5f, OtherInstanceShip.position.y);

                    OtherInstanceShip.gameObject.SetActive(true);
                }

                CameraTransform.position = PlayerShip.position;

                Instance1Start.enabled = true;

                break;
            case 2:
                PlayerPrefs.SetInt(SaveDataLookup.Instance2_Active, ToInt(true));
                PlayerPrefs.SetInt(SaveDataLookup.Instance2_Saving, ToInt(true));

                // set up instance 2 ship as player
                PlayerShip.position = Vector2.zero;
                PlayerShipRenderer.color = Color.blue;

                OtherInstanceRenderer.color = Color.red;

                // set up instance 2 music?

                if (ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance1_Active)))
                {
                    // Instance 1 is open, activete the ship to find and set up as instance 1 ship
                    OtherInstanceShip.position = new Vector2(
                        PlayerPrefs.GetFloat(SaveDataLookup.Instance1_XPos),
                        PlayerPrefs.GetFloat(SaveDataLookup.Instance1_YPos)
                        );

                    OtherInstanceShip.eulerAngles = new Vector3(
                        0,
                        0,
                        PlayerPrefs.GetFloat(SaveDataLookup.Instance1_Rotation)
                        );

                    // position instance 2 ship near instance 1 ship
                    PlayerShip.position = new Vector2(OtherInstanceShip.position.x - 5f, OtherInstanceShip.position.y);

                    OtherInstanceShip.gameObject.SetActive(true);
                }

                CameraTransform.position = PlayerShip.position;

                Instance2Start.enabled = true;

                break;
            case 3:
                UnityEngine.Debug.Log("Both instances are active. Stop the game, go to the InstanceLinker and click refresh");

                ExtraInstanceStart.enabled = true;

                //Application.Quit(); //Todo: maybe trigger a cutscene or something instead of just instantly quitting

                break;
        }
    }


    private void LoadData()
    {
        // deactivete the instance
        switch (CurrentInstance.ID)
        {
            case 1:
                // set saving to true here, then data will only be loaded from the other instance once this instance has been saved in SaveData
                PlayerPrefs.SetInt(SaveDataLookup.Instance1_Saving, ToInt(true));
                _instanceLoadCoroutine = LoadInstance2Data();
                StartCoroutine(_instanceLoadCoroutine);

                break;
            case 2:
                // set saving to true here, then data will only be loaded from the other instance once this instance has been saved in SaveData
                PlayerPrefs.SetInt(SaveDataLookup.Instance2_Saving, ToInt(true));
                _instanceLoadCoroutine = LoadInstance1Data();
                StartCoroutine(_instanceLoadCoroutine);

                break;
        }
    }

    private void SaveData()
    {
        // stop the data loading coroutine to avoid issues from flicking between instances quickly
        if (_instanceLoadCoroutine != null)
        {
            StopCoroutine(_instanceLoadCoroutine);
        }

        // deactivete the instance
        switch (CurrentInstance.ID)
        {
            case 1:
                PlayerPrefs.SetFloat(SaveDataLookup.Instance1_XPos, PlayerShip.position.x);
                PlayerPrefs.SetFloat(SaveDataLookup.Instance1_YPos, PlayerShip.position.y);
                PlayerPrefs.SetFloat(SaveDataLookup.Instance1_Rotation, PlayerShip.eulerAngles.z);

                // after data is saved, set flag to confirm data is saved
                PlayerPrefs.SetInt(SaveDataLookup.Instance1_Saving, ToInt(false)); 

                break;
            case 2:
                PlayerPrefs.SetFloat(SaveDataLookup.Instance2_XPos, PlayerShip.position.x);
                PlayerPrefs.SetFloat(SaveDataLookup.Instance2_YPos, PlayerShip.position.y);
                PlayerPrefs.SetFloat(SaveDataLookup.Instance2_Rotation, PlayerShip.eulerAngles.z);

                // after data is saved, set flag to confirm data is saved
                PlayerPrefs.SetInt(SaveDataLookup.Instance2_Saving, ToInt(false));

                break;
        }
    }


    private IEnumerator LoadInstance2Data()
    {
        // wait for instance 2 to be saved
        while (ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance2_Saving)))
        {
            yield return null;
        }

        // set instance 2 ship position
        OtherInstanceShip.position = new Vector2(
            PlayerPrefs.GetFloat(SaveDataLookup.Instance2_XPos),
            PlayerPrefs.GetFloat(SaveDataLookup.Instance2_YPos)
            );

        OtherInstanceShip.eulerAngles = new Vector3(
            0,
            0,
            PlayerPrefs.GetFloat(SaveDataLookup.Instance2_Rotation)
            );

        if (ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance2_Active)))
        {
            OtherInstanceShip.gameObject.SetActive(true);
        }
        else
        {
            OtherInstanceShip.gameObject.SetActive(false);
        }
    }


    private IEnumerator LoadInstance1Data()
    {
        // wait for instance 1 to be saved
        while (ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance1_Saving)))
        {
            yield return null;
        }

        // set instance 1 ship position
        OtherInstanceShip.position = new Vector2(
            PlayerPrefs.GetFloat(SaveDataLookup.Instance1_XPos),
            PlayerPrefs.GetFloat(SaveDataLookup.Instance1_YPos)
            );

        OtherInstanceShip.eulerAngles = new Vector3(
            0,
            0,
            PlayerPrefs.GetFloat(SaveDataLookup.Instance1_Rotation)
            );

        if (ToBool(PlayerPrefs.GetInt(SaveDataLookup.Instance1_Active)))
        {
            OtherInstanceShip.gameObject.SetActive(true);
        }
        else
        {
            OtherInstanceShip.gameObject.SetActive(false);
        }
    }


    private void InitSaveData()
    {
        PlayerPrefs.SetInt(SaveDataLookup.Instance1_Active, ToInt(false));
        PlayerPrefs.SetFloat(SaveDataLookup.Instance1_XPos, 0);
        PlayerPrefs.SetFloat(SaveDataLookup.Instance1_YPos, 0);
        PlayerPrefs.SetFloat(SaveDataLookup.Instance1_Rotation, 0);
        PlayerPrefs.SetInt(SaveDataLookup.Instance1_Saving, ToInt(false));


        PlayerPrefs.SetInt(SaveDataLookup.Instance2_Active, ToInt(false));
        PlayerPrefs.SetFloat(SaveDataLookup.Instance2_XPos, 0);
        PlayerPrefs.SetFloat(SaveDataLookup.Instance2_YPos, 0);
        PlayerPrefs.SetFloat(SaveDataLookup.Instance2_Rotation, 0);
        PlayerPrefs.SetInt(SaveDataLookup.Instance2_Saving, ToInt(false));

        PlayerPrefs.SetInt(SaveDataLookup.GameComplete, ToInt(false));
    }


    private int ToInt(bool b)
    {
        // 0 is false in C
        return b == false ? 0 : 1;
    }

    private bool ToBool(int i)
    {
        // any other number is true in C
        return i != 0;
    }
}



// use dataLookup class to avoid silly mistakes from string misspellings
public static class SaveDataLookup
{
    public static string Instance1_Active = "Instance1Active";
    public static string Instance1_XPos = "Instance1XPos";
    public static string Instance1_YPos = "Instance1YPos";
    public static string Instance1_Rotation = "Instance1Rotation";
    public static string Instance1_Saving = "Instance1Saving";


    public static string Instance2_Active = "Instance2Active";
    public static string Instance2_XPos = "Instance2XPos";
    public static string Instance2_YPos = "Instance2YPos";
    public static string Instance2_Rotation = "Instance2Rotation";
    public static string Instance2_Saving = "Instance2Saving";

    public static string GameComplete = "GameComplete"; // for when the game has been finished in another window
}


