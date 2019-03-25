//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Library.AgeOnlyCohorts;
using Landis.Core;
using Landis.Library.InitialCommunities;
using Landis.Library.Succession;
using System.Collections.Generic;
using Landis.SpatialModeling;
using System;

namespace Landis.Extension.Succession.AgeOnly
{
    public class PlugIn
        : Landis.Library.Succession.ExtensionBase
    {
        public static readonly string ExtensionName = "Age-only Succession";

        private IInputParameters parameters;

        public static Species.AuxParm<Ecoregions.AuxParm<double>> establishProbabilities;
        
        private static ICore modelCore;
        private ICommunity initialCommunity;


        //---------------------------------------------------------------------

        public PlugIn()
            : base(ExtensionName)
        {
        }

        //---------------------------------------------------------------------

        public override void LoadParameters(string dataFile, ICore mCore)
        {
            modelCore = mCore;
            InputParametersParser parser = new InputParametersParser();
            parameters = Landis.Data.Load<IInputParameters>(dataFile, parser);

        }

        //---------------------------------------------------------------------

        public static ICore ModelCore
        {
            get
            {
                return modelCore;
            }
        }

        //---------------------------------------------------------------------

        public override void Initialize()
        {

            Timestep = parameters.Timestep;

            SiteVars.Initialize();

            Cohort.DeathEvent += CohortDied; 

            //establishProbabilities = parameters.EstablishProbabilities;
            DynamicInputs.Initialize(parameters.DynamicInputFile, false);
            SpeciesData.ChangeDynamicParameters(0);  // Year 0

            //Reproduction.SufficientResources = SufficientLight;
            Reproduction.Establish = Establish;
            Reproduction.AddNewCohort = AddNewCohort;
            Reproduction.MaturePresent = MaturePresent;
            Reproduction.PlantingEstablish = PlantingEstablish;
            base.Initialize(modelCore, parameters.SeedAlgorithm);
            
            InitializeSites(parameters.InitialCommunities, parameters.InitialCommunitiesMap, modelCore);
        }

        //---------------------------------------------------------------------

        public void CohortDied(object         sender,
                               DeathEventArgs eventArgs)
        {
            ExtensionType disturbanceType = eventArgs.DisturbanceType;
            if (disturbanceType != null) {
                ActiveSite site = eventArgs.Site;
                Disturbed[site] = true;
                if (disturbanceType.IsMemberOf("disturbance:fire"))
                    Landis.Library.Succession.Reproduction.CheckForPostFireRegen(eventArgs.Cohort, site);
                else
                    Landis.Library.Succession.Reproduction.CheckForResprouting(eventArgs.Cohort, site);
            }

            //modelCore.Log.WriteLine("   Cohort DIED:  {0}:{1}.", eventArgs.Cohort.Species.Name, eventArgs.Cohort.Age);
        }


        //---------------------------------------------------------------------

        protected override void InitializeSite(ActiveSite site)//,ICommunity initialCommunity)
        {
            SiteVars.Cohorts[site] = new SiteCohorts(initialCommunity.Cohorts);

        }

        //---------------------------------------------------------------------

        protected override void AgeCohorts(ActiveSite site,
                                           ushort     years,
                                           int?       successionTimestep)
        {
            SpeciesData.ChangeDynamicParameters(PlugIn.ModelCore.CurrentTime);
            SiteVars.Cohorts[site].Grow(years, site, successionTimestep, modelCore);
        }

        //---------------------------------------------------------------------

        public override byte ComputeShade(ActiveSite site)
        {
            byte shade = 0;
            foreach (SpeciesCohorts speciesCohorts in SiteVars.Cohorts[site]) {
                ISpecies species = speciesCohorts.Species;
                if (species.ShadeTolerance > shade)
                    shade = species.ShadeTolerance;
            }
            return shade;
        }

        //---------------------------------------------------------------------
        /// <summary>
        /// Add a new cohort to a site.
        /// This is a Delegate method to base succession.
        /// </summary>

        public void AddNewCohort(ISpecies species, ActiveSite site)
        {
            SiteVars.Cohorts[site].AddNewCohort(species);
        }
        //---------------------------------------------------------------------

        /// <summary>
        /// Determines if a species can establish on a site.
        /// This is a Delegate method to base succession.
        /// </summary>
        public bool Establish(ISpecies species, ActiveSite site)
        {
            //double establishProbability = establishProbabilities[species][modelCore.Ecoregion[site]];
            IEcoregion ecoregion = modelCore.Ecoregion[site];
            double establishProbability = SpeciesData.EstablishProbability[species][ecoregion];

            return modelCore.GenerateUniform() < establishProbability;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Determines if there is a mature cohort at a site.  
        /// This is a Delegate method to base succession.
        /// </summary>
        public bool MaturePresent(ISpecies species, ActiveSite site)
        {
            return SiteVars.Cohorts[site].IsMaturePresent(species);
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Determines if a species can establish on a site.
        /// This is a Delegate method to base succession.
        /// </summary>
        public bool PlantingEstablish(ISpecies species, ActiveSite site)
        {
            IEcoregion ecoregion = modelCore.Ecoregion[site];
            double establishProbability = SpeciesData.EstablishProbability[species][ecoregion];

            return establishProbability > 0.0;
        }

        public override void InitializeSites(string initialCommunitiesText, string initialCommunitiesMap, ICore modelCore)
        {
            ModelCore.UI.WriteLine("   Loading initial communities from file \"{0}\" ...", initialCommunitiesText);
            Landis.Library.InitialCommunities.DatasetParser parser = new Landis.Library.InitialCommunities.DatasetParser(Timestep, ModelCore.Species);
            Landis.Library.InitialCommunities.IDataset communities = Landis.Data.Load<Landis.Library.InitialCommunities.IDataset>(initialCommunitiesText, parser);

            ModelCore.UI.WriteLine("   Reading initial communities map \"{0}\" ...", initialCommunitiesMap);
            IInputRaster<uintPixel> map;
            map = ModelCore.OpenRaster<uintPixel>(initialCommunitiesMap);
            using (map)
            {
                uintPixel pixel = map.BufferPixel;
                foreach (Site site in ModelCore.Landscape.AllSites)
                {
                    map.ReadBufferPixel();
                    uint mapCode = pixel.MapCode.Value;
                    if (!site.IsActive)
                        continue;

                    ActiveSite activeSite = (ActiveSite)site;
                    initialCommunity = communities.Find(mapCode);
                    if (initialCommunity == null)
                    {
                        throw new ApplicationException(string.Format("Unknown map code for initial community: {0}", mapCode));
                    }

                    InitializeSite(activeSite); //, community);
                }
            }
        }
    }
}
