using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeterHan.PLib.Buildings;
using TUNING;
using UnityEngine;

namespace ONIMod.IceBox
{
    public sealed class IceBoxConfig : IBuildingConfig
    {
        public const string ID = "IceBox";

        internal static PBuilding IceBoxTemplate;

        internal static PBuilding CreateBuilding()
        {
            return IceBoxTemplate = new PBuilding(ID, IceBoxStrings.BUILDING.PREFABS.ICEBOX.NAME)
            {
                AddAfter = IceBoxConfig.ID,
                Animation = "icebox_kanim",
                AudioCategory = "Metal",
                Category = "Food",
                ConstructionTime = 120f,
                Description = IceBoxStrings.BUILDING.PREFABS.ICEBOX.DESC,
                EffectText = IceBoxStrings.BUILDING.PREFABS.ICEBOX.EFFECT,
                Entombs = true,
                ExhaustHeatGeneration = 0,
                Floods = true,
                DefaultPriority = 6,
                HeatGeneration = 0f,
                Height = 3,
                Width = 3,
                HP = 100,
                IndustrialMachine = false,
                OverheatTemperature = null,
                Placement = BuildLocationRule.OnFloor,
                RotateMode = PermittedRotations.FlipH,
                SubCategory = "Food",
                Tech = "FineDining",
                ViewMode = OverlayModes.Power.ID,
                LogicIO =
                {
                    LogicPorts.Port.OutputPort(FilteredStorage.FULL_PORT_ID,new CellOffset(0,1),IceBoxStrings.BUILDING.PREFABS.ICEBOX.LOGIC_PORT,IceBoxStrings.BUILDING.PREFABS.ICEBOX.LOGIC_PORT_ACTIVE,IceBoxStrings.BUILDING.PREFABS.ICEBOX.LOGIC_PORT_INACTIVE,false,false),
                },
                Ingredients = {
                    new BuildIngredient(MATERIALS.RAW_MINERALS, tier: 4),
                },
                PowerInput = new PowerRequirement(Settings.GetSettings().DefultPower, new CellOffset(0,0)),
            };
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            base.ConfigureBuildingTemplate(go, prefab_tag);
            IceBoxTemplate?.ConfigureBuildingTemplate(go);
            go.GetComponent<KPrefabID>();
        }

        public override void DoPostConfigureUnderConstruction(GameObject go)
        {
            IceBoxTemplate?.CreateLogicPorts(go);
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
            IceBoxTemplate?.CreateLogicPorts(go);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            IceBoxTemplate?.DoPostConfigureComplete(go);
            IceBoxTemplate?.CreateLogicPorts(go);
            Storage storage = go.AddOrGet<Storage>();
            storage.showInUI = true;
            storage.showDescriptor = true;
            storage.storageFilters = STORAGEFILTERS.FOOD;
            storage.allowItemRemoval = true;
            storage.capacityKg = Settings.GetSettings().Defultstorage;
            storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
            storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
            storage.showCapacityStatusItem = true;
            Prioritizable.AddRef(go);
            go.AddOrGet<TreeFilterable>();
            go.AddOrGet<FoodStorage>();
            go.AddOrGet<Refrigerator>();
            RefrigeratorController.Def def = go.AddOrGetDef<RefrigeratorController.Def>();
            def.powerSaverEnergyUsage = 20f;
            def.coolingHeatKW = 0f;
            def.steadyHeatKW = 0f;
            go.AddOrGet<UserNameable>();
            go.AddOrGet<DropAllWorkable>();
            go.AddOrGetDef<RocketUsageRestriction.Def>().restrictOperational = false;
            go.AddOrGetDef<StorageController.Def>();
        }

        public override BuildingDef CreateBuildingDef()
        {
            if (IceBoxTemplate == null)
                throw new ArgumentNullException(nameof(IceBoxTemplate));
            var def = IceBoxTemplate.CreateDef();
            return def;
        }
    }
}
