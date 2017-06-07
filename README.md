# Smart Mirror REST API

REST API interface for the Smart Mirror Database

## Prerequisites
* [.NET Core](https://www.microsoft.com/net/core)

## Run Tests
In order to run the tests, you have to restore the dependencies of the project. This can be done using the .NET cli.
```
cd WebApi.Test
dotnet restore
dotnet test
```

## Run Embedded Server
To run the REST API locally, you need to set a MongoDB Server in the appsettings.json configuration. By default the REST Api tries to connect to localhost with `user` and `password` credentials.
```
cd WebApi
dotnet restore
dotnet run
```

# API-Documentation

REST API Base URL: [https://sm-webapi.azurewebsites.net/api](https://sm-webapi.azurewebsites.net/api)

## Postman Examples
The `postman.json` in the root directory of project can be imported into Postman to test the REST calls against the SmartMirror API. Please ensure to use the `POST login` action in the `/api/auth` folder of the collection, to create the JW-Token. The JWT will automatically written to the environment variables of postman and used to authenticate subsequent API calls.

## Authentication
The application uses JWT to authenticate requests made to the REST API. The Token has to be passed in the `Authorization`-Header with `Bearer`-Scheme.

## {Base URL}/auth

### POST /
Some API calls requires an authenticated user in order to fullfill the request. This method is used to create an JW-Token for the given username and password. The Password has to be MD5 hashed.

#### Request
Header: No Header required

Body: 
```
{
	"User": "cem@basoglu.de",
	"Password": "098f6bcd4621d373cade4e832627b4f6"
}
```

#### Response
* Status 400: Bad Request (malformed request body)
* Status 401: Unauthorized (invalid user, password)
* Status 200: 
```
{
  "user": "cem@basoglu.de",
  "name": "Cem Basoglu",
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkNlbSBCYXNvZ2x1IiwiZW1haWwiOiJjZW1AYmFzb2dsdS5kZSIsIm5iZiI6MTQ5NjgyMTMyMywiZXhwIjoxNDk2ODI0OTIzLCJpYXQiOjE0OTY4MjEzMjMsImlzcyI6IlNtYXJ0IE1pcnJvciIsImF1ZCI6IlNtYXJ0IE1pcnJvciBBdWRpZW5jZSJ9.P_oSjsA43t-nNncIeCWbe-fasoFFdy9gX0WScowt-mE"
}
```

### GET /
To verify that an given JWT is still valid, this API call can be used to get the current authenticated user.

#### Request
Header: Authorization: Bearer {token}

Body: No Body required

#### Response
* Status 401: Unauthorized (no, invalid or expired token)
* Status 200:
```
{
  "name": "Cem Basoglu",
  "email": "cem@basoglu.de",
}
```


## {Base URL}/tickets
The Ticket Collection is used to register new mirrors. Every mirror has to be registered, before it can be used with Smart Mirror REST API. To register a new mirror, you have to create a registration ticket. The Number can than be used, to link the mirror with the users account.

### GET /{id:string}
Creates a registration ticket.

#### Request
Header: No authentication required
Body: No body required

#### Response
* Status 400: Bad Request (invalid id)
* Status 200 (WebApi.DataLayer.Models.Ticket):
```
{
  "Number": "A7Z4FH",
  "MirrorId": "5f772e8c-392c-4c7f-a38b-2bee3bddb6fb"
}
```

### POST /{number:string}
Registers a new mirror and links it to the authenticated User.

#### Request
Header: Authorization: Bearer {token}
Body: No body required

#### Response
* Status 400: Bad Request (invalid number, ticket not exists, mirror already registered)
* Status 401: Unauthorized (no authentication header)
* Status 200 (WebApi.DataLayer.Models.Mirror):
```
{
  "Id": "5f772e8c-392c-4c7f-a38b-2bee3bddb6fb",
  "User": "a@smart.mirror",
  "Name": "My Mirror",
  "Image": "http://url.to/image.png",
  "Widgets": []
}
```

## {Base URL}/mirrors
The mirror collection can be used to read, update and delete mirrors.

### GET /
Gets all mirrors for the authenticated User.

#### Request
Header: Authorization: Bearer {token}
Body: No body required

#### Response
* Status 401: Unauthorized (no authentication header)
* Status 200: (Array of WebApi.DataLayer.Models.Mirror)
```
[
  {
    "Id": "5f772e8c-392c-4c7f-a38b-2bee3bddb6fb",
    "User": "a@smart.mirror",
    "Name": "My Mirror",
    "Image": "http://url.to/image.png",
    "Widgets": [ 
      { 
        "Name": "Breaking News", 
        "Type": 1, 
        "WidgetSide": 1, 
        "Order": 2, 
        "Setting": {
          "Feeds": ["http://news.prodiver/rss.xml", "http://news.prodiver/rss.xml", "http://news.prodiver/rss.xml"]
        }
      },
      { 
        "Name": "Weather", 
        "Type": 2, 
        "WidgetSide": 1, 
        "Order": 1, 
        "Setting": {
          "City": "Minden",
          "AccessToken": "accesstokenforweatherprovider"
        }
      }
    ]
  }
]
```

### GET /{id:string}
Returns a mirror by id.

#### Request 
Header: No authentication required. 
Body: No body required.

#### Response
* Status 400: Bad Request (invalid id)
* Status 404: Not Found (mirror with id does not exists)
* Status 200 (WebApi.DataLayer.Models.Mirror):
```
{
  "Id": "5f772e8c-392c-4c7f-a38b-2bee3bddb6fb",
  "User": "a@smart.mirror",
  "Name": "My Mirror",
  "Image": "http://url.to/image.png",
  "Widgets": [ 
    { 
      "Name": "Breaking News", 
      "Type": 1, 
      "WidgetSide": 1, 
      "Order": 2, 
      "Setting": {
        "Feeds": ["http://news.prodiver/rss.xml", "http://news.prodiver/rss.xml", "http://news.prodiver/rss.xml"]
      }
    },
    { 
      "Name": "Weather", 
      "Type": 2, 
      "WidgetSide": 1, 
      "Order": 1, 
      "Setting": {
        "City": "Minden",
        "AccessToken": "accesstokenforweatherprovider"
      }
    }
  ]
}
```

### PUT /
Updates an existing mirror.

#### Request 
Header: No authentication required. 
Body: 
```
{
  "Id": "5f772e8c-392c-4c7f-a38b-2bee3bddb6fb",
  "User": "a@smart.mirror",
  "Name": "My Mirror",
  "Image": "http://url.to/image.png",
  "Widgets": [ 
    { 
      "Name": "Breaking News", 
      "Type": 1, 
      "WidgetSide": 1, 
      "Order": 2, 
      "Setting": {
        "Feeds": ["http://news.prodiver/rss.xml", "http://news.prodiver/rss.xml", "http://news.prodiver/rss.xml"]
      }
    },
    { 
      "Name": "Weather", 
      "Type": 2, 
      "WidgetSide": 1, 
      "Order": 1, 
      "Setting": {
        "City": "Minden",
        "AccessToken": "accesstokenforweatherprovider"
      }
    }
  ]
}
```

#### Response
* Status 400: Bad Request (malformed body)
* Status 401: Unauthorized (no authentication header)
* Status 404: Not Found (mirror with id does not exists)
* Status 200: Ok

### DELETE /{id:string}
Deletes an existing mirror.

#### Reuqest
Header: Authorization: Bearer {token}
Body: No body required

#### Response
* Status 400: Bad Request (invalid id)
* Status 401: Unauthorized (no authentication header)
* Status 404: Not Found (mirror with id does not exists)
* Status 200: Ok
