### **Application Life Cycle, Features and Requirements**

- **Requirements:**
  - .NET 8.0 SDK installed.
  - An input file with the following format:
    - **First Line:** Grid size as two integers (e.g., `5 5`).
    - **Subsequent Lines:** Pairs of lines where:
      - **Line 1:** Initial position and direction (`X Y D` where `D` is `N`, `S`, `E`, or `W`).
      - **Line 2:** Movement instructions consisting of `L`, `R`, `F` (e.g., `LFRF`).

- **Life Cycle:**
  - **Start:** The application begins by verifying the file path argument.
  - **File Validation:** It checks if the file exists and reads all lines.
  - **Grid Setup:** The first line is parsed to set up the grid dimensions.
  - **Mower Instructions:** Subsequent lines are processed in pairs to extract initial positions and instructions.
  - **Execution:** For each mower, the application follows the instructions to calculate the final position.
  - **Error Handling:** The program validates input format, characters, and boundary conditions, throwing exceptions with clear messages if errors occur.
  - **Completion:** The final positions of the mowers are displayed, or errors are reported if any occur during execution.
 
**Features:**
- **Grid Size Processing:**
  - The first line of the input file is processed to set the grid size.

- **Mower Instructions Processing:**
  - Subsequent lines are paired (two lines at a time).
  - The first line in each pair is used to set the initial position (`X`, `Y`, `InitialDirection`).
  - The second line contains directions (`L`, `R`, `F`).

- **Direction Validation:**
  - The initial direction is validated to be one of `N`, `E`, `W`, or `S`.
  - The directions are validated to contain only `L`, `R`, or `F`.

- **Execution of Instructions:**
  - The program executes the instructions for each mower and moves it within the grid.
  - If the mower tries to move outside the grid, a descriptive message is shown and the mower keeps its current position.

- **Error Handling:**
  - Proper error handling is included for invalid formats, moving outside the grid, and more (see Error Handling).

### **Error Handling**

- **Missing File Path Argument:** Prompts user if no file path is provided and exits.
- **Nonexistent File:** Checks for file existence, shows an error if missing, and exits.
- **Invalid Grid Size Format:** Requires two integers; errors if incorrect, showing expected format and line number.
- **Incomplete/Invalid Mower Instruction Pairs:** Ensures pairs are complete; errors with line number if not.
- **Invalid Direction Characters:** Only allows `L`, `R`, `F`; errors on invalid characters with line number.
