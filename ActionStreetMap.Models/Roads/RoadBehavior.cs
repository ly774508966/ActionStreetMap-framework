﻿using ActionStreetMap.Core.Scene.World.Roads;
using UnityEngine;

namespace ActionStreetMap.Models.Roads
{
    /// <summary>
    ///     Represents road behavior which can be useful to tweak road after game starts.
    /// </summary>
    public class RoadBehavior: MonoBehaviour
    {
        /// <summary>
        ///     Gets or sets Road.
        /// </summary>
        public Road Road { get; set; }
    }
}
