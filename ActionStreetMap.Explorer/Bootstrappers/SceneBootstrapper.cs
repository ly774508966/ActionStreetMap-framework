﻿using ActionStreetMap.Core.MapCss;
using ActionStreetMap.Core.Scene;
using ActionStreetMap.Explorer.Scene;
using ActionStreetMap.Explorer.Scene.Builders;
using ActionStreetMap.Explorer.Themes;
using ActionStreetMap.Infrastructure.Bootstrap;
using ActionStreetMap.Infrastructure.Config;
using ActionStreetMap.Infrastructure.Dependencies;
using ActionStreetMap.Models.Buildings;
using ActionStreetMap.Models.Buildings.Facades;
using ActionStreetMap.Models.Buildings.Roofs;
using ActionStreetMap.Models.Roads;
using ActionStreetMap.Models.Terrain;
using ActionStreetMap.Models.Utils;

namespace ActionStreetMap.Explorer.Bootstrappers
{
    /// <summary>
    ///     Register scene specific classes.
    /// </summary>
    public class SceneBootstrapper: BootstrapperPlugin
    {
        private const string ThemeKey = "theme";

        /// <inheritdoc />
        public override string Name { get { return "scene"; } }

        /// <inheritdoc />
        public override bool Run()
        {
            Container.Register(Component.For<IResourceProvider>().Use<UnityResourceProvider>().Singleton());

            Container.Register(Component.For<IModelVisitor>().Use<TileModelLoader>().Singleton());

            var themeConfigPath = GlobalConfigSection.GetString(ThemeKey);
            var themeConfig = new ConfigSection(themeConfigPath, FileSystemService);
            // register theme provider
            Container.Register(Component
                .For<IThemeProvider>()
                .Use<ThemeProvider>()
                .Singleton()
                .SetConfig(themeConfig));

            // register stylesheet provider
            Container.Register(Component
                .For<IStylesheetProvider>()
                .Use<StylesheetProvider>()
                .Singleton()
                .SetConfig(GlobalConfigSection));

            // register model builders
            Container.Register(Component.For<IModelBuilder>().Use<BuildingModelBuilder>().Named("building").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<SphereModelBuilder>().Named("sphere").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<CylinderModelBuilder>().Named("cylinder").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<WaterModelBuilder>().Named("water").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<DetailModelBuilder>().Named("detail").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<BarrierModelBuilder>().Named("barrier").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<InfoModelBuilder>().Named("info").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<TreeModelBuilder>().Named("tree").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<RoadModelBuilder>().Named("road").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<SplatModelBuilder>().Named("splat").Singleton());
            
            // register core behaviours
            // NOTE no standard behaviours so far

            // buildings
            Container.Register(Component.For<IBuildingStyleProvider>().Use<BuildingStyleProvider>().Singleton());
            // facades
            Container.Register(Component.For<IBuildingBuilder>().Use<BuildingBuilder>().Singleton());
            Container.Register(Component.For<IFacadeBuilder>().Use<FlatFacadeBuilder>().Named("flat").Singleton());
            // roofs

            Container.Register(Component.For<IRoofBuilder>().Use<GabledRoofBuilder>().Named("gabled").Singleton());
            Container.Register(Component.For<IRoofBuilder>().Use<HippedRoofBuilder>().Named("hipped").Singleton());
            Container.Register(Component.For<IRoofBuilder>().Use<DomeRoofBuilder>().Named("dome").Singleton());
            Container.Register(Component.For<IRoofBuilder>().Use<PyramidalRoofBuilder>().Named("pyramidal").Singleton());
            Container.Register(Component.For<IRoofBuilder>().Use<MansardRoofBuilder>().Named("mansard").Singleton());
            Container.Register(Component.For<IRoofBuilder>().Use<FlatRoofBuilder>().Named("flat").Singleton());

            // terrain
            Container.Register(Component.For<ITerrainBuilder>().Use<TerrainBuilder>().Singleton());

            // relation specific processing
            Container.Register(Component.For<RelationBuilder>().Use<RelationBuilder>().Singleton());
            
            // roads
            Container.Register(Component.For<IRoadStyleProvider>().Use<RoadStyleProvider>().Singleton());
            Container.Register(Component.For<IRoadBuilder>().Use<RoadBuilder>().Singleton());

            return true;
        }
    }
}
