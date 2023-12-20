namespace SEW3_5_3_Laufzeitanalyse_Playlist_gr1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int searchedDuration = 600;
            //Console.WriteLine("Geben Sie die gewünschte Laufzeit ein: ");
            //searchedDuration = Convert.ToInt32(Console.ReadLine());
            const int arrSize = 5;
            int[] arr = new int[arrSize];
            for (int i = 0; i < arrSize; i++)
            {
                arr[i] = i;
            }

            List<Song> songs = new List<Song>();
            string path = "SEW3 05 Playlist.csv";
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    int songNumber = 0;
                    string line;
                    bool firstline = true;
                    while ((line = sr.ReadLine()) != null && songNumber++ < arrSize)
                    {
                        if (firstline)
                        {
                            songNumber--;
                            firstline = false;
                            continue;
                        }
                        string[] values = line.Split(";");
                        string title = values[0];
                        string artist = values[1];
                        string album = values[2];
                        int songSeconds = Convert.ToInt32(values[3].Replace("s", ""));
                        songs.Add(new Song(title, artist, album, songSeconds));
                    }
                }
            }
            else
            {
                Console.WriteLine("No CSV found! -> EXIT!");
                return;
            }

            ulong counter = 0;
            List<List<int>> finalList = new List<List<int>>();
            recursiveTest(arr, new List<int>(), 0, arrSize, ref counter, finalList, songs, searchedDuration);

            Console.WriteLine("Anzahl Kombinationen: " + counter);
            Console.WriteLine("Searched Duration: " + searchedDuration);

            foreach (List<int> bs in finalList)
            {
                Console.Write(checkPLLength(bs, songs)+ "s  ");
                Console.Write("[ ");
                foreach (int x in bs)
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine("]");
            }

            //Console.WriteLine(finalList);

            void recursiveTest(int[] arr, List<int> testList, int n, int max, ref ulong counter, List<List<int>> finalList, List<Song> songs, int searchedDuration)
            {
                if (n >= max)
                {
                    return;
                }
                counter++;
                testList.Add(arr[n]);
                /*
                Console.Write("[ ");
                foreach (int x in testList)
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine("]");*/
                // Testen der Kombination
                testCombination(testList, finalList, songs, searchedDuration);

                recursiveTest(arr, testList, n + 1, max, ref counter, finalList, songs, searchedDuration);

                testList.Remove(arr[n]);

                recursiveTest(arr, testList, n + 1, max, ref counter, finalList, songs, searchedDuration);
            }

            void testCombination(List<int> testList, List<List<int>> finalList, List<Song> songs, int searchedDuration)
            {
                if (finalList.Count < 10)
                {
                    finalList.Add(new List<int>(testList));
                }
                else
                {
                    int testSongsDuration = checkPLLength(testList, songs);
                    if (testSongsDuration > searchedDuration)
                    {
                        return;
                    }
                    int diffTestSongs = searchedDuration - testSongsDuration;
                    foreach (List<int> actualSongOfFinalList in finalList)
                    {
                        int diffActualSongs = searchedDuration - checkPLLength(actualSongOfFinalList, songs);
                        if (diffTestSongs < diffActualSongs)
                        {
                            finalList.Insert(finalList.IndexOf(actualSongOfFinalList), new List<int>(testList));
                            finalList.RemoveAt(finalList.Count - 1);
                            break;
                        }
                    }
                }
            }

            // Bestimme die Playlist-Länge der Kombination
            int checkPLLength(List<int> testList, List<Song> songs)
            {
                int totalSeconds = 0;

                foreach(int i in testList)
                {
                    totalSeconds += songs[i].Duration;
                }

                return totalSeconds;
            }
        }
    }
    public class Song
    {
        public string Title { get; }
        public string Artist { get; }
        public string Album { get; }
        public int Duration { get; }
        public Song(string title, string artist, string album, int duration)
        {
            Title = title;
            Artist = artist;
            Album = album;
            Duration = duration;
        }
    }
}