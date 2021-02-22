using HtecDakarRallyWebApi.Enumerations;
using System.Collections.Generic;
using System;
using HtecDakarRallyWebApi.Extensions;
using System.Linq;

namespace HtecDakarRallyWebApi.Models
{
    public static class Generator
    {
        public static Vehicle Vehicle(Race race)
        {
            return new Vehicle()
            {
                RaceId = race.Id,
                Team = string.Join(' ', new string[] { adverb.Some<string>(), adjective.Some<string>(), plural.Some<string>(), }),
                Model = string.Join(' ', new string[] { manufacturer.Some<string>(), model.Some<string>(), }),
                Manufactured = DrConstants.Random.Next(DrConstants.OldestVehicle, race.Year + 1),
                Class = (Enum.GetValues<VehicleClassEnum>()).Where(c => c != VehicleClassEnum.None).ToList().Some<VehicleClassEnum>(),
            };
        }
        private static List<string> _adverb;
        private static List<string> adverb
        {
            get
            {
                if (_adverb == null)
                {
                    _adverb = new List<string> {
                        "Almost",
                        "Bravely",
                        "Commonly",
                        "Daily",
                        "Easily",
                        "Eventually",
                        "Fast",
                        "Fatally",
                        "Furiously",
                        "Gently",
                        "Instantly",
                        "Kindly",
                        "Less",
                        "Likely",
                        "Madly",
                        "Mechanically",
                        "Mostly",
                        "Naturally",
                        "Never",
                        "Nicely",
                        "Not",
                        "Oddly",
                        "Often",
                        "Only",
                        "Partially",
                        "Quickly",
                        "Randomly",
                        "Rapidly",
                        "Rarely",
                        "Repeatedly",
                        "Seldom",
                        "Seriously",
                        "Slowly",
                        "Sometimes",
                        "Soon",
                        "Strictly",
                        "Suddenly",
                        "Too",
                        "Ultimately",
                        "Usually",
                        "Voluntarily",
                        "Well",
                    };
                }
                return _adverb;
            }
        }
        private static List<string> _adjective;
        private static List<string> adjective
        {
            get
            {
                if (_adjective == null)
                {
                    _adjective = new List<string> {
                        "Adorable",
                        "Attractive",
                        "Bloody",
                        "Brave",
                        "Breakable",
                        "Charming",
                        "Cruel",
                        "Curious",
                        "Cute",
                        "Dangerous",
                        "Determined",
                        "Distinct",
                        "Dizzy",
                        "Elegant",
                        "Excited",
                        "Fancy",
                        "Foolish",
                        "Funny",
                        "Gentle",
                        "Grotesque",
                        "Handsome",
                        "Homeless",
                        "Impossible",
                        "Itchy",
                        "Jealous",
                        "Jolly",
                        "Lazy",
                        "Magnificent",
                        "Misty",
                        "Motionless",
                        "Nasty",
                        "Odd",
                        "Precious",
                        "Proud",
                        "Strange",
                        "Talented",
                        "Terrible",
                        "Ugly",
                        "Vast",
                        "Victorious",
                        "Wandering",
                        "Wild",
                    };
                }
                return _adjective;
            }
        }
        private static List<string> _plural;
        private static List<string> plural
        {
            get
            {
                if (_plural == null)
                {
                    _plural = new List<string> {
                        "Boats",
                        "Houses",
                        "Cats",
                        "Rivers",
                        "Buses",
                        "Wishes",
                        "Pitches",
                        "Boxes",
                        "Pennies",
                        "Spies",
                        "Babies",
                        "Cities",
                        "Daisies",
                        "Women",
                        "Men",
                        "Children",
                        "Teeth",
                        "Feet",
                        "People",
                        "Leaves",
                        "Mice",
                        "Geese",
                        "Halves",
                        "Knives",
                        "Wives",
                        "Lives",
                        "Elves",
                        "Loaves",
                        "Potatoes",
                        "Tomatoes",
                        "Cacti",
                        "Foci",
                        "Fungi",
                        "Nuclei",
                        "Syllabi",
                        "Analyses",
                        "Diagnoses",
                        "Oases",
                        "Theses",
                        "Crises",
                        "Phenomena",
                        "Criteria",
                    };
                }
                return _plural;
            }
        }
        private static List<string> _manufacturer;
        private static List<string> manufacturer
        {
            get
            {
                if (_manufacturer == null)
                {
                    _manufacturer = new List<string> {
                        "Opel",
                        "Toyota",
                        "Ford",
                        "Jaguar",
                        "Volvo",
                        "Volkswagen",
                        "Daimler",
                        "Chrysler",
                        "Peugeot",
                        "Citroën",
                        "Honda",
                        "Nissan",
                        "Hyundai",
                        "Kia",
                        "Renault",
                        "Dacia",
                        "Fiat",
                        "Iveco",
                        "Suzuki",
                        "Mitsubishi",
                        "Mazda",
                        "BMW",
                        "Daewoo",
                        "Subaru",
                        "Isuzu",
                        "Rover",
                        "Porsche",
                        "MAN",
                        "Scania",
                        "Chevrolet",
                        "Mercedes",
                        "Pontiac",
                        "Oldsmobile",
                        "Lamborghini",
                        "Ferrari",
                        "Dodge",
                        "Buick",
                        "AlfaRomeo",
                        "Lancia",
                        "AstonMartin",
                        "McLaren",
                        "Jeep",
                    };
                }
                return _manufacturer;
            }
        }
        private static List<string> _model;
        private static List<string> model
        {
            get
            {
                if (_model == null)
                {
                    _model = new List<string> {
                        "Cruiser",
                        "Valkyrie",
                        "Beetle",
                        "Gremlin",
                        "Gladiator",
                        "Stealth",
                        "Typhoon",
                        "Raptor",
                        "Superbird",
                        "Spitfire",
                        "Legend",
                        "Vanquish",
                        "Demon",
                        "Stratos",
                        "Superfast",
                        "Mustang",
                        "Roadmaster",
                        "Corvette",
                        "Mangusta",
                        "Magnum",
                        "Rampage",
                        "Viper",
                        "Talon",
                        "Testarossa",
                        "Hornet",
                        "Interceptor",
                        "Countach",
                        "Diablo",
                        "Defender",
                        "Raider",
                        "Toronado",
                        "Esperante",
                        "Firebird",
                        "Carrera",
                        "Dictator",
                        "Phantom",
                        "Cobra",
                        "Hammer",
                        "Vector",
                        "Venom",
                        "Stingray",
                        "Lightning",
                    };
                }
                return _model;
            }
        }
    }
}