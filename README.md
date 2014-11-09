StockDragFix
============
TLDR: Fixes aerodynamics without breaking compatibility
As the name says, this fixes a problem with KSP's aerodynamic model. To recap, KSP uses mass to approximate cross sectional 
area. While this may not be the correct way to implement drag, the premise of this mod is that KSP's drag system is an 
abstraction of a complex problem and while not accurate is acceptable enough from a game design point of view.

The problem is that KSP's drag model uses resource mass as well, producing incorrect behavior.

This plugin neutralizes resource mass by continually adjusting the part's drag coefficients while continuing to allow stock
aerodynamics to function rather than replacing it. Because of this, other stock game functions continue to interact 
appropriately. For instance, SAS works properly. MechJeb will also mostly function properly with this plugin in effect.
Its handling of rockets and spaceplanes will not be impaired. It may have some problems with reentry accuracy but
not to the extent that it does with FAR because underlying game systems are intact.
