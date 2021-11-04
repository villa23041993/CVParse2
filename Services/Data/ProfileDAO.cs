using CVParse2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVParse2.Services.Data
{
    public class ProfileDAO
    {
        
        public ProfilModel Extract(Test rawData) {
            ProfilModel profilModel = new ProfilModel();
            profilModel.ID =1;
            profilModel.Name = "Hoa Nguyen";
            profilModel.Ort = "Hannover";
            profilModel.FirmaPosition = "Security";
            profilModel.Zusammenfassung = "i dont have money";
            profilModel.Erfahrung = "10 years";
            profilModel.Location = "Hannover";
            profilModel.Skill = "Gaming";
            return profilModel;
        }
    }
}