﻿namespace Forum.App.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class ContentViewModel
    {
        private const int lineLength = 37;

        public string[] Content { get; }

        public ContentViewModel(string content)
        {
            this.Content = this.GetLines(content);
        }

        private string[] GetLines(string content)
        {
            char[] contentChars = content.ToCharArray();

            ICollection<string> lines = new List<string>();

            for (int i = 0; i < content.Length; i += lineLength)
            {
                char[] row = contentChars.Skip(i).Take(lineLength).ToArray();
                string rowString = string.Join("", row);
                lines.Add(rowString);
            }

            return lines.ToArray();
        }
    }
}