﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class StudentsRepository
{
    private bool isDataInitialized;
    private Dictionary<string, Course> courses;
    private Dictionary<string, Student> students;
    private RepositoryFilter filter;
    private RepositorySorter sorter;

    public StudentsRepository(RepositoryFilter repositoryFilter, RepositorySorter repositorySorter)
    {
        this.filter = repositoryFilter;
        this.sorter = repositorySorter;
    }

    public void LoadData(string fileName)
    {
        if (this.isDataInitialized)
        {
            throw new ArgumentException(ExceptionMessages.DataAlreadyInitializedException);
        }

        this.courses = new Dictionary<string, Course>();
        this.students = new Dictionary<string, Student>();

        OutputWriter.WriteMessageOnNewLine("Reading data...");
        ReadData(fileName);
    }

    public void UnloadData()
    {
        if (!this.isDataInitialized)
        {
            OutputWriter.WriteMessageOnNewLine(ExceptionMessages.DataNotInitializedExceptionMessage);
            return;
        }

        this.courses = null;
        this.students = null;

        this.isDataInitialized = false;
    }

    private void ReadData(string fileName)
    {
        string pattern = @"([A-Z][a-zA-Z#\++]*_[A-Z][a-z]{2}_\d{4})\s+([A-Za-z]+\d{2}_\d{2,4})\s([\s0-9]+)";
        Regex regex = new Regex(pattern);

        string currentPath = SessionData.currentPath + "\\" + fileName;

        if (File.Exists(currentPath))
        {
            string[] allInputLines = File.ReadAllLines(currentPath);
            for (int line = 0; line < allInputLines.Length; line++)
            {
                if (!string.IsNullOrEmpty(allInputLines[line]) && regex.IsMatch(allInputLines[line]))
                {
                    Match currentMatch = regex.Match(allInputLines[line]);
                    string courseName = currentMatch.Groups[1].Value;
                    string userName = currentMatch.Groups[2].Value;
                    string scoresStr = currentMatch.Groups[3].Value;

                    try
                    {
                        int[] scores = scoresStr.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();


                        if (scores.Any(x => x > 100 || x < 0))
                        {
                            OutputWriter.DisplayException(ExceptionMessages.InvalidScore);
                        }

                        if (scores.Length > Course.NumberOfTasksOnExam)
                        {
                            OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScores);
                            continue;
                        }

                        if (!this.students.ContainsKey(userName))
                        {
                            this.students.Add(userName, new Student(userName));
                        }

                        if (!this.courses.ContainsKey(courseName))
                        {
                            this.courses.Add(courseName, new Course(courseName));
                        }

                        Course course = this.courses[courseName];
                        Student student = this.students[userName];

                        course.EnrollStudent(student);

                        student.EnrollInCourse(course);
                        student.SetMarksInCourse(courseName, scores);

                        
                    }
                    catch (FormatException fex)
                    {
                        OutputWriter.DisplayException($"{fex.Message} at line : {line}");                        
                    }
                }
            }

            isDataInitialized = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }
        else
        {
            OutputWriter.DisplayException(ExceptionMessages.invalidPath);
            isDataInitialized = false;
        }
    }

    public void GetStudentsScoresFromCourse(string courseName, string username)
    {
        if (IsQueryForStudentPossiblе(courseName, username))
        {
            OutputWriter.PrintStudent(new KeyValuePair<string, double>(username, this.courses[courseName].StudentsByName[username].MarksByCourseName[courseName]));
        }
    }

    public void GetAllStudentsFromCourse(string courseName)
    {
        if (IsQueryForCoursePossible(courseName))
        {
            OutputWriter.WriteMessageOnNewLine($"{courseName}");
            foreach (var studentMarkEntry in this.courses[courseName].StudentsByName)
            {
                this.GetStudentsScoresFromCourse(courseName, studentMarkEntry.Key);
            }
        }
    }

    private bool IsQueryForCoursePossible(string courseName)
    {
        if (isDataInitialized)
        {
            if (this.courses.ContainsKey(courseName))
            {
                return true;
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InexistingCourseInDataBase);
            }
        }
        else
        {
            OutputWriter.DisplayException(ExceptionMessages.DataNotInitializedExceptionMessage);
        }

        return false;
    }

    private bool IsQueryForStudentPossiblе(string courseName, string studentUserName)
    {
        if (IsQueryForCoursePossible(courseName) && this.courses[courseName].StudentsByName.ContainsKey(studentUserName))
        {
            return true;
        }
        else
        {
            OutputWriter.DisplayException(ExceptionMessages.InexistingStudentInDataBase);
        }

        return false;
    }

    public void FilterAndTake(string courseName, string givenFilter, int? studentsToTake = null)
    {
        if (IsQueryForCoursePossible(courseName))
        {
            if (studentsToTake == null)
            {
                studentsToTake = this.courses[courseName].StudentsByName.Count;
            }

            Dictionary<string, double> marks = this.courses[courseName]
                .StudentsByName
                .ToDictionary(pair => pair.Key, pair => pair.Value.MarksByCourseName[courseName]);

            this.filter.FilterAndTake(marks, givenFilter, studentsToTake.Value);
        }
    }

    public void OrderAndTake(string courseName, string comparison, int? studentsToTake = null)
    {
        if (IsQueryForCoursePossible(courseName))
        {
            if (studentsToTake == null)
            {
                studentsToTake = this.courses[courseName].StudentsByName.Count;
            }

            Dictionary<string, double> marks = this.courses[courseName]
                .StudentsByName
                .ToDictionary(pair => pair.Key, pair => pair.Value.MarksByCourseName[courseName]);

            this.sorter.OrderAndTake(marks, comparison, studentsToTake.Value);
        }
    }
}

