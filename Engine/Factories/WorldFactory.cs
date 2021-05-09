using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        internal static World CreateWorld()
        {
            World newWorld = new World();
            newWorld.AddLocation(-2, -1, "Farmer's Field", "There are rows of crops here, with danger lurking within.", "/Engine;component/Images/Locations/startinghouse.jpg");
            newWorld.AddLocation(-1, -1, "Farmer's House", "This is the house of the local farmer, John.", "/Engine;component/Images/Locations/startinghouse.jpg");
            newWorld.AddLocation(0, -1, "Your Home", "This is your home.", "/Engine;component/Images/Locations/startinghouse.jpg");
            newWorld.AddLocation(-1, 0, "Trader's Shop", "The shop of Susan, the local merchant.", "/Engine;component/Images/Locations/startinghouse.jpg");
            newWorld.AddLocation(0, 0, "Town Square", "You see a fountain here.", "/Engine;component/Images/Locations/startinghouse.jpg");
            newWorld.AddLocation(1, 0, "Town Gate", "There is a gate here, protecting the the from giant spiders.", "/Engine;component/Images/Locations/startinghouse.jpg");
            newWorld.AddLocation(2, 0, "Spider Forest", "Each tree in this forest is home to hundreds of tiny spiders.", "/Engine;component/Images/Locations/startinghouse.jpg");
            newWorld.AddLocation(0, 1, "Herbalist's Hut", "You spot a tiny hut out here, with medical plants all around it.", "/Engine;component/Images/Locations/startinghouse.jpg");
            newWorld.LocationAt(0, 1).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(1)); //Adding quest to location by ID
            newWorld.AddLocation(0, 2, "Herbalist's Garden", "There's a lot of herbs here, with critters lurking betwixt them all.", "/Engine;component/Images/Locations/startinghouse.jpg");

            return newWorld;
        }
    }
}
