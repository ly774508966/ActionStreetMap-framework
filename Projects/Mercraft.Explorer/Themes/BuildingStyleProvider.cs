﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mercraft.Core.Unity;
using Mercraft.Core.World.Buildings;
using Mercraft.Infrastructure.Primitives;
using Mercraft.Models.Buildings;

namespace Mercraft.Explorer.Themes
{
    /// <summary>
    ///     Provides the way to get BuildingStyle using Building
    ///     This implementation uses color as key to find style
    /// </summary>
    public class BuildingStyleProvider : IBuildingStyleProvider
    {
        private readonly Dictionary<string, List<BuildingStyle.FacadeStyle>> _facadeStyleMapping;
        private readonly Dictionary<string, List<BuildingStyle.RoofStyle>> _roofStyleMapping;

        // Cache which used to increase speed of lookup
        private readonly DoubleKeyDictionary<string, Color32, BuildingStyle.FacadeStyle> _facadeStyleCache;
        private readonly DoubleKeyDictionary<string, Color32, BuildingStyle.RoofStyle> _roofStyleCache;

        public BuildingStyleProvider(Dictionary<string, List<BuildingStyle.FacadeStyle>> facadeStyleMapping,
            Dictionary<string, List<BuildingStyle.RoofStyle>> roofStyleMapping)
        {
            _facadeStyleMapping = facadeStyleMapping;
            _roofStyleMapping = roofStyleMapping;

            _facadeStyleCache = new DoubleKeyDictionary<string, Color32, BuildingStyle.FacadeStyle>();
            _roofStyleCache = new DoubleKeyDictionary<string, Color32, BuildingStyle.RoofStyle>();
        }

        /// <summary>
        ///     Returns building style corresponding to given building 
        /// </summary>
        public BuildingStyle Get(Building building)
        {
            // NOTE we don't want to have osm specific logic in code outside osm project.
            // Tags mapping this should be done in mapcss file. So, we will throw exception 
            // here if type isn't mapped to existing style
            if (!_facadeStyleMapping.ContainsKey(building.Type))
                throw new ArgumentException(String.Format(ErrorStrings.CannotGetBuildingStyle, building.Type));

            var facadeStyle = _facadeStyleMapping[building.Type];
            var roofStyle = _roofStyleMapping[building.Type];

            if (facadeStyle.Count == 1 && roofStyle.Count == 1)
                return new BuildingStyle
                {
                    Facade = facadeStyle.First(),
                    Roof = roofStyle.First()
                };

            return FindBestMatch(building, facadeStyle, roofStyle);
        }

        private BuildingStyle FindBestMatch(Building building, List<BuildingStyle.FacadeStyle> facadeStyles,
            List<BuildingStyle.RoofStyle> roofStyles)
        {
            // NOTE So far, this matcher uses only color to find the best style
            // as default implementation assumes that we're using only RAL colored textures
            // no real brick material, facades, etc
            const int threshold = 5;

            // facade
            BuildingStyle.FacadeStyle facadeStyle;
            if (!_facadeStyleCache.ContainsKey(building.Type, building.FacadeColor))
            {
                var facadeIndex = 0;
                var currentDiff = int.MaxValue;
                for (int i = 0; i < facadeStyles.Count; i++)
                {
                    var difference = Math.Abs(building.FacadeColor.ToInt() - facadeStyles[i].Color.ToInt());
                    if (difference < currentDiff)
                    {
                        currentDiff = difference;
                        facadeIndex = i;
                        if (currentDiff <= threshold) break;
                    }
                }
                facadeStyle = facadeStyles[facadeIndex];
                _facadeStyleCache.Add(building.Type, building.FacadeColor, facadeStyle);
            }
            else
            {
                facadeStyle = _facadeStyleCache[building.Type, building.FacadeColor];
            }

            // roof
            BuildingStyle.RoofStyle roofStyle;
            if (!_roofStyleCache.ContainsKey(building.Type, building.RoofColor))
            {
                var roofIndex = 0;
                var currentDiff = int.MaxValue;
                for (int i = 0; i < roofStyles.Count; i++)
                {
                    var difference = Math.Abs(building.RoofColor.ToInt() - roofStyles[i].Color.ToInt());
                    if (difference < currentDiff)
                    {
                        currentDiff = difference;
                        roofIndex = i;
                        if (currentDiff <= threshold) break;
                    }
                }
                roofStyle = roofStyles[roofIndex];
                _roofStyleCache.Add(building.Type, building.RoofColor, roofStyle);
            }
            else
            {
                roofStyle = _roofStyleCache[building.Type, building.RoofColor];
            }

            return new BuildingStyle
            {
                Facade = facadeStyle,
                Roof = roofStyle
            };
        }
    }
}
