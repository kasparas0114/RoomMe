# RoomMe
RoomMe is a mobile application that allows for users to seek out potential roommates in a mutual location. Users can view a feed of other users either looking for people to fill vacancy in their own property, or looking for people who they want to find housing with. Either way, a user can view a complete profile of other users on the feed and get contact information, as well as save potential roommates to a favorited list of users, the RoomMe List. One's own profile is editable and viewable accordingly. Users can also view a match score based on their compatibility to a specific user. A complex algorithm computes the score based on several factors of the users preferences. 

To get started, a user must login with their Facebook credentials and fill out a short preferences form to be registered. Go forth, and find your RoomMe!

Check out our issue board [here!](https://waffle.io/zplata/RoomMe)

Issue Counts:

[![Stories in Ready](https://badge.waffle.io/zplata/RoomMe.png?label=ready&title=Ready)](https://waffle.io/zplata/RoomMe)

[![Stories in In Progress](https://badge.waffle.io/zplata/RoomMe.png?label=in%20progress&title=In%20Progress)](https://waffle.io/zplata/RoomMe)

[![Stories in Code Review](https://badge.waffle.io/zplata/RoomMe.png?label=code%20review&title=Code%20Review)](https://waffle.io/zplata/RoomMe)

## Technical Details

For hackfest, we've tossed together an android application that consumes API endpoints exposed by a webservice located at http://roomme.azurewebsites.net (yes, we're live!). The android code features mostly UI and API-interfacing code. The "guts" of the service is actually located within the webservice. The fully fledged webservice was constructed using .NET's WebAPI 2.0 and Entity Framework 6. By taking advantage of their controller scaffolding feature (which is why one of first commits contains about half a million lines of code), we were able to quickly erect CRUD API endpoints with an autogenerated database once we had solidified the data model and a layer of corresponding POCO models. Of course, while we developed the application, the data model changed quite frequently. We used code-first migrations to update the database schema on both prod and local environments with the latest model/schema changes. Our webservice also includes a layer of DTO models to avoid relational-data headaches on the mobile application and to reduce data transfer latency. 

Because all business logic is abstracted into the webservice, users should be able to create their own apps on any platform given that they can make HTTP GET/POST/PUT calls and parse JSON. 

## Data Model

### Models
Our application uses the following models:

* User - encapsulates data about the user including name, gender, contact info, employment data, current housing, favorited users, etc.
* Preferences - encapsulates data about the user's preferences including gender, age, price range, tags, locations, and housings.
* Tag - users can characterize themselves with a collection of tags that represent their interests.
* Housing - represents a residential building. Contains the building's name, address, and geographic location.
* Location - represents a location on earth with latitude and longitude.
* Career - contains data about a user's job. Contains the name of the company and the job title.

### Relations

#### User

* User may have a Career object
  * If the user has current employment, it will be represented using the career object
* User may have a Housing object
  * If the user currently has housing, it will be represented using the housing object
* User may have a list of Tags
  * The user may describe itself with a list of tags.
* User may have a Preferences object
  * The user has a set of preferences. It will only be null if the user has not submitted a set of preferences.

#### Preferences

* Preferences may have a list of tags
  * Tags included in this list represent what tags the user would like to see in potential roommates. They are not necessarily equivalent to the user's own tags. For example, I may not tag myself with Music, but I may include the Music tag in my preferences because I want a roommate who can serenade me every night. 
* Preferences may have a list of locations
  * Locations included in this list denote where the user prefers to live in terms of geographic coordinates. This can be used to denote preferences in general locations rather than by specific buildings.
* Preferences may have a list of Housings
  * Housing object included in this list denote where the user prefers to live in terms of specific buildings. 

#### Tag

* Tags have Users
  * This many-to-many relationship allows the application to retrieve a list of users who have identified themselves with a particular tag. This may come in handy when performing complex queries.

#### Housing

* List of Users
  * The Residents property of a Housing object is a list of users who live in the building.
* Location
  * Denotes the geographic location of the building

#### Career

* List of Users
  * Denotes the users who have this career in common.

## API

All of our api exists on `roomme.azurewebsites.net/api`. Check out the url for (incomplete) documentation on all the available enpoints. 

To return models from the DB, you can either hit `roomme.azurewebsites.net/api/{model}` OR `roomme.azurewebsites.net/api/min{model}`. The former returns a fully fleshed out object with all of its relational data included. As a general rule, try to use the `api/min{model}` endpoints instead, as they return stripped down versions of the models (no relational data). When you want to complete rehydrate the model from its DTO form, query for those properties again using the `api/min{model}/byuserid` endpoints. Finally, to associate objects with each other, checkout the `api/{model}/associate{object}` endpoints. 

Moral of the story is, we've exposed just enough API endpoints so that you can build your own app around our service!

## Algorithm

Our super roommate algorithm takes into consideration several factors before spitting out a compatibility score:
* Status
  * scores are computed slightly differently depending on if you're moving in with another roommate, if you're filling a vacancy with another roommate, or just looking at other roommates who don't have residencies yet.
* your age, their age, your preferred age
* your gender, their gender, your preferred gender
* your preferred price range vs their preferred price range, vs their price
* location preferrences and location proximity
* housing preferrences and location proximity
  
The range of scores you can get starts at 0 but has no upper limit. As a rule of thumb, consider a score of 50 to be a match made in heaven and 0 to be absolutely incompatible.
