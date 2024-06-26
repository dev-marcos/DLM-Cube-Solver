using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace Kociemba
{
    public class Tools
    {
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Check if the cube string s represents a solvable cube.
        // 0: Cube is solvable
        // -1: There is not exactly one facelet of each colour
        // -2: Not all 12 edges exist exactly once
        // -3: Flip error: One edge has to be flipped
        // -4: Not all corners exist exactly once
        // -5: Twist error: One corner has to be twisted
        // -6: Parity error: Two corners or two edges have to be exchanged
        // 
        /// <summary>
        /// Checks if the cube definition string represents a solvable cube.
        /// </summary>
        /// <param name="s">The cube definition string (see <see cref="Facelet"/> for the format).</param>
        /// <returns>
        /// <br>0: Cube is solvable</br>
        /// <br>-1: There is not exactly one facelet of each color</br>
        /// <br>-2: Not all 12 edges exist exactly once</br>
        /// <br>-3: Flip error: One edge has to be flipped</br>
        /// <br>-4: Not all 8 corners exist exactly once</br>
        /// <br>-5: Twist error: One corner has to be twisted</br>
        /// <br>-6: Parity error: Two corners or two edges have to be exchanged</br>
        /// </returns>
        public static int verify(string s)
        {
            int[] count = new int[6];
            try
            {
                for (int i = 0; i < 54; i++)
                {
                    count[(int)CubeColor.Parse(typeof(CubeColor), i.ToString())]++;
                }
            }
            catch (Exception)
            {
                return -1;
            }

            for (int i = 0; i < 6; i++)
            {
                if (count[i] != 9)
                {
                    return -1;
                }
            }

            FaceCube fc = new FaceCube(s);
            CubieCube cc = fc.toCubieCube();

            return cc.verify();
        }

        /// <summary>
        /// Generates a random cube. </summary>
        /// <returns> A random cube in the string representation. Each cube of the cube space has the same probability. </returns>
        public static string randomCube()
        {
            CubieCube cc = new CubieCube();
            Random gen = new Random();
            cc.setFlip((short)gen.Next(CoordCube.N_FLIP));
            cc.setTwist((short)gen.Next(CoordCube.N_TWIST));
            do
            {
                cc.setURFtoDLB(gen.Next(CoordCube.N_URFtoDLB));
                cc.setURtoBR(gen.Next(CoordCube.N_URtoBR));
            } while ((cc.edgeParity() ^ cc.cornerParity()) != 0);
            FaceCube fc = cc.toFaceCube();
            return fc.to_fc_String();
        }


        // https://stackoverflow.com/questions/7742519/c-sharp-export-write-multidimension-array-to-file-csv-or-whatever
        // Kristian Fenn: https://stackoverflow.com/users/989539/kristian-fenn

        /*
        // All this code got obsolete. I had to change it to use BinaryWriter and BinaryReader - DougTag
        public static void SerializeTable(string filename, short[,] array)
        {
            EnsureFolder("Assets\\Kociemba\\Tables\\");
            BinaryFormatter bf = new BinaryFormatter();
            Stream s = File.Open("Assets\\Kociemba\\Tables\\" + filename, FileMode.Create);
            bf.Serialize(s, array);
            s.Close();
        }
        
        public static short[,] DeserializeTable(string filename)
        {
            EnsureFolder("Assets\\Kociemba\\Tables\\");
            Stream s = File.Open("Assets\\Kociemba\\Tables\\" + filename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            short[,] array = (short[,])bf.Deserialize(s);
            s.Close();
            return array;
        }
        

        public static void SerializeSbyteArray(string filename, sbyte[] array)
        {
            EnsureFolder("Assets\\Kociemba\\Tables\\");
            BinaryFormatter bf = new BinaryFormatter();
            Stream s = File.Open("Assets\\Kociemba\\Tables\\" + filename, FileMode.Create);
            bf.Serialize(s, array);
            s.Close();
        }
        
        public static sbyte[] DeserializeSbyteArray(string filename)
        {
            EnsureFolder("Assets\\Kociemba\\Tables\\");
            Stream s = File.Open("Assets\\Kociemba\\Tables\\" + filename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            sbyte[] array = (sbyte[])bf.Deserialize(s);
            s.Close();
            return array;
        }
        */

        // This part has been rewritten bc BinaryFormatter got obsolete. I used BinaryWriter - DougTag
        public static void SerializeTable(string filename, short[,] array)
        {
            string basePath = Path.Combine(FileSystem.Current.AppDataDirectory, "Assets", "Kociemba", "Tables");
            string filePath = Path.Combine(basePath, filename);
            EnsureFolder(filePath);

            using FileStream fs = File.OpenWrite(filePath);
            using BinaryWriter writer = new BinaryWriter(fs);

            // Write dimensions of the array
            writer.Write(array.GetLength(0)); // Number of rows
            writer.Write(array.GetLength(1)); // Number of columns

            // Write each element of the array
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    writer.Write(array[i, j]);
                }
            }
        }

        public static short[,] DeserializeTable(string filename)
        {
            string basePath = Path.Combine(FileSystem.Current.AppDataDirectory, "Assets", "Kociemba", "Tables");
            string filePath = Path.Combine(basePath, filename);
            EnsureFolder(filePath);

            using FileStream fs = File.OpenRead(filePath);
            using BinaryReader reader = new BinaryReader(fs);

            // Read dimensions of the array
            int rows = reader.ReadInt32();
            int columns = reader.ReadInt32();

            // Create new short[,] array
            short[,] array = new short[rows, columns];

            // Read each element and populate the array
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    array[i, j] = reader.ReadInt16();
                }
            }

            return array;
        }

        public static void SerializeSbyteArray(string filename, sbyte[] array)
        {
            string basePath = Path.Combine(FileSystem.Current.AppDataDirectory, "Assets", "Kociemba", "Tables");
            string filePath = Path.Combine(basePath, filename);
            EnsureFolder(filePath);

            using FileStream fs = File.OpenWrite(filePath);
            using BinaryWriter writer = new BinaryWriter(fs);

            // Write the length of the array
            writer.Write(array.Length);

            // Write each element of the sbyte array
            foreach (sbyte value in array)
            {
                writer.Write(value);
            }
        }

        public static sbyte[] DeserializeSbyteArray(string filename)
        {
            string basePath = Path.Combine(FileSystem.Current.AppDataDirectory, "Assets", "Kociemba", "Tables");
            string filePath = Path.Combine(basePath, filename);
            EnsureFolder(filePath);

            using FileStream fs = File.OpenRead(filePath);
            using BinaryReader reader = new BinaryReader(fs);

            // Read the length of the array
            int length = reader.ReadInt32();

            // Create a new sbyte array based on the length
            sbyte[] array = new sbyte[length];

            // Read each element and populate the array
            for (int i = 0; i < length; i++)
            {
                array[i] = reader.ReadSByte();
            }

            return array;
        }

        // https://stackoverflow.com/questions/3695163/filestream-and-creating-folders
        // Joe: https://stackoverflow.com/users/13087/joe

        static void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            // If path is a file name only, directory name will be an empty string
            if (directoryName.Length > 0)
            {
                // Create all directories on the path that don't already exist
                Directory.CreateDirectory(directoryName);
            }
        }

        // Intended to check if should use Search or SearchRunTime for building tables
        public static bool NeedToCreateTables()
        {
            string basePath = Path.Combine(FileSystem.Current.AppDataDirectory, "Assets", "Kociemba", "Tables");

            if (!File.Exists(Path.Combine(basePath, "twist")))
                return true;
            else if (!File.Exists(Path.Combine(basePath, "flip")))
                return true;
            else if (!File.Exists(Path.Combine(basePath, "FRtoBR")))
                return true;
            else if (!File.Exists(Path.Combine(basePath, "URFtoDLF")))
                return true;
            else if (!File.Exists(Path.Combine(basePath, "URtoDF")))
                return true;
            else if (!File.Exists(Path.Combine(basePath, "URtoUL")))
                return true;
            else if (!File.Exists(Path.Combine(basePath, "UBtoDF")))
                return true;
            else if (!File.Exists(Path.Combine(basePath, "MergeURtoULandUBtoDF")))
                return true;
            else if (!File.Exists(Path.Combine(basePath, "Slice_URFtoDLF_Parity_Prun")))
                return true;
            else if (!File.Exists(Path.Combine(basePath, "Slice_URtoDF_Parity_Prun")))
                return true;
            else if (!File.Exists(Path.Combine(basePath, "Slice_Twist_Prun")))
                return true;
            else if (!File.Exists(Path.Combine(basePath, "Slice_Flip_Prun")))
                return true;
            else
                return false;
        }

        public static string solutionToReadable(string solution)
        {
            string solution_readable = "";

            List<char> validMoves = ['u', 'r', 'f', 'd', 'l', 'b', 'U', 'R', 'F', 'D', 'L', 'B'];
            
            for (int i = 0; i<solution.Length; ++i)
            {
                char c = solution[i];
                if (!validMoves.Contains(c)) // Means that the input string is not a solution string
                    return solution;

                if (i < solution.Length - 1 && c == solution[i+1])
                {
                    solution_readable += char.ToUpper(c) + "2";
                    ++i;
                }
                else if (char.IsUpper(c))
                {
                    solution_readable += c + "\'";
                }
                else
                {
                    solution_readable += char.ToUpper(c);
                }

                if (i < solution.Length - 1)
                {
                    solution_readable += " ";
                }
            }
            
            return solution_readable;
        }
    }    
}
