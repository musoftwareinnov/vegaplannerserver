using System.Collections.Generic;
using System.Security.Claims;
using static vegaplanner.Core.Models.Security.Helpers.Constants;
using static vegaplanner.Core.Models.Security.Helpers.Constants.Strings;

namespace vegaplannerserver.Core.Security
{
    public static class RoleClaimSetup
    {
        private static List<string> AdminClaims = 
            new List<string>(new string[] { "app", //Apps in Progress
                                            "apc", //Apps Create
                                            "ape", //Apps Edit
                                            "apv", //Apps View
                                            "apn", //Apps Next State
                                            "apa", //Apps Archive
                                            "csc", //Customer Create
                                            "cse", //Customer Edit
                                            "csv", //Customer View
                                            "csd", //Customer Delete
                                            "gnc", //Generator Create
                                            "gne", //Generator Edit
                                            "gnv", //Generator View
                                            "gnd", //Generator Delete                                        
                                            });
        private static List<string> DesignerSurveyClaims = 
            new List<string>(new string[] { "app", //Apps in Progress
                                            "apc", //Apps Create
                                            "ape", //Apps Edit
                                            "apv", //Apps View
                                            "apn", //Apps Next State
                                            "apa", //Apps Archive                                       
                                            });
        private static List<string> DesignerDrawerClaims = 
            new List<string>(new string[] { "app", //Apps in Progress
                                            "apc", //Apps Create
                                            "ape", //Apps Edit
                                            "apv", //Apps View
                                            "apn", //Apps Next State
                                            "apa", //Apps Archive   
                                            "csv", //Customer View                                    
                                            });

        // public RoleClaimSetup() {

        // }

        public static RoleClaim RoleClaimSetupAdmin() {
            RoleClaim role = new RoleClaim();
            role.Role.Name = Strings.JwtClaims.AdminUser;
            foreach(var claim in AdminClaims) 
                role.Claims.Add(new Claim(claim, "1"));

            //Add policy role for controllers
            role.Claims.Add(new Claim(JwtClaimIdentifiers.rol, JwtClaims.AdminUser));
            return role;
        }
        public static RoleClaim RoleClaimSetupDesignerSurvey() {
            RoleClaim role = new RoleClaim();
            role.Role.Name = Strings.JwtClaims.DesignerSurveyUser;
            foreach(var claim in DesignerSurveyClaims) 
                role.Claims.Add(new Claim(claim, "1"));

            //Add policy role for controllers
            role.Claims.Add(new Claim(JwtClaimIdentifiers.rol, JwtClaims.DesignerSurveyUser));
            return role;
        }
        
        public static RoleClaim RoleClaimSetupDesignerDrawer() {
            RoleClaim role = new RoleClaim();
            role.Role.Name = Strings.JwtClaims.DesignerDrawingUser;
            foreach(var claim in DesignerDrawerClaims) 
                role.Claims.Add(new Claim(claim, "1"));

            //Add policy role for controllers
            role.Claims.Add(new Claim(JwtClaimIdentifiers.rol, JwtClaims.DesignerDrawingUser));
            return role;
        }
    }
}