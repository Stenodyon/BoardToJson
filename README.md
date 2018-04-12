# BoardToJson
Convers TUNG boards to Json format, and vice-versa.

## Installation

  - Download the .zip from the [releases page](https://github.com/Stenodyon/BoardToJson/releases), extract it in a folder.
  - You will have to supply from the game the `Assembly-CSharp.dll` which you can find in
  `tung_exe_folder/The Ultimate Nerd Game_Data/Managed/`. Place it in the same folder as `BoardToJson.exe`

## Usage
```
BoardToJson.exe [-i] [-v] [-output_file=FILE] INPUT_FILE

  -i                Output indented Json, for added readability
  -v                Verbose
  -output_file -o   Specify an output file, default has the same name with the other extention
```
Using this program on a `.tungboard` file will convert it to Json, using it on a `.json` file will
convert to TUNG board.
