# How to use Ordoclic Third-Party ?

step 1  :

    Build the below object :

    ```
    {
        "partnerId":"value",
        "rpps": "value",
        "dateTime": "2019-12-10T19:08:41+00:00"
    }
    ```

step 2 :

    Sign this object with the private key given by Ordoclic team
    The algorithme is : PKCS1v15
    You'll find an example of signature in the example project
    With the signature build the below object :
    ```
    {
        "partnerId":"value",
        "rpps": "value",
        "dateTime": "2019-12-10T19:08:41+00:00",
        "sig": "value"
    }
    ```

step 3 :

    Send a request to Ordoclic third-party service

    location staging : https://partners.staging.ordoclic.fr/v1/auth
    location production : https://partners.ordoclic.fr/v1/auth

    route : `POST {{location}}/third-party/auth`
    header :
    ```
    Content-Type : application/json
    ```
    body :
    ```
        {
        "partnerId":"value",
        "rpps": "value",
        "dateTime": "2019-12-10T19:08:41+00:00",
        "sig": "value"
    }
    ```
    response :
    ```
    {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE1NzYyMDQ2NjcsImlkZW50aXR5Ijoie1wicHJvZmVzc2lvbmFsXCI6e1wiaWRcIjpcIjk5ZWVjMmNhLTJlMDktNGE0OS04MDMyLWM2ZTE4YmYxYTM4Y1wiLFwicnBwc1wiOlwiOTk3MDAwOTI2NDBcIixcImZpcnN0TmFtZVwiOlwiUmFjaGVsbGVcIixcImxhc3ROYW1lXCI6XCJIT1VERVwiLFwiZnVsbE5hbWVcIjpcIlJhY2hlbGxlIEhPVURFXCIsXCJlbWFpbFwiOlwibWF4aW1lLmxhcnF1ZW1pbkBvcmRvY2xpYy5mclwiLFwiZ2VuZGVyXCI6XCJGXCIsXCJiaXJ0aGRheVwiOlwiMTkzMy0wMi0xNlQwMDowMDowMFpcIixcImNyZWF0ZWRBdFwiOlwiMTk4NC0wNi0xOFQxMzo0OTo0NS43ODcxMTErMDI6MDBcIixcImxhc3RDb25uZWN0aW9uXCI6XCIyMDE5LTEyLTEyVDE5OjM2OjEzLjU1Mjc5NSswMTowMFwiLFwidGl0bGVJRFwiOjIsXCJqb2JJRFwiOjMsXCJzcGVjaWFsdHlJZFwiOjMzMCxcImxhdFwiOjAsXCJsbmdcIjowLFwiYWRlbGlcIjpcIlwiLFwiYWN0aXZhdGVkXCI6dHJ1ZSxcImFjdGl2YXRlZEF0XCI6XCIyMDE5LTA2LTE4VDEzOjQ5OjQ3LjcwMDY3NlpcIixcImNyZWF0ZWRCeVwiOlwiMjZlOWE0YmMtMjg3MC00NTYxLWFiNTktYzUxNjdlOThlZGZiXCIsXCJjcmVhdGlvbk1ldGhvZFwiOlwiU3RhZmYgQWRtaW5cIn0sXCJyb2xlXCI6XCJ0aGlyZC1wYXJ0eVwiLFwiYWNjb3VudElkXCI6XCIyMTg2YzVmMy05ODVmLTQ0YjMtYTAzMC00M2QzNTk4YTA0ZjlcIixcImVudGl0eUlkXCI6bnVsbH0iLCJyb2xlIjoidGhpcmQtcGFydHkifQ.ea69gYg1hMzFtVOh1VigZy1Lu7TMlTRjyY-7TflPt4w",
        "identity": {
            "professional": {
                "id": "99eec2ca-2e09-4a49-8032-c6e18bf1a38c",
                "rpps": "99700092640",
                "firstName": "Rachelle",
                "lastName": "HOUDE",
                "fullName": "Rachelle HOUDE",
                "email": "hugo@ordoclic.fr",
                "gender": "F",
                "birthday": "1933-02-16T00:00:00Z",
                "createdAt": "1984-06-18T13:49:45.787111+02:00",
                "lastConnection": "2019-12-12T19:36:13.552795+01:00",
                "titleID": 2,
                "jobID": 3,
                "specialtyId": 330,
                "lat": 0,
                "lng": 0,
                "adeli": "",
                "activated": true,
                "activatedAt": "2019-06-18T13:49:47.700676Z",
                "createdBy": "26e9a4bc-2870-4561-ab59-c5167e98edfb",
                "creationMethod": "Staff Admin"
            },
            "role": "third-party",
            "accountId": "2186c5f3-985f-44b3-a030-43d3598a04f9",
            "partnerId": "2186c5f3-985f-44b3-a030-43d3598a04f9",
            "entityId": null
        }
    }
    ```
step 4 :

    Get the `token` field like a Bearer token to https://partners.staging.ordoclic.fr or  https://partners.ordoclic.fr
    The documentation : https://partners.test.ordoclic.fr/swaggerui/#/

