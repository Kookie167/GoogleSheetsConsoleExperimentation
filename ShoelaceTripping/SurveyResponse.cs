using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoelaceTripping
{
    public class SurveyResponse
    {
        public string DiscordHandle { get; private set; }
        public string PreferredName { get; private set; }
        public string Email { get; private set; }
        public string Country { get; private set; }
        public string State { get; private set; }
        public List<string> Events { get; private set; }
        public string FavouriteColour { get; private set; }

        public SurveyResponse(string handle, string name, string email, string country, string state, List<string> events, string colour)
        {
            DiscordHandle = handle;
            PreferredName = name;
            Email = email;
            Country = country;
            State = state;
            Events = events;
            FavouriteColour = colour;
        }
    }
}
