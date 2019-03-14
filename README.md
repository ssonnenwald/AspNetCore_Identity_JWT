# AspNetCore_Identity_JWT
AspNetCore Identity JWT API

This is an implementation of using the Identity component of AspNetCore to implement JWT Token authentication for 
a user signing into and out of a application.  There are two projects here, the AspNetCore.Identity.Data and the AspNetCore.Identity.Api.
The AspNetCore.Identity.Data project is just creating the Identity database and extending the application user with two properties.
This project uses migrations to create the database setup.  The second project AspNetCore.Identity.Api is the web api that exposes login,
signout, register and refreshtoken.
