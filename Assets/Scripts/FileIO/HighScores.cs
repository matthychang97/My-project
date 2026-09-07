using UnityEngine;
using System.IO;

public class HighScores : MonoBehaviour
{
    public int[] scores = new int[10];

    string currentDirectory;

    public string scoreFileName = "highscores.txt";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Shows / print directory
        currentDirectory = Application.dataPath;
        Debug.Log("Our current directory is: " +  currentDirectory);

        //Load the scores by default
        LoadScoresFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadScoresFromFile();

            for (int i = 0; i < scores.Length; i++)
            {
                Debug.Log("High score " + i + ": " + scores[i]);
            }
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            SaveScoresToFile();
        }
    }

    public void LoadScoresFromFile()
    {
        //Check existance. Else abort
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if (fileExists == true)
        { 
            Debug.Log("Found high score file " + scoreFileName);
        }

        else
        {
            Debug.Log("The file " + scoreFileName + " does not exist. No scores will be loaded.", this);
            return;
        }

        //Make new array if default values. 
        scores = new int[scores.Length];

        StreamReader fileReader = new StreamReader(currentDirectory + "\\" + scoreFileName);

        //A counter to make sure we don't go past the end of our score
        int scoreCount = 0;

        //A while loop, runs as long as there is data to be read and we haven't reached the end of score array
        while (fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            //Read that line into a variable
            string fileLine = fileReader.ReadLine();

            //Try to parse that variable into an int
            int readScore = -1;
            //Try to parse it
            bool didParse = int.TryParse(fileLine, out readScore);
            if (didParse)
            {
                //If we read a number, put it in the array.
                scores[scoreCount] = readScore;
            }

            else
            {
                //If number couldn't be parsed then we had junk. Print error and use default.
                Debug.Log("Invalid line in scores file at " + scoreCount + ", using default value.", this);

                scores[scoreCount] = 0;
            }

            //Increment counter
            scoreCount++;
        }
        //close stream
        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);
    }

    public void SaveScoresToFile()
    {
        //create streamwriter for file path
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\" + scoreFileName);

        //Write line on file
        for (int i = 0; i < scores.Length; i++)
        { 
            fileWriter.WriteLine(scores[i]);
        }

        //Close stream
        fileWriter.Close();

        //Write log message
        Debug.Log("High scores written to " + scoreFileName);
        
    }

    public void AddScore(int newScore)
    {
        //Find index
        int desiredIndex = -1;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] < newScore || scores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }
        //If no index were found then score isn't high enough to get on table
        if (desiredIndex < 0)
        {
            Debug.Log("Score of " + newScore + " not high enough for high scores list.", this);
            return;
        }
        for (int i = scores.Length - 1; i > desiredIndex; i--)

        {
            scores[i] = scores[i - 1];
        }

        //Insert new score
        scores[desiredIndex] = newScore;
        Debug.Log("Score if " + newScore + " entered into high scores at position " + desiredIndex, this);
    }
}
