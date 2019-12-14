// --> Game Script with save funcitons

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public List<GameObject> bullets = new List<GameObject>();
    public int shots = 0;
    public int hits = 0;

    [SerializeField]
    private Text hitsText;
    [SerializeField]
    private Text shotsText;
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject[] targets;
    private bool isPaused = false;

    private void Awake()
    {
        Pause();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Pause()
    {
        menu.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Unpause()
    {
        menu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
        isPaused = false;
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }
    
    // Function that creates a local save of a game session
    
    public void SaveGame()
    {
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        // 3
        hits = 0;
        shots = 0;
        shotsText.text = "Shots: " + shots;
        hitsText.text = "Hits: " + hits;

        ClearRobots();
        ClearBullets();
        Debug.Log("Game Saved");
    }

    public void NewGame()
    {
        hits = 0;
        shots = 0;
        shotsText.text = "Shots: " + shots;
        hitsText.text = "Hits: " + hits;

        ClearRobots();
        ClearBullets();
        RefreshRobots();

        Unpause();
    }

    private void ClearRobots()
    {
        foreach (GameObject target in targets)
        {
            target.GetComponent<Target>().DisableRobot();
        }
    }

    private void ClearBullets()
    {
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }

    private void RefreshRobots()
    {
        foreach (GameObject target in targets)
        {
            target.GetComponent<Target>().RefreshTimers();
        }
    }
    
    //Stores saved game session
    
    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        int i = 0;
        foreach (GameObject targetGameObject in targets)
        {
            Target target = targetGameObject.GetComponent<Target>();
            if (target.activeRobot != null)
            {
                save.livingTargetPositions.Add(target.position);
                save.livingTargetsTypes.Add((int)target.activeRobot.GetComponent<Robot>().type);
                i++;
            }
        }

        save.hits = hits;
        save.shots = shots;

        return save;
    }

    public void AddShot()
    {
        shots++;
        shotsText.text = "Shots: " + shots;
    }

    public void AddHit()
    {
        hits++;
        hitsText.text = "Hits: " + hits;
    }
    
    //Loads saved game session
    
    public void LoadGame()
    {
        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            ClearBullets();
            ClearRobots();
            RefreshRobots();

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3
            for (int i = 0; i < save.livingTargetPositions.Count; i++)
            {
                int position = save.livingTargetPositions[i];
                Target target = targets[position].GetComponent<Target>();
                target.ActivateRobot((RobotTypes)save.livingTargetsTypes[i]);
                target.GetComponent<Target>().ResetDeathTimer();
            }

            // 4
            shotsText.text = "Shots: " + save.shots;
            hitsText.text = "Hits: " + save.hits;
            shots = save.shots;
            hits = save.hits;

            Debug.Log("Game Loaded");

            Unpause();
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    // Function that saves a game session as a JSON file
    
    public void SaveAsJSON()
    {
        Save save = CreateSaveGameObject();
        string json = JsonUtility.ToJson(save);

        Debug.Log("Saving as JSON: " + json);
    }
}

// --> Save script file

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

// Stores all relevant game data of the current session 

public class Save
{
    public List<int> livingTargetPositions = new List<int>();
    public List<int> livingTargetsTypes = new List<int>();

    public int hits = 0;
    public int shots = 0;
}

// --> Branches and Loops

//Page 1

int a = 5;
int b = 6;
if (a + b > 10)
    Console.WriteLine("The answer is greater than 10.");
    
int b = 3;

//Page 2

int a = 5;
int b = 3;
if (a + b > 10)
    Console.WriteLine("The answer is greater than 10");
else
    Console.WriteLine("The answer is not greater than 10");

int a = 5;
int b = 3;
if (a + b > 10)
{
    Console.WriteLine("The answer is greater than 10");
}
else
{
    Console.WriteLine("The answer is not greater than 10");
}

int a = 5;
int b = 3;
int c = 4;
if ((a + b + c > 10) && (a == b))
{
    Console.WriteLine("The answer is greater than 10");
    Console.WriteLine("And the first number is equal to the second");
}
else
{
    Console.WriteLine("The answer is not greater than 10");
    Console.WriteLine("Or the first number is not equal to the second");
}

int a = 5;
int b = 3;
int c = 4;
if ((a + b + c > 10) || (a == b))
{
    Console.WriteLine("The answer is greater than 10");
    Console.WriteLine("Or the first number is equal to the second");
}
else
{
    Console.WriteLine("The answer is not greater than 10");
    Console.WriteLine("And the first number is not equal to the second");
}

//Page 3

int counter = 0;
while (counter < 10)
{
  Console.WriteLine($"Hello World! The counter is {counter}");
  counter++;
}

int counter = 0;
do
{
  Console.WriteLine($"Hello World! The counter is {counter}");
  counter++;
} while (counter < 10);

//Page 4

for(int counter = 0; counter < 10; counter++)
{
  Console.WriteLine($"Hello World! The counter is {counter}");
}

//Page 5

%
if
for

//Page 6

int sum = 0;
for (int number = 1; number < 21; number++)
{
  if (number % 3 == 0)
  {
    sum = sum + number;
  }
}
Console.WriteLine($"The sum is {sum}");

// --> Numbers in C#

//Page 1

int a = 18;
int b = 6;
int c = a + b;
Console.WriteLine(c);

int c = a - b;
int c = a * b;
int c = a / b;

//Page 2

int a = 5;
int b = 4;
int c = 2;
int d = a + b * c;
Console.WriteLine(d);

int a = 5;
int b = 4;
int c = 2;
int d = (a + b) * c;
Console.WriteLine(d);

int d = (a + b) - 6 * c + (12 * 4) / 3 + 12;

int a = 7;
int b = 4;
int c = 3;
int d = (a  + b) / c;
Console.WriteLine(d);

//Page 3

int a = 7;
int b = 4;
int c = 3;
int d = (a  + b) / c;
int e = (a + b) % c;
Console.WriteLine($"quotient: {d}");
Console.WriteLine($"remainder: {e}");

int max = int.MaxValue;
int min = int.MinValue;
Console.WriteLine($"The range of integers is {min} to {max}");

int what = max + 3;
Console.WriteLine($"An example of overflow: {what}");

// Page 4

double a = 5;
double b = 4;
double c = 2;
double d = (a  + b) / c;
Console.WriteLine(d);

double a = 19;
double b = 23;
double c = 8;
double d = (a  + b) / c;
Console.WriteLine(d);

double max = double.MaxValue;
double min = double.MinValue;
Console.WriteLine($"The range of double is {min} to {max}");

double third = 1.0 / 3.0;
Console.WriteLine(third);

//Page 5

decimal min = decimal.MinValue;
decimal max = decimal.MaxValue;
Console.WriteLine($"The range of the decimal type is {min} to {max}");

double a = 1.0;
double b = 3.0;
Console.WriteLine(a / b);

decimal c = 1.0M;
decimal d = 3.0M;
Console.WriteLine(c / d);

//Page 6

double radius = 2.50;
double area = Math.PI * radius * radius;
Console.WriteLine(area);

// --> Pasing and formats

//Parsing and Format Providers

using System;
using System.Globalization;

public class Example
{
   public static void Main()
   {
      string[] values = { "1,304.16", "$1,456.78", "1,094", "152", 
                          "123,45 €", "1 304,16", "Ae9f" };
      double number;
      CultureInfo culture = null;
      
      foreach (string value in values) {
         try {
            culture = CultureInfo.CreateSpecificCulture("en-US");
            number = Double.Parse(value, culture);
            Console.WriteLine("{0}: {1} --> {2}", culture.Name, value, number);
         }   
         catch (FormatException) {
            Console.WriteLine("{0}: Unable to parse '{1}'.", 
                              culture.Name, value);
            culture = CultureInfo.CreateSpecificCulture("fr-FR");
            try {
               number = Double.Parse(value, culture);
               Console.WriteLine("{0}: {1} --> {2}", culture.Name, value, number);
            }
            catch (FormatException) {
               Console.WriteLine("{0}: Unable to parse '{1}'.", 
                                 culture.Name, value);
            }
         }
         Console.WriteLine();
      }   
   }
}
// The example displays the following output:
//    en-US: 1,304.16 --> 1304.16
//    
//    en-US: Unable to parse '$1,456.78'.
//    fr-FR: Unable to parse '$1,456.78'.
//    
//    en-US: 1,094 --> 1094
//    
//    en-US: 152 --> 152
//    
//    en-US: Unable to parse '123,45 €'.
//    fr-FR: Unable to parse '123,45 €'.
//    
//    en-US: Unable to parse '1 304,16'.
//    fr-FR: 1 304,16 --> 1304.16
//    
//    en-US: Unable to parse 'Ae9f'.
//    fr-FR: Unable to parse 'Ae9f'.

//Parsing and NumberStyles Values

using System;
using System.Globalization;

public class Example
{
   public static void Main()
   {
      string value = "1,304";
      int number;
      IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-US");
      if (Int32.TryParse(value, out number))
         Console.WriteLine("{0} --> {1}", value, number);
      else
         Console.WriteLine("Unable to convert '{0}'", value);
            
      if (Int32.TryParse(value, NumberStyles.Integer | NumberStyles.AllowThousands, 
                        provider, out number))
         Console.WriteLine("{0} --> {1}", value, number);
      else
         Console.WriteLine("Unable to convert '{0}'", value);
   }
}
// The example displays the following output:
//       Unable to convert '1,304'
//       1,304 --> 1304

//Parsing and Unicode Digits

using System;

public class Example
{
   public static void Main()
   {
      string value;
      // Define a string of basic Latin digits 1-5.
      value = "\u0031\u0032\u0033\u0034\u0035";
      ParseDigits(value);

      // Define a string of Fullwidth digits 1-5.
      value = "\uFF11\uFF12\uFF13\uFF14\uFF15";
      ParseDigits(value);
      
      // Define a string of Arabic-Indic digits 1-5.
      value = "\u0661\u0662\u0663\u0664\u0665";
      ParseDigits(value);
      
      // Define a string of Bangla digits 1-5.
      value = "\u09e7\u09e8\u09e9\u09ea\u09eb";
      ParseDigits(value);
   }

   static void ParseDigits(string value)
   {
      try {
         int number = Int32.Parse(value);
         Console.WriteLine("'{0}' --> {1}", value, number);
      }   
      catch (FormatException) {
         Console.WriteLine("Unable to parse '{0}'.", value);      
      }     
   }
}
// The example displays the following output:
//       '12345' --> 12345
//       Unable to parse '１２３４５'.
//       Unable to parse '١٢٣٤٥'.
//       Unable to parse '১২৩৪৫'.

// --> Parsing Strings using String.Split

//1

string phrase = "The quick brown fox jumps over the lazy dog.";
string[] words = phrase.Split(' ');

foreach (var word in words)
{
    System.Console.WriteLine($"<{word}>");
}

//2

string phrase = "The quick brown    fox     jumps over the lazy dog.";
string[] words = phrase.Split(' ');

foreach (var word in words)
{
    System.Console.WriteLine($"<{word}>");
}

//3

char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

string text = "one\ttwo three:four,five six seven";
System.Console.WriteLine($"Original text: '{text}'");

string[] words = text.Split(delimiterChars);
System.Console.WriteLine($"{words.Length} words in text:");

foreach (var word in words)
{
    System.Console.WriteLine($"<{word}>");
}

//4

char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

string text = "one\ttwo :,five six seven";
System.Console.WriteLine($"Original text: '{text}'");

string[] words = text.Split(delimiterChars);
System.Console.WriteLine($"{words.Length} words in text:");

foreach (var word in words)
{
    System.Console.WriteLine($"<{word}>");
}


//5

string[] separatingStrings = { "<<", "..." };

string text = "one<<two......three<four";
System.Console.WriteLine($"Original text: '{text}'");

string[] words = text.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
System.Console.WriteLine($"{words.Length} substrings in text:");

foreach (var word in words)
{
    System.Console.WriteLine(word);
}
