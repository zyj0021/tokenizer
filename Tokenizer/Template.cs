﻿using System.Collections.Generic;
using System.Linq;

namespace Tokens
{
    /// <summary>
    /// Represents a template to use to extract data from
    /// free text.
    /// </summary>
    public class Template
    {
        private readonly List<Token> tokens;

        public Template()
        {
            tokens = new List<Token>();
        }

        public Template(string name, string content) : this()
        {
            Name = name;
            Content = content;
        }

        /// <summary>
        /// The template content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The name of the template
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The tokens contained within the template
        /// </summary>
        public IReadOnlyCollection<Token> Tokens => tokens.AsReadOnly();

        public TokenizerOptions Options { get; set; }

        public IEnumerable<int> GetTokenIdsUpTo(Token token)
        {
            var matchIds = new List<int>();

            // Only remove match if out-of-order token
            if (Options.OutOfOrderTokens)
            {
                if (token.Repeating == false) matchIds.Add(token.Id);
                return matchIds;
            }

            foreach (var candiate in tokens)
            {
                if (candiate == token)
                {
                    if (candiate.Repeating == false)
                    {
                        matchIds.Add(candiate.Id);
                    }
                    break;
                }

                matchIds.Add(candiate.Id);
            }

            return matchIds;
        }

        public void AddToken(Token token)
        {
            token.Id = tokens.Count + 1;

            tokens.Add(token);
        }

        public IEnumerable<Token> TokensExcluding(IList<int> tokenIds)
        {
            return tokens.Where(t => tokenIds.Contains(t.Id) == false);
        }
    }
}
