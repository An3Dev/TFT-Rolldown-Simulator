using System.Collections.Generic;

public enum Trait
{
    Abomination,
    Assassin,
    Brawler,
    Caretaker,
    Cavalier,
    Coven,
    Cruel,
    Dawnbringer,
    Draconic,
    Dragonslayer,
    Eternal,
    Forgotten,
    God_King,
    Hellion,
    Invoker,
    Ironclad,
    Knight,
    Legionnaire,
    Mystic,
    Nightbringer,
    Ranger,
    Redeemed,
    Renewer,
    Revenant,
    Skirmisher,
    Spellweaver,
    Verdant
}

public class TraitDescriptions
{
    public static string[] descriptions = {
        "Abominations empower their creation, The Monstrosity gets stronger with every Champion death, gaining a percent of the dying champion’s maximum Health. Upon reaching peak strength, or when no allied Abomination champions remain, the Monstrosity will be unleashed. (2/3/4/5)",
        "Assassin Innate: At the start of combat Assassins leap to the enemy backline. Assassins’ abilities can critically strike and they gain bonus Critical Strike Chance and bonus Critical Strike Damage. (2/4/6)",
        "Brawlers gain additional maximum Health. (2/4)",
        "Caretakers deploy with a Baby Dragon that can be placed anywhere on the battlefield. Baby Dragons gain 100% of their handler’s Attack Speed and restore 50 mana to their Caretaker upon death. (1)",
        "Innate: Cavaliers charge quickly towards their target whenever they move. Cavaliers take reduced damage. At the start of combat and after charging, this effect is doubled for 4 seconds. (2/3/4)",
        "At the start of combat, the Champion nearest to the center of your Coven Champions is chosen as the Coven Leader, gaining 50% Bonus Ability Power. Each time a Coven unit casts their ability, a fraction of the cost is bestowed upon the Coven Leader as mana. (3)",
        "Cruel champions are purchased with Little Legend Health instead of gold. Never underestimate the hunger of a Cruel champion in one versus one combat. (1)",
        "Dawnbringers rapidly heal a percent of their maxumum Health the first time they drop below 50%. When this heal occurs, all allied Dawnbringer gain 10% bonus damage. (2/4/6/8)",
        "(3) At the end of each player combat, gain a dragon egg on your bench. Dragon eggs hatch into a Draconic Champion after 3 rounds. (5) Dragon eggs are golden! Golden eggs can hatch into rare loot! (3/5)",
        "Dragonslayers gain bonus Ability Power. After the first ally Dragonslayer scores a takedown on an enemy with at least 1400 maximum health each combat, all allies gain additional Ability Power for the remainder of combat. (2/4)",
        "Kindred deploys as Lamb and Wolf separately, each with their own abilities and gaining the effects of Kindred's items. (1)",
        "Forgotten champions have bonus Health and Attack Damage. Each Shadow item worn by a Forgotten champion increases the bonus by 10% on all Forgotten champions, stacking up to 5 times. (3/6/9)",
        "If you have exactly one God-King they deal 30% bonus damage to enemies who have at least one of their Rival Traits. Garen's Rival Traits: Forgotten, Nightbringer, Coven, Hellion, Dragonslayer, Abomination, Revenant. Darius' Rival Traits: Redeemed, Dawnbringer, Verdant, Draconic, Ironclad. (1)",
        "Hellions gain Attack Speed. Whenever a Hellion dies an imperfect Doppelhellion of the same type will leap from the Hellion portal and join the fight! (3/5/7)",
        "All allies gain increased Mana per Basic Attack. (2/4)",
        "All allies gain bonus Armor. (2/3)",
        "All allies block a flat amount of damage from all sources. (2/4/6)",
        "Legionnaires gain bonus attack speed, and their first attack after casting a spell heals them for 50% of the damage dealt. (2/4/6/8)",
        "All allies gain magic resist. (2/3/4)",
        "Nightbringers gain a decaying shield equal to a percent of their maximum Health the first time they drop below 50%. When this shield is applied, that Nightbringer gain 35% bonus damage. (2/4/6/8)",
        "All Rangers gain Attack Speed for 2 seconds every 4 seconds. (2/4)",
        "Redeemed have increased Armor, Magic Resistance, and Ability Power. When they die, they pass this bonus split among living allied Redeemed. (3/6/9)",
        "Renewers heal for a percent of their maximum Health each second. If they're full health, they restore mana instead. (2/4)",
        "Revenants revive after their first death each combat. Once revived, they take and deal 30% increased damage. (2/3)",
        "Skirmishers gain a shield at the start of combat, and gain Attack Damage each second. (3/6)",
        "Spellweavers have increased Ability Power and get bonus Ability Power any time a champion uses an ability, stacking up to 10 times. (2/4)",
        "Champions that start combat adjacent to at least one Verdant Champion are immune to crowd control for the first 3 seconds of combat. (2/3)"
    };
}

