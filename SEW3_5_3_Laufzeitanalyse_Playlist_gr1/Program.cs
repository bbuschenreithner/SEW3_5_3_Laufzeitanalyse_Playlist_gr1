namespace SEW3_5_3_Laufzeitanalyse_Playlist_gr1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int searchedDuration = 200;
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

            void recursiveTest(int[] arr, List<int> testList, int n, int max, ref ulong counter, List<List<int>> finalList, List<Song> songs, int searchedDuration)
            {
                if (n >= max)
                {
                    return;
                }
                counter++;
                testList.Add(arr[n]);

                Console.Write("[ ");
                foreach (int x in testList)
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine("]");
                // Testen der Kombination
                testCombination(testList, finalList, songs, searchedDuration);

                recursiveTest(arr, testList, n + 1, max, ref counter, finalList, songs, searchedDuration);

                testList.Remove(arr[n]);

                recursiveTest(arr, testList, n + 1, max, ref counter, finalList, songs, searchedDuration);
            }

            void testCombination(List<int> testList, List<List<int>> finalList, List<Song> songs, int searchedDuration)
            {
                if (finalList.Count <= 10)
                {
                    finalList.Add(new List<int>(testList));
                }
            }

            // Bestimme die Playlist-Länge der Kombination
            int checkPLLength(List<int> testList, List<Song> songs)
            {
                int totalSeconds = 0;



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