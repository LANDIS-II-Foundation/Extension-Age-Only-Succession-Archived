# What is Age-Only Succession?

Age-Only succession behaves similar to previous LANDIS models. However, this extension has a variable temporal resolution and therefore cohort ages are not limited to 10-year time steps.  Age-only Succession v2.0 - Previous versions of LANDIS applied an assumption that species cohorts younger than their age of sexual maturity did not cast shade. This assumption was implemented as a proxy for crown closure, assuming that opportunities for species establishment existed prior to the onset of direct competition for light. Version 2.0 now includes this assumption to be most compatible with earlier versions of LANDIS.

Age-Only succession defines a species-age cohort  as a function of cohort reproduction, cohort ageing, and cohort mortality. The onset of cohort reproduction requires three prior events:  
1.  a propagule must exist, either through seeding, resprouting, or planting; 
2. there must be adequate light; and  
3. the probability of species establishment must exceed a random number. Disturbance, seed dispersal, and amount of shade all affect cohort reproduction. Extension-Age-Only-Succession uses the seed dispersal algorithm described in the white paper provided on the LANDIS-II web site (www.landis-ii.org/documentation) by Ward et al. (2005).

# Features

- [x] Does not require biomass inputs for the initial community.
- [x] Is the most widely compatible extension as it as has the simplist cohort structure: only species and age.

# Release Notes

- Latest official release: Version 5.2 — October 2019
- [Age-Only Succession User Guide](https://github.com/LANDIS-II-Foundation/Extension-Age-Only-Succession/blob/master/docs/LANDIS-II%20Age-Only%20Succession%20v5.0%20User%20Guide.pdf).
- Full release details found in the User Guide and on GitHub.

# Requirements

To use Age-Only Succession, you need:

- The [LANDIS-II model v7.0](http://www.landis-ii.org/install) installed on your computer.
- Example files (see below)

# Download

Version 5.2 can be downloaded [here](https://github.com/LANDIS-II-Foundation/Extension-Age-Only-Succession/blob/master/deploy/installer/LANDIS-II-V7%20Age%20Only%20Succession%205.2-setup.exe). To install it on your computer, just launch the installer.  To run the extension, use the batch file provided with the example.

# Citation

Mladenoff, D.J. and H.S. He. 1999. Design, behavior and applications of LANDIS, an object-oriented models of forest landscape disturbance and succession. Pages 125-162 in Spatial modeling of forest landscape change, Mladenoff, D.J. and Baker, W.L. (eds.), Cambridge University Press, Cambridge, UK.

# Compatible Extensions

Base Fire, Base Harvest, Base Wind, Base BDA, Base Drought, Dynamic Fuels and Fire

# Example Files

LANDIS-II requires a global parameter file for your scenario, and separate parameter files for each extension.

Example files are [here](https://github.com/LANDIS-II-Foundation/Extension-Age-Only-Succession/blob/master/testing/Corev7/Age-only-example.zip).

# Support

If you have a question, please contact Robert Scheller. 
You can also ask for help in the [LANDIS-II users group](http://www.landis-ii.org/users).

If you come across any issue or suspected bug when using NECN, please post about it in the [issue section of the Github repository](https://github.com/LANDIS-II-Foundation/Extension-Age-Only-Succession/issues) (GitHub ID required).

# Author

[The LANDIS-II Foundation](http://www.landis-ii.org)

Mail : rschell@ncsu.edu
