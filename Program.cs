using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Enter file location of input e.g C:\Files\input.txt :");
            String UserInputFile = Console.ReadLine();
            Console.WriteLine(@"Enter file location of output e.g C:\Files\output.txt:");
            String UserOutputFile = Console.ReadLine();
            Match MainInstance = new Match();
            MainInstance.Process(UserInputFile,UserOutputFile);
        }
    }
    class Match
    {
        public string Input;
        public string Output;
        private Dictionary<string,string> ScoreDict = new Dictionary<string, string>();
        private Dictionary<char, int> PlayerDict = new Dictionary<char, int>();
        private List<string> ReadList = new List<string>();

        /* private Init :
         * Input : None
         * Output : None
         * Description : Initialises ScoreDict and PlayerDict with values*/

        private void Init()
        {
            ScoreDict.Add("0", "15"); ScoreDict.Add("15", "30"); ScoreDict.Add("30", "40"); ScoreDict.Add("40", "A"); ScoreDict.Add("A", "40");
            PlayerDict.Add('A', 0); PlayerDict.Add('B', 1);
        }

        /* public Process :
         * Input : string UserInputFile, string UserOutputFile
         * Output : None
         * Description : This is the main method of the class. It assigns the parameters to local variables to store file locations and calls relevant methods to read and write files*/

        public void Process(string UserInputFile,string UserOutputFile)
        {
            Init();
            Input = UserInputFile;
            Output = UserOutputFile;
            ReadFile();
            WriteFile();
        }

        /* private ReadFile :
         * Input : None
         * Output : None
         * Description : Reads lines from file using stored file location (Input). Each line is appended to the list (ReadList)*/

        private void ReadFile()
        {
            string line;
            System.IO.StreamReader inputFile = new System.IO.StreamReader(Input);
            
            while ((line = inputFile.ReadLine()) != null)
            {
                ReadList.Add(line);
            }
            inputFile.Close(); 

        }

        /* private WriteFile :
         * Input : None
         * Output : None
         * Description : Opens file at stored file location (Output). For every line in (ReadList) it calls (LineLogic) and writes the result on the file.*/

        private void WriteFile()
        {
            System.IO.StreamWriter outputFile = new System.IO.StreamWriter(Output);

            foreach (var i in ReadList)
            {
                outputFile.WriteLine(LineLogic(i));
            }
            outputFile.Close();
        }

        /* private LineLogic :
         * Input : string InputLine
         * Output : string OutputLine
         * Description : Takes (InputLine), applies logic to produce correct output and outputs (OutputLine). Inline comments. Note I don't understand tennis so may confuse sets and matches*/

        private string LineLogic(string InputLine)
        {
            List<string> SetScore = new List<string>();
            List<List<int>> GameScore = new List<List<int>>();
            int MatchTracker = 0;
            bool OrderFlag = false;
            string OutputString = "";

            GameScore.Add(new List<int>());
            GameScore[0].Add(0); GameScore[0].Add(0);
            SetScore.Add("0"); SetScore.Add("0");

            foreach (var InputChar in InputLine)
            {
                if (!(InputChar.Equals("")))
                {
                    if (SetScore[PlayerDict[InputChar]] == "40" & ((SetScore[Math.Abs(PlayerDict[InputChar] - 1)]) == "A"))
                    { /* If set score is 40-A or A-40 and player with 40 scores then change A to 40*/
                        SetScore[Math.Abs(PlayerDict[InputChar] - 1)] = "40";
                    }
                    else if (SetScore[PlayerDict[InputChar]] == "A" || SetScore[PlayerDict[InputChar]] == "40" & (SetScore[Math.Abs(PlayerDict[InputChar] - 1)]) != "A" & (SetScore[Math.Abs(PlayerDict[InputChar] - 1)]) != "40")
                    { /* If a player scores and wins the match when the set score is 40-0,40-15,40-30,A-40, then update the end of GameScore which contains the current match wins. NOT OrderFlag to keep track of server*/
                        SetScore[0] = "0"; SetScore[1] = "0";
                        GameScore[MatchTracker][PlayerDict[InputChar]] += 1;
                        OrderFlag = !OrderFlag;

                        if ((GameScore[MatchTracker][0] >= 6 || GameScore[MatchTracker][1] >= 6) & Math.Abs(GameScore[MatchTracker][0] - GameScore[MatchTracker][1]) >= 2)
                        { /* If either players game wins is at least 6 and 2 clear games ahead of opponent then end match and create new match at end of GameScore 0 wins for both players*/
                            MatchTracker++;
                            GameScore.Add(new List<int>());
                            GameScore[MatchTracker].Add(0); GameScore[MatchTracker].Add(0);
                        }
                    }
                    else
                    { /* If no games or matches have been won then update set score for player that scored*/
                        SetScore[PlayerDict[InputChar]] = ScoreDict[SetScore[PlayerDict[InputChar]]];

                    }
                }

            }
            
            for (int I = 0; I < GameScore.Count; I++)
            { /* Format output string for previous games and current game*/
                if (OrderFlag)
                { /* Reverse order of scores if server is incorrect*/
                    GameScore[I].Reverse();
                }
                OutputString += GameScore[I][0].ToString() + "-" + GameScore[I][1].ToString() + " ";
            }
            if (SetScore[0] + SetScore[1] != "00")
            { /* If current set is not 0-0 then add to output string*/
                if (OrderFlag)
                { /* Reverse order of scores if server is incorrect*/
                    SetScore.Reverse();
                }
                OutputString += SetScore[0] + "-" + SetScore[1];
            }

            OutputString = OutputString.TrimEnd(' '); /* Trim trailing space char*/
            return OutputString;

        }
    }
}
