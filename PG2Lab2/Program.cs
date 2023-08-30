using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace PG2Lab2
{
    class Program
    {
        public static void Main(string[] args)
        {
            string filePath = "inputFile.csv";
            bool dontQuit = true;
            List<string> titles = ReadFile(filePath);
            List<string> bubbleTitles = BubbleSort(titles);
            List<string> bubbleReverse = BubbleSort(titles, true);
            List<string> mergeTitles = MergeSort(titles);
            bool reverse = false;
            while (dontQuit)
            {
                Console.Clear();
                int selection = Menu();
                switch (selection)
                {
                    case 0:
                        {
                            dontQuit = false;
                            break;
                        }
                    case 1:
                        {
                            if (!reverse)
                            {
                                for (int i = 0; i < titles.Count; i++)
                                {
                                    Console.CursorLeft = 10;
                                    Console.Write($"{titles[i]}");
                                    Console.CursorLeft = 60;
                                    Console.WriteLine($"{bubbleReverse[i]}");
                                }
                                reverse = true;
                            }
                            else
                            {
                                for (int i = 0; i < titles.Count; i++)
                                {
                                    Console.CursorLeft = 10;
                                    Console.Write($"{titles[i]}");
                                    Console.CursorLeft = 60;
                                    Console.WriteLine($"{bubbleTitles[i]}");
                                }
                                reverse = false;
                            }
                            
                            Console.ReadKey();
                            break;
                        }
                    case 2:
                        {
                            for (int i = 0; i < titles.Count; i++)
                            {
                                Console.CursorLeft = 10;
                                Console.Write($"{titles[i]}");
                                Console.CursorLeft = 60;
                                Console.WriteLine($"{mergeTitles[i]}");
                            }
                            Console.ReadKey();
                            break;
                        }
                    case 3:
                        {
                            List<string> binaryList = new List<string>(titles);
                            binaryList.Sort();
                            for (int i = 0; i < binaryList.Count; i++)
                            {
                                int foundIndex = BinarySearch(binaryList, binaryList[i], 0, binaryList.Count);
                                Console.CursorLeft = 10;
                                Console.Write($"{binaryList[i]}");
                                Console.CursorLeft = 70;
                                Console.Write($"Index: {i}");
                                Console.CursorLeft = 90;
                                Console.WriteLine($"Found Index: {foundIndex}");
                            }
                            Console.ReadKey();
                            break;
                        }
                    case 4:
                        {
                            string input = ReadString("Enter the name of the save file.");
                            if (Path.HasExtension(input))
                            {
                                input = Path.ChangeExtension(input, ".json");
                            }
                            else
                            {
                                input += ".json";
                            }
                            Serialize(input, titles);
                            break;
                        }

                }

            }
        }
        public static int Input(string prompt)
        {
            Console.Write(prompt);
            while (true)
            {
                if (Int32.TryParse(Console.ReadLine(), out int selection))
                {
                    return selection;
                }
                else
                {
                    Console.WriteLine("Invalid input, try again.\n");
                }
            }
        }
        public static string ReadString(string prompt)
        {
            string input;
            Console.WriteLine(prompt);
            input = Console.ReadLine();
            return input;
        }
        public static int Menu()
        {
            Console.WriteLine(" 1: Bubble Sort\n 2: Merge Sort\n 3: Binary Search\n 4: Save\n 0: Exit");
            int selection = Input("What would you like to do? ");
            return selection;
        }

        public static List<string> ReadFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string temp = sr.ReadToEnd();
                string[] tempArr = temp.Split(',');
                List<string> titles = tempArr.ToList();
                return titles;
            }
        }

        public static List<string> Swap(List<string> needSwapped, int i)
        {
            string temp = needSwapped[i];
            needSwapped[i] = needSwapped[i + 1];
            needSwapped[i + 1] = temp;
            return needSwapped;
        }

        public static List<string> BubbleSort(List<string> unsortedTitles, bool reverse = false)
        {
            List<string> sortedBubble = new List<string>(unsortedTitles);
            int n = unsortedTitles.Count;
            bool swapped;
            if (reverse)
            {
                do
                {
                    swapped = true;
                    for (int i = 1; i <= n - 1; i++)
                    {
                        for (int j = 0; j < n - 1; j++)
                        {
                            if (sortedBubble[j + 1].CompareTo(sortedBubble[j]) < 0)
                            {
                                sortedBubble = Swap(sortedBubble, j);
                                swapped = true;
                            }
                            if (!swapped)
                            {
                                break;
                            }
                        }
                    }
                    n -= 1;
                } while (!swapped);
                return sortedBubble;
            }
            do
            {
                swapped = true;
                for (int i = 1; i <= n - 1; i++)
                {
                    for (int j = 0; j < n - 1; j++)
                    {
                        if (sortedBubble[j + 1].CompareTo(sortedBubble[j]) > 0)
                        {
                            sortedBubble = Swap(sortedBubble, j);
                            swapped = true;
                        }
                        if (!swapped)
                        {
                            break;
                        }
                    }
                }
                n -= 1;
            } while (!swapped);
            return sortedBubble;
        }

        public static List<string> MergeSort(List<string> unsortedTitles)
        {
            if (unsortedTitles.Count <= 1)
            {
                return unsortedTitles;
            }
            //recursive case.
            List<string> left = new List<string>();
            List<string> right = new List<string>();
            int mid = unsortedTitles.Count / 2;
            for (int i = 0; i < unsortedTitles.Count; i++)
            {
                if(i < mid)
                {
                    left.Add(unsortedTitles[i]);
                }
                else
                {
                    right.Add(unsortedTitles[i]);
                }
            }
            left = MergeSort(left);
            right = MergeSort(right);

            return Merge(left, right);
        }

        public static List<string> Merge(List<string> left, List<string> right, bool reverse = false)
        {
            List<string> result = new List<string>();
            while (left.Count > 0  && right.Count > 0)
            {
                if (left[0].CompareTo(right[0]) <= 0)
                {
                    result.Add(left[0]);
                    left.RemoveAt(0);
                }
                else
                {
                    result.Add(right[0]);
                    right.RemoveAt(0);
                }
            }
            while (left.Count > 0)
            {
                result.Add(left[0]);
                left.RemoveAt(0);
            }
            while (right.Count > 0)
            {
                result.Add(right[0]);
                right.RemoveAt(0);
            }
            return result;
            
        }
        public static int BinarySearch(List<string> sortedList, string searchTerm, int low, int high)
        {
            if (high < low)
            {
                return -1; //not found
            }
            int mid = (low + high) / 2;
            int compareResult = sortedList[mid].CompareTo(searchTerm);
            if (compareResult > 0)
            {
                return BinarySearch(sortedList, searchTerm, low, mid - 1);
            }
            else if (compareResult < 0)
            {
                return BinarySearch(sortedList, searchTerm, mid + 1, high);
            }
            else
            {
                return mid;
            }
        }

        public static void Serialize(string fname, List<string> unsortedList)
        {
            List<string> savedList = MergeSort(unsortedList);
            using (StreamWriter writer = File.CreateText(fname))
            {
                using (JsonTextWriter jWriter = new JsonTextWriter(writer))
                {
                    jWriter.Formatting = Formatting.Indented;
                    JsonSerializer ser = new JsonSerializer();
                    ser.Serialize(jWriter, savedList);
                    jWriter.Flush();
                }
            }

        }
    }
}
