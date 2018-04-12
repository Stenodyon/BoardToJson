using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SavedObjects;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using NDesk.Options;

namespace BoardToJson
{
    class Program
    {
        private static String input_file = null;
        private static String output_file = null;
        private static bool verbose = false;
        private static bool indent = false;

        public static SavedObjectV2 ReadBoard(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            return (SavedObjectV2)formatter.Deserialize(fs);
        }

        public static void ToJson(string board_filename, string json_filename = null)
        {
            json_filename = json_filename ?? Path.GetFileNameWithoutExtension(board_filename) + ".json";
            if (verbose)
                Console.WriteLine($"Converting from tungboard to JSON ({board_filename} -> {json_filename})");
            SavedObjectV2 board = ReadBoard(board_filename);
            if (verbose)
                Console.WriteLine("Board read");
            string serialized = JsonConvert.SerializeObject(board,
                indent ? Formatting.Indented : Formatting.None,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            if (verbose)
                Console.WriteLine("Board serialized");
            File.WriteAllText(json_filename, serialized);
        }

        public static SavedObjectV2 ReadJson(string json_filename)
        {
            return JsonConvert.DeserializeObject<SavedCircuitBoard>(File.ReadAllText(json_filename), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public static void ToBoard(string json_filename, string board_filename = null)
        {
            board_filename = board_filename ?? Path.GetFileNameWithoutExtension(json_filename) + ".tungboard";
            if (verbose)
                Console.WriteLine($"Converting from JSON to tungboard ({json_filename} -> {board_filename})");
            SavedObjectV2 board = ReadJson(json_filename);
            if (verbose)
                Console.WriteLine("Board read");
            FileStream fs = new FileStream(board_filename, FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, board);
            if (verbose)
                Console.WriteLine("Board serialized");
        }

        internal static void Fail(String message)
        {
            Console.Error.WriteLine(message);
            System.Environment.Exit(-1);
        }

        static void Main(string[] args)
        {
            var option_set = new OptionSet()
            {
                {"o|output_file=", "Specify the output file", v => {
                    if(output_file != null)
                        Fail("Output file already specified");
                    output_file = v;
                } },
                {"i|indent", "Indents the Json", v => { if (v != null) indent = true; }},
                {"v", "Verbose", v => {if (v != null) verbose = true;}},
                {"<>", v => {
                    if(input_file != null)
                        Fail("Can only process 1 file");
                    input_file = v;
                }}
            };

            try
            {
                option_set.Parse(args);
            } catch(OptionException error)
            {
                Fail(error.Message);
            }

            if(input_file == null)
                Fail("You must specify an input file");

            if (input_file.EndsWith(".json"))
                ToBoard(input_file, output_file);
            else
                ToJson(input_file, output_file);
        }
    }
}
