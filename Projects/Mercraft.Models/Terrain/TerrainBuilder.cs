﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mercraft.Core.Unity;
using Mercraft.Models.Utils;
using UnityEngine;

namespace Mercraft.Models.Terrain
{
    public interface ITerrainBuilder
    {
        IGameObject Build(IGameObject parent, TerrainSettings settings);
    }

    /// <summary>
    ///     Creates Terrain object using given settings
    /// </summary>
    public class TerrainBuilder: ITerrainBuilder
    {
        private readonly AlphaMapGenerator _alphaMapGenerator = new AlphaMapGenerator();
        private readonly HeightMapGenerator _heightMapGenerator = new HeightMapGenerator();

        public IGameObject Build(IGameObject parent, TerrainSettings settings)
        {
            var size = new Vector3(settings.Tile.Size, settings.Tile.HeightMap.MaxElevation, settings.Tile.Size);

            /*// fill heightmap
            var heightMapElements = CreateElements(settings, settings.Elevations,
                settings.HeightMap.Resolution / size.x,
                settings.HeightMap.Resolution / size.z,
                t => t.ZIndex);
            var htmap = _heightMapGenerator.FillHeights(settings, heightMapElements);*/

            var htmap = settings.Tile.HeightMap.Data;

            // normalize
            for (int i = 0; i < settings.Tile.HeightMap.Resolution; i++)
                for (int j = 0; j < settings.Tile.HeightMap.Resolution; j++)
                    htmap[i, j] /= settings.Tile.HeightMap.MaxElevation;
            

            // create TerrainData
            var terrainData = new TerrainData();
            terrainData.heightmapResolution = settings.Tile.HeightMap.Resolution;
            terrainData.SetHeights(0, 0, htmap);
            terrainData.size = size;
            terrainData.splatPrototypes = GetSplatPrototypes(settings.TextureParams);

            // fill alphamap
            var alphaMapElements = CreateElements(settings, settings.Areas, 
                settings.AlphaMapSize / size.x,
                settings.AlphaMapSize / size.z,
                t => t.SplatIndex);
            var alphamap = _alphaMapGenerator.GetAlphaMap(settings, alphaMapElements);

            // create Terrain using terrain data
            var gameObject = UnityEngine.Terrain.CreateTerrainGameObject(terrainData);
            gameObject.transform.parent = parent.GetComponent<GameObject>().transform;
            var terrain = gameObject.GetComponent<UnityEngine.Terrain>();

            terrain.transform.position = new Vector3(settings.CornerPosition.x, settings.ZIndex, settings.CornerPosition.y);
            terrain.heightmapPixelError = settings.PixelMapError;
            terrain.basemapDistance = settings.BaseMapDist;

            //disable this for better frame rate
            terrain.castShadows = false;

            var terrainGameObject = new GameObjectWrapper("terrain", gameObject);

            terrainData.SetAlphamaps(0, 0, alphamap);

            return terrainGameObject;
        }

        protected SplatPrototype[] GetSplatPrototypes(List<List<string>> textureParams)
        {
            var splatPrototypes = new SplatPrototype[textureParams.Count];
            for (int i = 0; i < textureParams.Count; i++)
            {
                var texture = textureParams[i];
                var splatPrototype = new SplatPrototype();
                // TODO remove hardcoded path
                // NOTE use TerrainSettings and mapcss rule?
                splatPrototype.texture = Resources.Load<Texture2D>(@"Textures/Terrain/" + texture[1].Trim());
                splatPrototype.tileSize = new Vector2(int.Parse(texture[2]), int.Parse(texture[3]));

                splatPrototypes[i] = splatPrototype;
            }
            return splatPrototypes;
        }

        private TerrainElement[] CreateElements(TerrainSettings settings,
            IEnumerable<AreaSettings> areas, float widthRatio, float heightRatio, Func<TerrainElement, float> orderBy)
        {
            return areas.Select(a => new TerrainElement()
            {
                ZIndex = a.ZIndex,
                SplatIndex = a.SplatIndex,
                Points = a.Points.Select(p =>
                    ConvertWorldToTerrain(p.X, p.Elevation, p.Y, settings.CornerPosition, widthRatio, heightRatio)).ToArray()
            }).OrderBy(orderBy).ToArray();
        }

        private static Vector3 ConvertWorldToTerrain(float x, float y, float z, Vector2 terrainPosition, float widthRatio, float heightRatio)
        {
            return new Vector3
            {
                // NOTE Coords are inverted here!
                z = (x - terrainPosition.x) * widthRatio,
                x = (z - terrainPosition.y) * heightRatio,
                y = y,
            };
        } 
    }
}