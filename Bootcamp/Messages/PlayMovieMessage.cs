using System;
using System.Collections.Generic;
using System.Text;

namespace Bootcamp.Messages
{
    public class PlayMovieMessage
    {
        public string MovieTitle { get; }
        public int UserId { get; }

        public PlayMovieMessage(string movieTitle, int userId)
        {
            MovieTitle = movieTitle;
            UserId = userId;
        }
    }
}
