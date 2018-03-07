﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvalidFileNameException.cs" company="MetodiObetsanov@SoftUni">
//   Copyright (c) MetodiObetsanov@SoftUni. All rights reserved.
// </copyright>
// <summary>
//   Defines the InvalidFileNameException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BashSoft.Exceptions
{
    using System;

    /// <summary>
    /// The invalid file name exception.
    /// </summary>
    public class InvalidFileNameException : Exception
    {
        /// <summary>
        /// Exception message.
        /// </summary>
        private const string ForbiddenSymbolsContainedInName = "The given name contains symbols that are not allowed to be used in names of files and folders.";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFileNameException"/> class.
        /// </summary>
        public InvalidFileNameException()
            : base(ForbiddenSymbolsContainedInName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFileNameException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public InvalidFileNameException(string message)
            : base(message)
        {
        }
    }
}
