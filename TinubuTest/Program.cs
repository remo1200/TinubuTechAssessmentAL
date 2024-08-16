namespace TinubuTest
{

    class Program
    {
        //The file path needs to be pass as a coman line argument when running the program
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a file path as a command-line argument.");
                return;
            }

            string filePath = args[0];

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                GridSize gridSize = ProcessGridSize(lines[0]);
                List<MowerInstructions> mowerInstructionsList = GetAllPairs(lines);

                Console.WriteLine($"GridSize: X = {gridSize.X}, Y = {gridSize.Y}");

                foreach (var mowerInstructions in mowerInstructionsList)
                {

                    Console.WriteLine($"Mower starting At Position: {mowerInstructions.Position.X} {mowerInstructions.Position.Y} {mowerInstructions.Position.FacingDirection}");
                    Position finalPosition = CalculateFinalPosition(mowerInstructions, gridSize);
                    Console.WriteLine($"FinalPosition: {finalPosition.X} {finalPosition.Y} {finalPosition.FacingDirection}");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"The file at path '{filePath}' was not found.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An I/O error occurred while reading the file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static GridSize ProcessGridSize(string line)
        {
            string[] gridSizeParts = line.Split(' ');
            if (gridSizeParts.Length == 2 &&
                int.TryParse(gridSizeParts[0], out int gridX) &&
                int.TryParse(gridSizeParts[1], out int gridY))
            {
                return new GridSize { X = gridX, Y = gridY };
            }
            else
            {
                throw new FormatException("Invalid format for the grid size. The correct format is: 'X Y' where X and Y are integers.");
            }
        }

        static List<MowerInstructions> GetAllPairs(string[] lines)
        {
            string[] acceptedDirections = { "N", "E", "W", "S" };
            string[] acceptedInstructions = { "L", "R", "F" };

            var mowerInstructionsList = new List<MowerInstructions>();

            for (int i = 1; i < lines.Length; i += 2)
            {
                if (i + 1 >= lines.Length)
                {
                    throw new InvalidOperationException("The file has an odd number of lines, missing a pair.");
                }

                string positionLine = lines[i].Trim();
                string[] positionParts = positionLine.Split(' ');
                if (positionParts.Length == 3 &&
                    int.TryParse(positionParts[0], out int posX) &&
                    int.TryParse(positionParts[1], out int posY))
                {
                    string initialDirection = positionParts[2];

                    if (!acceptedDirections.Contains(initialDirection))
                    {
                        throw new ArgumentException($"Invalid direction '{initialDirection}' on line {i + 1}. Accepted values are: N, E, W, S.");
                    }

                    Position position = new Position
                    {
                        X = posX,
                        Y = posY,
                        FacingDirection = initialDirection
                    };

                    string instructionLine = lines[i + 1].Trim();
                    if (instructionLine.Contains(" "))
                    {
                        throw new FormatException($"Instructions line {i + 2} should not contain spaces.");
                    }

                    for (int j = 0; j < instructionLine.Length; j++)
                    {
                        string currentChar = instructionLine[j].ToString();
                        if (!acceptedInstructions.Contains(currentChar))
                        {
                            throw new ArgumentException($"Invalid character '{currentChar}' at position {j + 1} on line {i + 2}. Accepted values are: L, R, F.");
                        }
                    }

                    MowerInstructions mowerInstructions = new MowerInstructions
                    {
                        Position = position,
                        Directions = instructionLine
                    };

                    mowerInstructionsList.Add(mowerInstructions);
                }
                else
                {
                    throw new FormatException($"Invalid format for the position line at line {i + 1}. The correct format is: 'X Y InitialDirection' where X and Y are integers and InitialDirection is one of the following values: N, E, W, S.");
                }
            }

            return mowerInstructionsList;
        }

        static Position CalculateFinalPosition(MowerInstructions mowerInstructions, GridSize gridSize)
        {
            int x = mowerInstructions.Position.X;
            int y = mowerInstructions.Position.Y;
            string direction = mowerInstructions.Position.FacingDirection;

            foreach (char instruction in mowerInstructions.Directions)
            {
                switch (instruction)
                {
                    case 'L':
                        direction = TurnLeft(direction);
                        break;
                    case 'R':
                        direction = TurnRight(direction);
                        break;
                    case 'F':
                        if (!MoveForward(ref x, ref y, direction, gridSize))
                        {
                            Console.WriteLine($"Attempt to move outside the grid boundaries. The instruction was ignored, mower remains at X = {x}, Y = {y} facing {direction}.");
                        }
                        break;
                }
            }

            return new Position { X = x, Y = y, FacingDirection = direction };
        }

        static string TurnLeft(string direction)
        {
            return direction switch
            {
                "N" => "W",
                "W" => "S",
                "S" => "E",
                "E" => "N",
                _ => direction
            };
        }

        static string TurnRight(string direction)
        {
            return direction switch
            {
                "N" => "E",
                "E" => "S",
                "S" => "W",
                "W" => "N",
                _ => direction
            };
        }

        static bool MoveForward(ref int x, ref int y, string direction, GridSize gridSize)
        {
            int newX = x, newY = y;

            switch (direction)
            {
                case "N":
                    newY++;
                    break;
                case "E":
                    newX++;
                    break;
                case "S":
                    newY--;
                    break;
                case "W":
                    newX--;
                    break;
            }

            if (newX < 0 || newX > gridSize.X || newY < 0 || newY > gridSize.Y)
            {
                return false; // Indicate that the move would go outside the grid
            }

            x = newX;
            y = newY;
            return true;
        }
    }

    class GridSize
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string FacingDirection { get; set; }
    }

    class MowerInstructions
    {
        public Position Position { get; set; }
        public string Directions { get; set; }
    }
}



