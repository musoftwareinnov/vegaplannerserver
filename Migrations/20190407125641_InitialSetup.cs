using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace vega.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FacebookId = table.Column<long>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessDates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PrevBusDate = table.Column<DateTime>(nullable: false),
                    CurrBusDate = table.Column<DateTime>(nullable: false),
                    NextBusDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerContact_CustomerTitleId = table.Column<int>(nullable: false),
                    CustomerContact_CustomerTitle = table.Column<string>(nullable: true),
                    CustomerContact_FirstName = table.Column<string>(maxLength: 30, nullable: true),
                    CustomerContact_LastName = table.Column<string>(maxLength: 30, nullable: true),
                    CustomerContact_TelephoneWork = table.Column<string>(maxLength: 30, nullable: true),
                    CustomerContact_TelephoneMobile = table.Column<string>(maxLength: 30, nullable: true),
                    CustomerContact_TelephoneHome = table.Column<string>(maxLength: 30, nullable: true),
                    CustomerContact_EmailAddress = table.Column<string>(maxLength: 30, nullable: true),
                    CustomerAddress_CompanyName = table.Column<string>(maxLength: 255, nullable: true),
                    CustomerAddress_AddressLine1 = table.Column<string>(maxLength: 255, nullable: true),
                    CustomerAddress_City = table.Column<string>(nullable: true),
                    CustomerAddress_County = table.Column<string>(nullable: true),
                    CustomerAddress_Postcode = table.Column<string>(maxLength: 10, nullable: true),
                    CustomerAddress_GeoLocation = table.Column<string>(maxLength: 20, nullable: true),
                    SearchCriteria = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DescriptionOfWork",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescriptionOfWork", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DevelopmentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevelopmentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    DefaultAmount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Makes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Makes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGenerators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGenerators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StateInitialiserCustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    isPlanningAppField = table.Column<bool>(nullable: false),
                    isMandatory = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateInitialiserCustomFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StateInitialisers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateInitialisers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StateStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    GroupType = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Title",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Title", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    MakeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Makes_MakeId",
                        column: x => x.MakeId,
                        principalTable: "Makes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGeneratorSequence",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SeqId = table.Column<int>(nullable: false),
                    GeneratorId = table.Column<int>(nullable: true),
                    ProjectGeneratorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGeneratorSequence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectGeneratorSequence_StateInitialisers_GeneratorId",
                        column: x => x.GeneratorId,
                        principalTable: "StateInitialisers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectGeneratorSequence_ProjectGenerators_ProjectGeneratorId",
                        column: x => x.ProjectGeneratorId,
                        principalTable: "ProjectGenerators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StateInitialiserState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    OrderId = table.Column<int>(nullable: false),
                    CompletionTime = table.Column<int>(nullable: false),
                    AlertToCompletionTime = table.Column<int>(nullable: false),
                    StateInitialiserId = table.Column<int>(nullable: false),
                    isDeleted = table.Column<bool>(nullable: false),
                    canDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateInitialiserState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateInitialiserState_StateInitialisers_StateInitialiserId",
                        column: x => x.StateInitialiserId,
                        principalTable: "StateInitialisers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanningApps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    PlanningReferenceId = table.Column<string>(nullable: true),
                    ProjectGeneratorId = table.Column<int>(nullable: false),
                    CurrentPlanningStatusId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    ApplicationNo = table.Column<string>(nullable: true),
                    Developer_CustomerTitleId = table.Column<int>(nullable: false),
                    Developer_CustomerTitle = table.Column<string>(nullable: true),
                    Developer_FirstName = table.Column<string>(maxLength: 30, nullable: true),
                    Developer_LastName = table.Column<string>(maxLength: 30, nullable: true),
                    Developer_TelephoneWork = table.Column<string>(maxLength: 30, nullable: true),
                    Developer_TelephoneMobile = table.Column<string>(maxLength: 30, nullable: true),
                    Developer_TelephoneHome = table.Column<string>(maxLength: 30, nullable: true),
                    Developer_EmailAddress = table.Column<string>(maxLength: 30, nullable: true),
                    DevelopmentAddress_CompanyName = table.Column<string>(maxLength: 255, nullable: true),
                    DevelopmentAddress_AddressLine1 = table.Column<string>(maxLength: 255, nullable: true),
                    DevelopmentAddress_City = table.Column<string>(nullable: true),
                    DevelopmentAddress_County = table.Column<string>(nullable: true),
                    DevelopmentAddress_Postcode = table.Column<string>(maxLength: 10, nullable: true),
                    DevelopmentAddress_GeoLocation = table.Column<string>(maxLength: 20, nullable: true),
                    SearchCriteria = table.Column<string>(nullable: true),
                    DescriptionOfWork = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningApps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanningApps_StateStatus_CurrentPlanningStatusId",
                        column: x => x.CurrentPlanningStatusId,
                        principalTable: "StateStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanningApps_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModelId = table.Column<int>(nullable: false),
                    IsRegistered = table.Column<bool>(nullable: false),
                    Contact_CustomerTitleId = table.Column<int>(nullable: false),
                    Contact_CustomerTitle = table.Column<string>(nullable: true),
                    Contact_FirstName = table.Column<string>(maxLength: 30, nullable: true),
                    Contact_LastName = table.Column<string>(maxLength: 30, nullable: true),
                    Contact_TelephoneWork = table.Column<string>(maxLength: 30, nullable: true),
                    Contact_TelephoneMobile = table.Column<string>(maxLength: 30, nullable: true),
                    Contact_TelephoneHome = table.Column<string>(maxLength: 30, nullable: true),
                    Contact_EmailAddress = table.Column<string>(maxLength: 30, nullable: true),
                    LastUpdate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StateInitialiserStateCustomFields",
                columns: table => new
                {
                    StateInitialiserStateId = table.Column<int>(nullable: false),
                    StateInitialiserCustomFieldId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateInitialiserStateCustomFields", x => new { x.StateInitialiserStateId, x.StateInitialiserCustomFieldId });
                    table.ForeignKey(
                        name: "FK_StateInitialiserStateCustomFields_StateInitialiserCustomFields_StateInitialiserCustomFieldId",
                        column: x => x.StateInitialiserCustomFieldId,
                        principalTable: "StateInitialiserCustomFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StateInitialiserStateCustomFields_StateInitialiserState_StateInitialiserStateId",
                        column: x => x.StateInitialiserStateId,
                        principalTable: "StateInitialiserState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Drawings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(maxLength: 255, nullable: false),
                    PlanningAppId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drawings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drawings_PlanningApps_PlanningAppId",
                        column: x => x.PlanningAppId,
                        principalTable: "PlanningApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanningAppAdmins",
                columns: table => new
                {
                    PlanningAppId = table.Column<int>(nullable: false),
                    AppUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningAppAdmins", x => new { x.PlanningAppId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_PlanningAppAdmins_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanningAppAdmins_PlanningApps_PlanningAppId",
                        column: x => x.PlanningAppId,
                        principalTable: "PlanningApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanningAppDrawers",
                columns: table => new
                {
                    PlanningAppId = table.Column<int>(nullable: false),
                    AppUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningAppDrawers", x => new { x.PlanningAppId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_PlanningAppDrawers_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanningAppDrawers_PlanningApps_PlanningAppId",
                        column: x => x.PlanningAppId,
                        principalTable: "PlanningApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanningAppFees",
                columns: table => new
                {
                    PlanningAppId = table.Column<int>(nullable: false),
                    FeeId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningAppFees", x => new { x.PlanningAppId, x.FeeId });
                    table.ForeignKey(
                        name: "FK_PlanningAppFees_Fees_FeeId",
                        column: x => x.FeeId,
                        principalTable: "Fees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanningAppFees_PlanningApps_PlanningAppId",
                        column: x => x.PlanningAppId,
                        principalTable: "PlanningApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanningAppState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlanningAppId = table.Column<int>(nullable: false),
                    GeneratorOrder = table.Column<int>(nullable: false),
                    GeneratorName = table.Column<string>(nullable: true),
                    StateInitialiserStateId = table.Column<int>(nullable: false),
                    DueByDate = table.Column<DateTime>(nullable: false),
                    userModifiedDate = table.Column<bool>(nullable: false),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    StateStatusId = table.Column<int>(nullable: false),
                    CurrentState = table.Column<bool>(nullable: false),
                    CustomDurationSet = table.Column<bool>(nullable: false),
                    CustomDuration = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningAppState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanningAppState_PlanningApps_PlanningAppId",
                        column: x => x.PlanningAppId,
                        principalTable: "PlanningApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PlanningAppState_StateInitialiserState_StateInitialiserStateId",
                        column: x => x.StateInitialiserStateId,
                        principalTable: "StateInitialiserState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanningAppState_StateStatus_StateStatusId",
                        column: x => x.StateStatusId,
                        principalTable: "StateStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanningAppSurveyors",
                columns: table => new
                {
                    PlanningAppId = table.Column<int>(nullable: false),
                    AppUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningAppSurveyors", x => new { x.PlanningAppId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_PlanningAppSurveyors_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanningAppSurveyors_PlanningApps_PlanningAppId",
                        column: x => x.PlanningAppId,
                        principalTable: "PlanningApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FileName = table.Column<string>(maxLength: 255, nullable: false),
                    VehicleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleFeatures",
                columns: table => new
                {
                    VehicleId = table.Column<int>(nullable: false),
                    FeatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleFeatures", x => new { x.VehicleId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_VehicleFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleFeatures_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanningAppStateCustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StateInitialiserStateCustomFieldId = table.Column<int>(nullable: false),
                    StrValue = table.Column<string>(nullable: true),
                    IntValue = table.Column<int>(nullable: false),
                    DateValue = table.Column<DateTime>(nullable: false),
                    PlanningAppStateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningAppStateCustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanningAppStateCustomFields_PlanningAppState_PlanningAppStateId",
                        column: x => x.PlanningAppStateId,
                        principalTable: "PlanningAppState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Drawings_PlanningAppId",
                table: "Drawings",
                column: "PlanningAppId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_MakeId",
                table: "Models",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_VehicleId",
                table: "Photos",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppAdmins_AppUserId",
                table: "PlanningAppAdmins",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppDrawers_AppUserId",
                table: "PlanningAppDrawers",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppFees_FeeId",
                table: "PlanningAppFees",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningApps_CurrentPlanningStatusId",
                table: "PlanningApps",
                column: "CurrentPlanningStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningApps_CustomerId",
                table: "PlanningApps",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppState_PlanningAppId",
                table: "PlanningAppState",
                column: "PlanningAppId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppState_StateInitialiserStateId",
                table: "PlanningAppState",
                column: "StateInitialiserStateId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppState_StateStatusId",
                table: "PlanningAppState",
                column: "StateStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppStateCustomFields_PlanningAppStateId",
                table: "PlanningAppStateCustomFields",
                column: "PlanningAppStateId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningAppSurveyors_AppUserId",
                table: "PlanningAppSurveyors",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGeneratorSequence_GeneratorId",
                table: "ProjectGeneratorSequence",
                column: "GeneratorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectGeneratorSequence_ProjectGeneratorId",
                table: "ProjectGeneratorSequence",
                column: "ProjectGeneratorId");

            migrationBuilder.CreateIndex(
                name: "IX_StateInitialiserState_StateInitialiserId",
                table: "StateInitialiserState",
                column: "StateInitialiserId");

            migrationBuilder.CreateIndex(
                name: "IX_StateInitialiserStateCustomFields_StateInitialiserCustomFieldId",
                table: "StateInitialiserStateCustomFields",
                column: "StateInitialiserCustomFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleFeatures_FeatureId",
                table: "VehicleFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ModelId",
                table: "Vehicles",
                column: "ModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BusinessDates");

            migrationBuilder.DropTable(
                name: "DescriptionOfWork");

            migrationBuilder.DropTable(
                name: "DevelopmentType");

            migrationBuilder.DropTable(
                name: "Drawings");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "PlanningAppAdmins");

            migrationBuilder.DropTable(
                name: "PlanningAppDrawers");

            migrationBuilder.DropTable(
                name: "PlanningAppFees");

            migrationBuilder.DropTable(
                name: "PlanningAppStateCustomFields");

            migrationBuilder.DropTable(
                name: "PlanningAppSurveyors");

            migrationBuilder.DropTable(
                name: "ProjectGeneratorSequence");

            migrationBuilder.DropTable(
                name: "StateInitialiserStateCustomFields");

            migrationBuilder.DropTable(
                name: "Title");

            migrationBuilder.DropTable(
                name: "VehicleFeatures");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Fees");

            migrationBuilder.DropTable(
                name: "PlanningAppState");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ProjectGenerators");

            migrationBuilder.DropTable(
                name: "StateInitialiserCustomFields");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "PlanningApps");

            migrationBuilder.DropTable(
                name: "StateInitialiserState");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "StateStatus");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "StateInitialisers");

            migrationBuilder.DropTable(
                name: "Makes");
        }
    }
}
