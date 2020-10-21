using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Lead Game Developer Test
    /// By: Kevin Mackey
    /// Quick Note:
    /// Would have liked to expand the code a bit more but already finnished requirements and running a little long
    /// </summary>
    // text for display on display page
    public UnityEngine.UI.Text text;

    // File path for persistant data
    private string FilePath { get { return Application.persistentDataPath + "/InputData.dat"; } }

    // use double to track button presses for longer
    private double RedPresses;
    private double BluePresses;
    private double GreenPresses;

    // Call load when scene is loading, and save when scene is changing
    private void OnEnable()
    {
        LoadData();
    }

    private void OnDisable()
    {
        SaveData();
    }

    private void Start()
    {
        if (text != null)
            UpdateText();
    }

    private void UpdateText()
    {
        text.text = string.Format("Here's how you did: \n\n red button presses: {0} \n\n blue button presses: {1} \n\n green button presses: {2}", RedPresses, BluePresses, GreenPresses);
    }

    // Create save and load funtions to allow more control when wanted
    private void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(FilePath);

        InputData data = new InputData();
        data.RedButtonPresses = RedPresses;
        data.BlueButtonPresses = BluePresses;
        data.GreenButtonPresses = GreenPresses;

        formatter.Serialize(file, data);
        file.Close();
    }

    private void LoadData()
    {
        if (File.Exists(FilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(FilePath, FileMode.Open);

            InputData data = (InputData)formatter.Deserialize(file);
            file.Close();

            RedPresses = data.RedButtonPresses;
            BluePresses = data.BlueButtonPresses;
            GreenPresses = data.GreenButtonPresses;
        }
    }

    // use button press as an over arching function to help with scalability 
    public void PressButton(string Action)
    {
        switch (Action)
        {
            case "Red":
                PressRed();
                break;
            case "Blue":
                PressBlue();
                break;
            case "Green":
                PressGreen();
                break;
            case "LoadNextScene":
                LoadNextScene();
                break;
            case "ClearData":
                ClearData();
                break;
            default:
                Debug.LogError(String.Format("{0} not a valid Button", Action));
                break;
        }
    }

    private void ClearData()
    {
        RedPresses = 0;
        BluePresses = 0;
        GreenPresses = 0;

        SaveData();
        UpdateText();
    }

    private void LoadNextScene()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void PressRed()
    {
        RedPresses++;
    }

    private void PressBlue()
    {
        BluePresses++;
    }

    private void PressGreen()
    {
        GreenPresses++;
    }
}

// Data to be saved
[Serializable]
class InputData
{
    public double RedButtonPresses;
    public double BlueButtonPresses;
    public double GreenButtonPresses;
}