# SocialFeedAPI

Retrieves Statisctics about Subreddits using Reddit API

# Dependencies

Microsoft .Net Framework 6.0

# Gettting Started

1. Clone the repo
2. Retreive Authorization token to access Reddit API

     Create a developer application on Reddit. Using the ClientId/Client Secret and user credentials, invoke Reddit's "https://www.reddit.com/api/v1/access_token" endpoint to get the access token.

     More information can be found at https://github.com/reddit-archive/reddit/wiki/OAuth2

3. Update Access token in following file in your local repo

     appSettings.json --> RedditAPISettings: Token

4. Run the project with editor supporting Microsoft .Net 6.0

# API Endpoints

  This project currently contains two endpoints.

  1. /reddit/{subredditname}
     This endpoint retrieves top five posts with most upvotes and top five authors with most posts for the given subreddit.

     Example: https://localhost:7051/reddit/nfl

     {
        "topPosts": [
            {
                "title": "[Highlight] NO vs MIN - Diggs game-winning TD reception",
                "upVotes": 45623
            },
            {
                "title": "[Highlight] Dolphins score on the final play to beat the Pats",
                "upVotes": 40120
            },
            {
                "title": "[NFL Update on Twitter]Tom Brady now has more Super Bowl wins than all 32 NFL franchises.",
                "upVotes": 35270
            },
            {
                "title": "[Tom Brady] These past two months I’ve realized my place is still on the field and not in the stands. That time will come. But it’s not now. I love my teammates, and I love my supportive family. They make it all possible. I’m coming back for my 23rd season in Tampa. Unfinished business LFG",
                "upVotes": 34856
            },
            {
                "title": "New England Patriots are the Super Bowl LI Champions",
                "upVotes": 34450
            }
        ],
        "topAuthors": [
            {
                "author": "[deleted]",
                "totalPosts": 78
            },
            {
                "author": "suzukigun4life",
                "totalPosts": 32
            },
            {
                "author": "_bonzibuddy",
                "totalPosts": 27
            },
            {
                "author": "MattyT7",
                "totalPosts": 22
            },
            {
                "author": "ValKilmsnipsinBatman",
                "totalPosts": 21
            }
        ]
      }

  3. /redditfeed
     This endpoints continuously retrieves information about top five posts with most upvotes and top five authors with most posts for "nfl", "nba" and "nhl" subreddits.

     Example:
     ![image](https://github.com/patelshekhar7/SocialFeedAPI/assets/145226781/d20ef5ee-3818-42fd-b4cb-ca724206d6f3)

     
# Known limitations
1. Authorization token needs to be manually updated in the settings file after the expiration. Code flow currently doesn't support automatic renewal of the authorization code.
2. Reddit's API enforces rate limits. Code currently throttles HTTP requests with very basic mechanism of allowing one request per two seconds.
   

