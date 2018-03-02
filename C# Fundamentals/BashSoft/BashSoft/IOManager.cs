﻿
using System.IO;
using System.Collections.Generic;
using System;

public static class IOManager
{
    public static void TraverseDirectory(int depth)
    {
        OutputWriter.WriteEmptyLine();
        int initialIdentation = SessionData.currentPath.Split('\\').Length;
        Queue<string> subFolder = new Queue<string>();
        subFolder.Enqueue(SessionData.currentPath);

        while (subFolder.Count != 0)
        {
            var currentPath = subFolder.Dequeue();
            int indentation = currentPath.Split('\\').Length - initialIdentation;
            OutputWriter.WriteMessageOnNewLine(string.Format($"{new string('-', indentation)}{currentPath}"));

            try
            {
                foreach (var file in Directory.GetFiles(currentPath))
                {
                    int indexOfLastSlash = file.LastIndexOf('\\');
                    string fileName = file.Substring(indexOfLastSlash);
                    OutputWriter.WriteMessageOnNewLine($"{new string('-', indexOfLastSlash)}{fileName}");
                }

                foreach (string directoryPath in Directory.GetDirectories(currentPath))
                {
                    subFolder.Enqueue(directoryPath);
                }
            }
            catch (UnauthorizedAccessException)
            {
                OutputWriter.DisplayException(ExceptionMessages.UnauthorizedAccessException);
            }

            if (depth - indentation < 0)
            {
                break;
            }
        }
    }

    public static void ChangeCurrentDirectoryRelative(string relativePath)
    {
        if (relativePath == "..")
        {
            try
            {
                string currentPath = SessionData.currentPath;
                int indexOfLastSlash = currentPath.LastIndexOf('\\');
                string newPath = currentPath.Substring(0, indexOfLastSlash);
                SessionData.currentPath = newPath;
            }
            catch (ArgumentOutOfRangeException)
            {
                OutputWriter.DisplayException(ExceptionMessages.UnableToGoHigherInPartitionHierarchy);
            }
        }
        else
        {
            string currentPath = SessionData.currentPath;
            currentPath += $"\\{relativePath}";
            ChangeCurrentDirectoryAbsolute(currentPath);
        }
    }

    public static void ChangeCurrentDirectoryAbsolute(string currentPath)
    {
        if (!Directory.Exists(currentPath))
        {
            OutputWriter.DisplayException(ExceptionMessages.invalidPath);
            return;
        }

        SessionData.currentPath = currentPath;
    }

    public static void CreateDirectoryInCurrentFolder(string name)
    {
        string path = $"{SessionData.currentPath}\\{name}";

        try
        {
            Directory.CreateDirectory(path);
        }
        catch (ArgumentException)
        {
            OutputWriter.WriteMessageOnNewLine(ExceptionMessages.ForbiddenSymbolsContainedInName);
        }
    }

}