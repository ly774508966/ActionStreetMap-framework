﻿using System.Collections.Generic;
using ActionStreetMap.Core.Scene.Models;

namespace ActionStreetMap.Core.MapCss.Domain
{
    /// <summary>
    ///     Represents MapCSS. style
    /// </summary>
    public class Style
    {
        /// <summary>
        ///     True if all selectors should be applicable to given model
        /// </summary>
        public bool MatchAll { get; set; }

        /// <summary>
        ///     List of selectors.
        /// </summary>
        public IList<Selector> Selectors { get; set; }

        /// <summary>
        ///     List of declarations.
        /// </summary>
        public Dictionary<string, Declaration> Declarations { get; set; }

        /// <summary>
        ///     Creates empty Style
        /// </summary>
        public Style()
        {
            Selectors = new List<Selector>();
            Declarations = new Dictionary<string, Declaration>();
        }

        /// <summary>
        ///     Checks whether model is defined in style.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <returns>True if model is applicable.</returns>
        public bool IsApplicable(Model model)
        {
            // NOTE don't use LINQ here as it's performance critical code
            // we want to decrease heap allocations
            if (MatchAll)
            {
                // Selectors.All(s => s.IsApplicable(model));
                for (int i = 0; i < Selectors.Count; i++)
                {
                    if (!Selectors[i].IsApplicable(model))
                        return false;
                }
                return true;
            }

            // any is applicable or closed as special case
            for (int i = 0; i < Selectors.Count; i++)
            {
                var selector = Selectors[i];
                if (selector.IsClosed && !selector.IsApplicable(model))
                    return false;
                if (selector.IsApplicable(model))
                    return true;
            }

            return false;
        }
    }
}