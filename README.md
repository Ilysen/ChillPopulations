# ChillPopulations

This is a Caves of Qud mod that excludes certain disruptive creatures from appearing as villagers, converts, psychic thralls, and sultan cultists. You can check the [workshop page](https://steamcommunity.com/sharedfiles/filedetails/?id=2918134876) for a full list of things affected.

Chill Populations is licensed under the [GNU General Public License v3](http://www.gnu.org/licenses/agpl.html), which can be found in full in [LICENSE.md](LICENSE.md). Code prior to commit `5221c9c6b332892dca6ce93e95dc1c72594fe9e8` is licensed under the [MIT License](https://opensource.org/license/mit/) instead.

## Changelog

### 11 October, 2023
Note: 1.1 onwards is licensed under GNU General Public License v3. Versions prior to 1.0 remain licensed under the MIT License.

**This version won't be publicly released until a save-breaking update has been pushed to the stable branch, to avoid bricking existing saves.**

#### Version 1.1
* Reassessed the whole list of barred creatures. A lot of things can spawn in certain roles now, rather than the block list being completely comprehensive. The new list is as follows:
	* Cannot be guards: bone worms, seedsprout worms, turret tinkers
	* Cannot be guards or thralls: chrome idols, eyeless king crabs, fire ant queen, goatfolk heroes,  mechanimist houndmasters, snapjaw heroes
	* Cannot be guards or villagers: humors of all kids, molting basilisks, segmented mirthworms, snailmothers, worms of the earth
	* Special cases:
		* Newfathers and dromad traders remain blocked from all special roles
		* Clonelings and minelayers cannot be guards, pariahs, or villagers
		* Liquid weeps cannot be villagers
* Fungus puffers, giant clams, pulsed field magnets, and sparking baetyls removed from the block list entirely
	* In general, a lot of these things were already excluded with vanilla logic, meaning their complete inclusion in the list didn't actually do anything different from vanilla. For those that were, though, this change is mostly in the interest of increasing variety and just letting 'em have their fun. Their removals shouldn't cause any major problems
* General code cleanup and modernization pass.

### 12 September, 2023
#### Version 1.0.1
* Added some creatures that were missed in the initial release (snapjaw heroes and goatfolk heroes)

### 2 February, 2023 (version 1.0)
* Initial release.
