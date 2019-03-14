using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AspNetCore.Identity.Api.Mappings;
using AspNetCore.Identity.Data.DbContext;
using AspNetCore.Identity.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.Web.Api
{
    /// <summary>
    /// The startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor for the startup.
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime.  Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Specifies a contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Automapper Configuration            
            AutoMapperConfiguration.OrganizationProfile();

            // Add CORS
            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                // Define what we are allowing or not allowing.
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            // Add Entity Framework
            services.AddDbContext<ApplicationUserDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));

            // Add Identity Framework
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationUserDbContext>();

            // Create the signing key for the JWT Token.
            var signingKey = 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]));

            // Add Authentication
            services.AddAuthentication(options =>
            {
                // Setup the schemas being used.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                // Setup the properties of the JWT Token we want.
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;

                //
                // TokenValidationParameters - parameter options for the JWT Token validation.
                //
                // ActorValidationParameters - Gets or sets TokenValidationParameters.
                // AudienceValidator - Gets or sets a delegate that will be used to validate the audience.
                // AuthenticationType - Gets or sets the AuthenticationType when creating a ClaimsIdentity.
                // ClockSkew - Gets or sets the clock skew to apply when validating a time.
                // CryptoProviderFactory - Users can override the default CryptoProviderFactory with this property.This factory will be used for creating signature providers.
                // IssuerSigningKey - Gets or sets the SecurityKey that is to be used for signature validation.
                // IssuerSigningKeyResolver - Gets or sets a delegate that will be called to retrieve a SecurityKey used for signature validation.
                // IssuerSigningKeys - Gets or sets an IEnumerable<T> used for signature validation.
                // IssuerSigningKeyValidator - Gets or sets a delegate for validating the SecurityKey that signed the token.
                // IssuerValidator - Gets or sets a delegate that will be used to validate the issuer of the token.
                // LifetimeValidator - Gets or sets a delegate that will be used to validate the lifetime of the token
                // NameClaimType - Gets or sets a String that defines the NameClaimType.
                // NameClaimTypeRetriever - Gets or sets a delegate that will be called to obtain the NameClaimType to use when creating a ClaimsIdentity after validating a token.
                // PropertyBag - Gets or sets the IDictionary<TKey, TValue> that contains a collection of custom key/value pairs. This allows addition of parameters that could be used in custom token validation scenarios.
                // RequireExpirationTime - Gets or sets a value indicating whether tokens must have an 'expiration' value.
                // RequireSignedTokens - Gets or sets a value indicating whether a SecurityToken can be considered valid if not signed.
                // RoleClaimType - Gets or sets the String that defines the RoleClaimType.
                // RoleClaimTypeRetriever - Gets or sets a delegate that will be called to obtain the RoleClaimType to use when creating a ClaimsIdentity after validating a token.
                // SaveSigninToken - Gets or sets a boolean to control if the original token should be saved after the security token is validated.
                // SignatureValidator - Gets or sets a delegate that will be used to validate the signature of the token.
                // TokenDecryptionKey - Gets or sets the SecurityKey that is to be used for decryption.
                // TokenDecryptionKeyResolver - Gets or sets a delegate that will be called to retreive a SecurityKey used for decryption.
                // TokenDecryptionKeys - Gets or sets the IEnumerable<T> that is to be used for decrypting inbound tokens.
                // TokenReader - Gets or sets a delegate that will be used to read the token.
                // TokenReplayCache - Gets or set the ITokenReplayCache that store tokens that can be checked to help detect token replay.
                // TokenReplayValidator - Gets or sets a delegate that will be used to validate the token replay of the token
                // ValidateActor - Gets or sets a value indicating if an actor token is detected, whether it should be validated.
                // ValidateAudience - Gets or sets a boolean to control if the audience will be validated during token validation.
                // ValidateIssuer - Gets or sets a boolean to control if the issuer will be validated during token validation.
                // ValidateIssuerSigningKey - Gets or sets a boolean that controls if validation of the SecurityKey that signed the securityToken is called.
                // ValidateLifetime - Gets or sets a boolean to control if the lifetime will be validated during token validation.
                // ValidateTokenReplay - Gets or sets a boolean to control if the token replay will be validated during token validation.
                // ValidAudience - Gets or sets a string that represents a valid audience that will be used to check against the token's audience.
                // ValidAudiences - Gets or sets the IEnumerable<T> that contains valid audiences that will be used to check against the token's audience.
                // ValidIssuer - Gets or sets a String that represents a valid issuer that will be used to check against the token's issuer.
                // ValidIssuers - Gets or sets the IEnumerable<T> that contains valid issuers that will be used to check against the token's issuer.

                // Setup the parameters on the JWT Token.
                config.TokenValidationParameters = new TokenValidationParameters()
               {
                    // The security key being used for signature validation.
                    IssuerSigningKey = signingKey,
                    // Sets whether audience validation will happen during token validation.
                    ValidateAudience = true,
                    // A valid audience that will be used to check against the token's audience.
                    ValidAudience = Configuration["Tokens:Audience"],
                    // Sets whether the issuer will be validated during token validation.
                    ValidateIssuer = true,
                    // A valid issuer that will be used to check against the token's issuer.
                    ValidIssuer = Configuration["Tokens:Issuer"],
                    // Sets whether the lifetime of the token will be validated during token 
                    // validation.
                    ValidateLifetime = true,
                    // Sets whether the security key that the token was signed with will be 
                    // validated during token validation.
                    ValidateIssuerSigningKey = true
                };
            });

            // Swagger Generation Configuration.
            services.AddSwaggerGen(c =>
            {   // Provide a full description for your API, terms of service or 
                // even contact and licensing information    
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Identity API - V1",
                        Version = "v1",
                        Description = "Created by Someone",
                        TermsOfService = "Knock yourself out",
                        Contact = new Contact
                        {
                            Name = "Web API Team",
                            Email = "webapi@test.com"
                        },
                        License = new License
                        {
                            Name = "Apache 2.0",
                            Url = "http://www.apache.org/licenses/LICENSE-2.0.html"
                        }
                    }
                );

                // Add Grouping to SwaggerUI.
                c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));

                // This is a fix for the deprecated method.
                c.TagActionsBy((api) => {
                    List<string> groupNames = new List<string>
                    {
                        api.GroupName
                    };

                    return groupNames;
                });

                // Add Http and Https Schemas to SwaggerUI.
                c.DocumentFilter<SchemaFilter>();

                // Include Descriptions from XML Comments
                var filePath = Path.Combine(AppContext.BaseDirectory, "IdentityApi.xml");
                c.IncludeXmlComments(filePath);
            });

            // Add the Mvc service.
            services.AddMvc()
                .AddJsonOptions(opts =>
                {
                    // Force Camel Case to JSON
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                    // Pretty Print Swagger JSON
                    opts.SerializerSettings.Formatting = Formatting.Indented;

                    opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        /// <summary>
        /// This method gets called by the runtime.  Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Provides the mechanisms to configure the application's request pipleline.</param>
        /// <param name="env">Provides information about the web hosting environment the application is running in.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Determine if we are running in the development environment.
            if (env.IsDevelopment())
            {
                // Use the developer exception page since we are in development environment.
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. 
                // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Use cors.
            app.UseCors("Cors");

            // Use authentication.
            app.UseAuthentication();

            // Redirect if this is an HTTP request to HTTPS.
            app.UseHttpsRedirection();

            // Use MVC.
            app.UseMvc();

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API V1");
            });
        }

        /// <summary>
        /// Filter for Swagger.
        /// </summary>
        public class SchemaFilter : IDocumentFilter
        {
            /// <summary>
            /// Method to apply the filter entries.
            /// </summary>
            /// <param name="swaggerDoc"></param>
            /// <param name="context"></param>
            public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
            {
                swaggerDoc.Schemes = new string[] { "http", "https" };
            }
        }
    }
}
