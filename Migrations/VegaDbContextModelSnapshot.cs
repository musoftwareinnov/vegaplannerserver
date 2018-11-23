﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using vega.Persistence;

namespace vega.Migrations
{
    [DbContext(typeof(VegaDbContext))]
    partial class VegaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("vega.Core.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Notes")
                        .HasMaxLength(1024);

                    b.Property<string>("SearchCriteria");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("vega.Core.Models.Drawing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("PlanningAppId");

                    b.HasKey("Id");

                    b.HasIndex("PlanningAppId");

                    b.ToTable("Drawings");
                });

            modelBuilder.Entity("vega.Core.Models.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("vega.Core.Models.Make", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Makes");
                });

            modelBuilder.Entity("vega.Core.Models.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MakeId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("MakeId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("vega.Core.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("VehicleId");

                    b.HasKey("Id");

                    b.HasIndex("VehicleId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("vega.Core.Models.PlanningApp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationNo");

                    b.Property<int>("CurrentPlanningStatusId");

                    b.Property<int>("CustomerId");

                    b.Property<string>("DescriptionOfWork");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Notes");

                    b.Property<string>("PlanningReferenceId");

                    b.Property<string>("SearchCriteria");

                    b.Property<int>("StateInitialiserId");

                    b.HasKey("Id");

                    b.HasIndex("CurrentPlanningStatusId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("StateInitialiserId");

                    b.ToTable("PlanningApps");
                });

            modelBuilder.Entity("vega.Core.Models.PlanningAppState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CompletionDate");

                    b.Property<bool>("CurrentState");

                    b.Property<int>("CustomDuration");

                    b.Property<bool>("CustomDurationSet");

                    b.Property<DateTime>("DueByDate");

                    b.Property<string>("Notes");

                    b.Property<int>("PlanningAppId");

                    b.Property<int>("StateInitialiserStateId");

                    b.Property<int>("StateStatusId");

                    b.Property<bool>("userModifiedDate");

                    b.HasKey("Id");

                    b.HasIndex("PlanningAppId");

                    b.HasIndex("StateInitialiserStateId");

                    b.HasIndex("StateStatusId");

                    b.ToTable("PlanningAppState");
                });

            modelBuilder.Entity("vega.Core.Models.PlanningAppStateCustomField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateValue");

                    b.Property<int>("IntValue");

                    b.Property<int?>("PlanningAppStateId");

                    b.Property<int>("StateInitialiserStateCustomFieldId");

                    b.Property<string>("StrValue");

                    b.HasKey("Id");

                    b.HasIndex("PlanningAppStateId");

                    b.ToTable("PlanningAppStateCustomFields");
                });

            modelBuilder.Entity("vega.Core.Models.StateInitialiser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("StateInitialisers");
                });

            modelBuilder.Entity("vega.Core.Models.StateInitialiserCustomField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Type");

                    b.Property<bool>("isMandatory");

                    b.Property<bool>("isPlanningAppField");

                    b.HasKey("Id");

                    b.ToTable("StateInitialiserCustomFields");
                });

            modelBuilder.Entity("vega.Core.Models.StateInitialiserStateCustomField", b =>
                {
                    b.Property<int>("StateInitialiserStateId");

                    b.Property<int>("StateInitialiserCustomFieldId");

                    b.HasKey("StateInitialiserStateId", "StateInitialiserCustomFieldId");

                    b.HasIndex("StateInitialiserCustomFieldId");

                    b.ToTable("StateInitialiserStateCustomFields");
                });

            modelBuilder.Entity("vega.Core.Models.States.StateInitialiserState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlertToCompletionTime");

                    b.Property<int>("CompletionTime");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("OrderId");

                    b.Property<int>("StateInitialiserId");

                    b.Property<bool>("canDelete");

                    b.Property<bool>("isDeleted");

                    b.HasKey("Id");

                    b.HasIndex("StateInitialiserId");

                    b.ToTable("StateInitialiserState");
                });

            modelBuilder.Entity("vega.Core.Models.States.StateStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GroupType");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("OrderId");

                    b.HasKey("Id");

                    b.ToTable("StateStatus");
                });

            modelBuilder.Entity("vega.Core.Models.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsRegistered");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<int>("ModelId");

                    b.HasKey("Id");

                    b.HasIndex("ModelId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("vega.Core.Models.VehicleFeature", b =>
                {
                    b.Property<int>("VehicleId");

                    b.Property<int>("FeatureId");

                    b.HasKey("VehicleId", "FeatureId");

                    b.HasIndex("FeatureId");

                    b.ToTable("VehicleFeatures");
                });

            modelBuilder.Entity("vegaplanner.Core.Models.DevelopmentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("DevelopmentType");
                });

            modelBuilder.Entity("vegaplanner.Core.Models.Security.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<long?>("FacebookId");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PictureUrl");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("vegaplanner.Core.Models.Security.InternalAppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Gender");

                    b.Property<string>("IdentityId");

                    b.Property<string>("Locale");

                    b.Property<string>("Location");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("vegaplannerserver.Core.Models.DescriptionOfWork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("DescriptionOfWork");
                });

            modelBuilder.Entity("vegaplannerserver.Core.Models.PlanningAppDrawers", b =>
                {
                    b.Property<int>("PlanningAppId");

                    b.Property<int>("InternalAppUserId");

                    b.HasKey("PlanningAppId", "InternalAppUserId");

                    b.HasIndex("InternalAppUserId");

                    b.ToTable("PlanningAppDrawers");
                });

            modelBuilder.Entity("vegaplannerserver.Core.Models.PlanningAppSurveyors", b =>
                {
                    b.Property<int>("PlanningAppId");

                    b.Property<int>("InternalAppUserId");

                    b.HasKey("PlanningAppId", "InternalAppUserId");

                    b.HasIndex("InternalAppUserId");

                    b.ToTable("PlanningAppSurveyors");
                });

            modelBuilder.Entity("vegaplannerserver.Core.Models.Settings.BusinessDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CurrBusDate");

                    b.Property<DateTime>("NextBusDate");

                    b.Property<DateTime>("PrevBusDate");

                    b.HasKey("Id");

                    b.ToTable("BusinessDates");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("vegaplanner.Core.Models.Security.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("vegaplanner.Core.Models.Security.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("vegaplanner.Core.Models.Security.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("vegaplanner.Core.Models.Security.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("vega.Core.Models.Customer", b =>
                {
                    b.OwnsOne("vega.Core.Models.Address", "CustomerAddress", b1 =>
                        {
                            b1.Property<int?>("CustomerId");

                            b1.Property<string>("AddressLine1")
                                .HasMaxLength(255);

                            b1.Property<string>("City");

                            b1.Property<string>("CompanyName")
                                .HasMaxLength(255);

                            b1.Property<string>("County");

                            b1.Property<string>("GeoLocation")
                                .HasMaxLength(20);

                            b1.Property<string>("Postcode")
                                .HasMaxLength(10);

                            b1.ToTable("Customers");

                            b1.HasOne("vega.Core.Models.Customer")
                                .WithOne("CustomerAddress")
                                .HasForeignKey("vega.Core.Models.Address", "CustomerId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("vega.Core.Models.Contact", "CustomerContact", b1 =>
                        {
                            b1.Property<int?>("CustomerId");

                            b1.Property<string>("EmailAddress")
                                .HasMaxLength(30);

                            b1.Property<string>("FirstName")
                                .HasMaxLength(30);

                            b1.Property<string>("LastName")
                                .HasMaxLength(30);

                            b1.Property<string>("TelephoneHome")
                                .HasMaxLength(30);

                            b1.Property<string>("TelephoneMobile")
                                .HasMaxLength(30);

                            b1.Property<string>("TelephoneWork")
                                .HasMaxLength(30);

                            b1.ToTable("Customers");

                            b1.HasOne("vega.Core.Models.Customer")
                                .WithOne("CustomerContact")
                                .HasForeignKey("vega.Core.Models.Contact", "CustomerId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("vega.Core.Models.Drawing", b =>
                {
                    b.HasOne("vega.Core.Models.PlanningApp")
                        .WithMany("Drawings")
                        .HasForeignKey("PlanningAppId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("vega.Core.Models.Model", b =>
                {
                    b.HasOne("vega.Core.Models.Make", "Make")
                        .WithMany("Models")
                        .HasForeignKey("MakeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("vega.Core.Models.Photo", b =>
                {
                    b.HasOne("vega.Core.Models.Vehicle")
                        .WithMany("Photos")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("vega.Core.Models.PlanningApp", b =>
                {
                    b.HasOne("vega.Core.Models.States.StateStatus", "CurrentPlanningStatus")
                        .WithMany()
                        .HasForeignKey("CurrentPlanningStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("vega.Core.Models.Customer", "Customer")
                        .WithMany("planningApps")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("vega.Core.Models.StateInitialiser", "StateInitialiser")
                        .WithMany()
                        .HasForeignKey("StateInitialiserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("vega.Core.Models.Address", "DevelopmentAddress", b1 =>
                        {
                            b1.Property<int>("PlanningAppId");

                            b1.Property<string>("AddressLine1")
                                .HasMaxLength(255);

                            b1.Property<string>("City");

                            b1.Property<string>("CompanyName")
                                .HasMaxLength(255);

                            b1.Property<string>("County");

                            b1.Property<string>("GeoLocation")
                                .HasMaxLength(20);

                            b1.Property<string>("Postcode")
                                .HasMaxLength(10);

                            b1.ToTable("PlanningApps");

                            b1.HasOne("vega.Core.Models.PlanningApp")
                                .WithOne("DevelopmentAddress")
                                .HasForeignKey("vega.Core.Models.Address", "PlanningAppId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("vega.Core.Models.Contact", "Developer", b1 =>
                        {
                            b1.Property<int?>("PlanningAppId");

                            b1.Property<string>("EmailAddress")
                                .HasMaxLength(30);

                            b1.Property<string>("FirstName")
                                .HasMaxLength(30);

                            b1.Property<string>("LastName")
                                .HasMaxLength(30);

                            b1.Property<string>("TelephoneHome")
                                .HasMaxLength(30);

                            b1.Property<string>("TelephoneMobile")
                                .HasMaxLength(30);

                            b1.Property<string>("TelephoneWork")
                                .HasMaxLength(30);

                            b1.ToTable("PlanningApps");

                            b1.HasOne("vega.Core.Models.PlanningApp")
                                .WithOne("Developer")
                                .HasForeignKey("vega.Core.Models.Contact", "PlanningAppId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("vega.Core.Models.PlanningAppState", b =>
                {
                    b.HasOne("vega.Core.Models.PlanningApp", "PlanningApp")
                        .WithMany("PlanningAppStates")
                        .HasForeignKey("PlanningAppId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("vega.Core.Models.States.StateInitialiserState", "state")
                        .WithMany()
                        .HasForeignKey("StateInitialiserStateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("vega.Core.Models.States.StateStatus", "StateStatus")
                        .WithMany()
                        .HasForeignKey("StateStatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("vega.Core.Models.PlanningAppStateCustomField", b =>
                {
                    b.HasOne("vega.Core.Models.PlanningAppState")
                        .WithMany("customFields")
                        .HasForeignKey("PlanningAppStateId");
                });

            modelBuilder.Entity("vega.Core.Models.StateInitialiserStateCustomField", b =>
                {
                    b.HasOne("vega.Core.Models.StateInitialiserCustomField", "StateInitialiserCustomField")
                        .WithMany()
                        .HasForeignKey("StateInitialiserCustomFieldId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("vega.Core.Models.States.StateInitialiserState", "StateInitialiserState")
                        .WithMany("StateInitialiserStateCustomFields")
                        .HasForeignKey("StateInitialiserStateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("vega.Core.Models.States.StateInitialiserState", b =>
                {
                    b.HasOne("vega.Core.Models.StateInitialiser")
                        .WithMany("States")
                        .HasForeignKey("StateInitialiserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("vega.Core.Models.Vehicle", b =>
                {
                    b.HasOne("vega.Core.Models.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("vega.Core.Models.Contact", "Contact", b1 =>
                        {
                            b1.Property<int>("VehicleId");

                            b1.Property<string>("EmailAddress")
                                .HasMaxLength(30);

                            b1.Property<string>("FirstName")
                                .HasMaxLength(30);

                            b1.Property<string>("LastName")
                                .HasMaxLength(30);

                            b1.Property<string>("TelephoneHome")
                                .HasMaxLength(30);

                            b1.Property<string>("TelephoneMobile")
                                .HasMaxLength(30);

                            b1.Property<string>("TelephoneWork")
                                .HasMaxLength(30);

                            b1.ToTable("Vehicles");

                            b1.HasOne("vega.Core.Models.Vehicle")
                                .WithOne("Contact")
                                .HasForeignKey("vega.Core.Models.Contact", "VehicleId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("vega.Core.Models.VehicleFeature", b =>
                {
                    b.HasOne("vega.Core.Models.Feature", "Feature")
                        .WithMany()
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("vega.Core.Models.Vehicle", "Vehicle")
                        .WithMany("Features")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("vegaplanner.Core.Models.Security.InternalAppUser", b =>
                {
                    b.HasOne("vegaplanner.Core.Models.Security.AppUser", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId");
                });

            modelBuilder.Entity("vegaplannerserver.Core.Models.PlanningAppDrawers", b =>
                {
                    b.HasOne("vegaplanner.Core.Models.Security.InternalAppUser", "InternalAppUser")
                        .WithMany()
                        .HasForeignKey("InternalAppUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("vega.Core.Models.PlanningApp", "PlanningApp")
                        .WithMany()
                        .HasForeignKey("PlanningAppId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("vegaplannerserver.Core.Models.PlanningAppSurveyors", b =>
                {
                    b.HasOne("vegaplanner.Core.Models.Security.InternalAppUser", "InternalAppUser")
                        .WithMany()
                        .HasForeignKey("InternalAppUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("vega.Core.Models.PlanningApp", "PlanningApp")
                        .WithMany("Surveyors")
                        .HasForeignKey("PlanningAppId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
