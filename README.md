# DiscordTerminal

This project aims to create a system of remote control for your pc, using commands run through discord

# Installation

Copy the code, paste in a discord bot token, compile and run.
You can now run commands in any server the bot has access to

# Commands
Currently supported commands:
  cd - used to switch current directory
    Usage: cd <folder in current directory>
    Usage: cd ..      //returns to previous folder in hierarchy
  ls - lists folders and files in current directory
    Usage: ls
  pwd - prints current directory
    Usage: pwd
  cat - prints out contents of a file
    Usage: cat !t <name of file>  //display as plaintext
    Usage: cat !b <name of file>  //display in bytes WIP, currently displays size and first 500 bytes
  touch - creates new file and writes to it
    Usage: touch !t <name of new file> //write as plaintext
    Usage: touch !b <name of new file> //writes as bytes WIP, currently not implemented
    //after running one of these commands, the next message you send will be written to the newly created file
   
# Upcoming
  Fixed byte reading/writing
  Retrieving files
  Improvements to current commands
