using CVParse2.Models;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace CVParse2.Services.Data
{
    public class ProfileDAO
    {

        public ProfilModel Extract(Test rawData)
        {
            ProfilModel profilModel = new ProfilModel();
            profilModel.ID = 1;
            profilModel.Name = "";
            profilModel.Ort = "";
            profilModel.FirmaPosition = "";
            profilModel.Zusammenfassung = "";
            profilModel.Erfahrung = "";
            profilModel.Location = "";
            profilModel.Skill = "";

            string tmpData = "";
            string editorData = rawData.Description;

            if (editorData == null)
            {
                profilModel.Name = "k.A";
                profilModel.Ort = "k.A";
                profilModel.FirmaPosition = "k.A";
                profilModel.Zusammenfassung = "k.A";
                profilModel.Erfahrung = "k.A";
                profilModel.Location = "k.A";
                profilModel.Skill = "k.A";
                return profilModel;
            }
            else if (editorData.Substring(0, 3) == "<p>")
            {
                profilModel.Name = "k.A";
                profilModel.Ort = "k.A";
                profilModel.FirmaPosition = "k.A";
                profilModel.Zusammenfassung = "k.A";
                profilModel.Erfahrung = "k.A";
                profilModel.Location = "k.A";
                profilModel.Skill = "k.A";
                return profilModel;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(editorData);

                int nameStart = skipHTMLTags(editorData, getPosition(editorData, "<h1>", 1));
                int nameEnd = nameStart;
                while (editorData[nameEnd] != '<')
                {
                    profilModel.Name += editorData[nameEnd];
                    nameEnd++;
                }

                tmpData = editorData.Substring(nameStart);
                int jobStart = getPosition(tmpData, "<p>", 1) + 3;    //start index of job/position
                string tmpJob = "";                    //buffer string for inspecting
                while (tmpData[jobStart] != '<')
                {
                    tmpJob += tmpData[jobStart];
                    jobStart++;
                }
                if (Regex.Match(tmpJob, @"degree\sconnection", RegexOptions.IgnoreCase).Success || Regex.Match(tmpJob, @"She\/Her", RegexOptions.IgnoreCase).Success                   //if tmpJob contains one of the mentioned 
                    || Regex.Match(tmpJob, @"He\/Him", RegexOptions.IgnoreCase).Success || Regex.Match(tmpJob, @"They\/Them", RegexOptions.IgnoreCase).Success)
                {
                    jobStart = skipHTMLTags(tmpData, getPosition(tmpData, "<p>", 2));                                              //search for next position
                    while (tmpData[jobStart] != '<')
                    {                                                 //look for the next < character and append the prior characters to job 
                        profilModel.FirmaPosition += tmpData[jobStart];
                        jobStart++;
                    }
                }
                else
                {
                    profilModel.FirmaPosition += tmpJob;
                }
                profilModel.FirmaPosition = profilModel.FirmaPosition.Replace("&ndash;", " - ");
                profilModel.FirmaPosition = profilModel.FirmaPosition.Replace("&auml;", "ä");
                profilModel.FirmaPosition = profilModel.FirmaPosition.Replace("&uuml;", "ü");
                profilModel.FirmaPosition = profilModel.FirmaPosition.Replace("&ouml;", "ö");
                profilModel.FirmaPosition = profilModel.FirmaPosition.Replace("&nbsp;", " ");
                profilModel.FirmaPosition = profilModel.FirmaPosition.Replace("&#39;", "'");
                profilModel.FirmaPosition = profilModel.FirmaPosition.Replace("&amp;", "&");

                string tagless = removeTags(editorData);                   //buffer string that cha<<nge every html tags into $
                List<String> dataArray = stringToArray(tagless, '$');           //turn string into array, only to make it easier to track the address
                /*foreach(String s in dataArray)
                {
                    System.Diagnostics.Debug.WriteLine(s);
                }*/
                //System.Diagnostics.Debug.WriteLine(tagless);
                profilModel.Ort += dataArray.ElementAt(dataArray.IndexOf("Contact info") - 1);                   //the address is always before Contact Info
                profilModel.Ort = profilModel.Ort.Replace("&ndash;", " - ");
                profilModel.Ort = profilModel.Ort.Replace("&auml;", "ä");
                profilModel.Ort = profilModel.Ort.Replace("&uuml;", "ü");
                profilModel.Ort = profilModel.Ort.Replace("&ouml;", "ö");
                profilModel.Ort = profilModel.Ort.Replace("&nbsp;", " ");
                profilModel.Ort = profilModel.Ort.Replace("&#39;", "'");
                profilModel.Ort = profilModel.Ort.Replace("&amp;", "&");

                if (editorData.Contains("<h2>About</h2>"))
                {
                    profilModel.Zusammenfassung += "\n";
                    tmpData = editorData.Substring(editorData.IndexOf("<h2>About</h2>"));
                    int nexth2 = getPosition(tmpData, "<h2>", 2);  //Extract the summary
                    tmpData = editorData.Substring(editorData.IndexOf("<h2>About</h2>"), nexth2);
                    //System.Diagnostics.Debug.WriteLine(tmpData);
                    int startIndex = skipHTMLTags(tmpData, getPosition(tmpData, "<p>", 1));
                    while (startIndex < tmpData.Length)
                    {
                        if (tmpData[startIndex] == '<')
                        {
                            startIndex = skipHTMLTags(tmpData, startIndex);
                            profilModel.Zusammenfassung += " ";
                        }
                        else
                        {
                            profilModel.Zusammenfassung += tmpData[startIndex];
                            startIndex++;
                        }
                    }
                    profilModel.Zusammenfassung += "\n";
                    profilModel.Zusammenfassung = profilModel.Zusammenfassung.Replace("&ndash;", " - ");
                    profilModel.Zusammenfassung = profilModel.Zusammenfassung.Replace("&auml;", "ä");
                    profilModel.Zusammenfassung = profilModel.Zusammenfassung.Replace("&uuml;", "ü");
                    profilModel.Zusammenfassung = profilModel.Zusammenfassung.Replace("&ouml;", "ö");
                    profilModel.Zusammenfassung = profilModel.Zusammenfassung.Replace("&nbsp;", " ");
                    profilModel.Zusammenfassung = profilModel.Zusammenfassung.Replace("&#39;", "'");
                    profilModel.Zusammenfassung = profilModel.Zusammenfassung.Replace("&amp;", "&");
                }
                else
                {
                    profilModel.Zusammenfassung += "k.A";
                }

                int h3Count = 1;
                if (editorData.Contains("<h2>Experience</h2>"))
                {                                                 //Extract experience information
                    tmpData = editorData.Substring(editorData.IndexOf("<h2>Experience</h2>"));
                    int posStart = skipHTMLTags(tmpData, getPosition(tmpData, "<h3>", h3Count));
                    int nexth2 = getPosition(tmpData, "<h2>", 2);
                    string tmpPos = "";
                    //profilModel.Erfahrung += "\n";
                    while (posStart < nexth2)
                    {
                        while (tmpData[posStart] != '<')
                        {
                            tmpPos += tmpData[posStart];
                            posStart++;
                        }
                        if (tmpPos.Contains("Company Name"))
                        {
                            profilModel.Erfahrung += "\n" + tmpPos.Split(new string[] { "Company Name" }, StringSplitOptions.None)[1] + "\n";
                        }
                        else if (tmpPos.Contains("Title"))
                        {
                            profilModel.Erfahrung += "* " + tmpPos.Split(new string[] { "Title" }, StringSplitOptions.None)[1] + "\n";
                            string tmpStr = tmpData.Substring(posStart, nexth2);
                            posStart = tmpStr.IndexOf("Dates Employed");
                            while (tmpStr[posStart] != '<')
                            {
                                tmpPos += tmpStr[posStart];
                                posStart++;
                            }
                            profilModel.Erfahrung += "- " + tmpPos.Split(new string[] { "Dates Employed" }, StringSplitOptions.None)[1];
                            posStart = tmpStr.IndexOf("Employment Duration");
                            while (tmpStr[posStart] != '<')
                            {
                                tmpPos += tmpStr[posStart];
                                posStart++;
                            }
                            profilModel.Erfahrung += " = " + tmpPos.Split(new string[] { "Employment Duration" }, StringSplitOptions.None)[1] + "\n";
                        }
                        else
                        {
                            profilModel.Erfahrung += "\n" + tmpPos + "\n";
                            var tmpStr = tmpData.Substring(posStart, nexth2);
                            posStart = skipHTMLTags(tmpStr, getPosition(tmpStr, "<p>", 2));
                            while (tmpStr[posStart] != '<')
                            {
                                profilModel.Erfahrung += tmpStr[posStart];
                                posStart++;
                            }
                            profilModel.Erfahrung += "\n";
                            posStart = tmpStr.IndexOf("Dates Employed");
                            while (tmpStr[posStart] != '<')
                            {
                                tmpPos += tmpStr[posStart];
                                posStart++;
                            }
                            profilModel.Erfahrung += " " + tmpPos.Split(new string[] { "Dates Employed" }, StringSplitOptions.None)[1];
                            posStart = tmpStr.IndexOf("Employment Duration");
                            while (tmpStr[posStart] != '<')
                            {
                                tmpPos += tmpStr[posStart];
                                posStart++;
                            }
                            profilModel.Erfahrung += " =  " + tmpPos.Split(new string[] { "Employment Duration" }, StringSplitOptions.None)[1] + "\n";
                        }
                        profilModel.Erfahrung = profilModel.Erfahrung.Replace("&ndash;", " - ");
                        profilModel.Erfahrung = profilModel.Erfahrung.Replace("&auml;", "ä");
                        profilModel.Erfahrung = profilModel.Erfahrung.Replace("&uuml;", "ü");
                        profilModel.Erfahrung = profilModel.Erfahrung.Replace("&ouml;", "ö");
                        profilModel.Erfahrung = profilModel.Erfahrung.Replace("&nbsp;", " ");
                        profilModel.Erfahrung = profilModel.Erfahrung.Replace("&#39;", "'");
                        profilModel.Erfahrung = profilModel.Erfahrung.Replace("&amp;", "&");
                        tmpPos = "";
                        h3Count++;
                        posStart = skipHTMLTags(tmpData, getPosition(tmpData, "<h3>", h3Count));
                    }
                }
                else
                {
                    profilModel.Erfahrung += "k.A";
                }
                profilModel.Erfahrung += "\n";

                h3Count = 1;
                if (editorData.Contains("<h2>Education</h2>"))
                {                                                      //Extract education information
                    tmpData = tmpData.Substring(tmpData.IndexOf("<h2>Education</h2>"));
                    var eduStart = skipHTMLTags(tmpData, getPosition(tmpData, "<h3>", h3Count));
                    var nexth2 = getPosition(tmpData, "<h2>", 2);
                    var tmpEdu = "";
                    while (eduStart < nexth2)
                    {
                        profilModel.Location += "\n";
                        while (tmpData[eduStart] != '<')
                        {
                            profilModel.Location += tmpData[eduStart];
                            eduStart++;
                        }
                        profilModel.Location += "\n";
                        var tmpStr = tmpData.Substring(eduStart, nexth2);

                        int start;
                        if (tmpStr.Contains("Degree Name"))
                        {
                            start = getPosition(tmpStr, "Degree Name", 1);
                            while (tmpStr[start] != '<')
                            {
                                tmpEdu += tmpStr[start];
                                start++;
                            }
                            profilModel.Location += tmpEdu.Split(new string[] { "Degree Name" }, StringSplitOptions.None)[1] + " ";
                            tmpEdu = "";
                        }

                        if (tmpStr.Contains("Field Of Study"))
                        {
                            start = getPosition(tmpStr, "Field Of Study", 1);
                            while (tmpStr[start] != '<')
                            {
                                tmpEdu += tmpStr[start];
                                start++;
                            }
                            profilModel.Location += tmpEdu.Split(new string[] { "Field Of Study" }, StringSplitOptions.None)[1] + "\n";
                            tmpEdu = "";
                        }
                        else
                        {
                            profilModel.Location += "\n";
                        }

                        if (tmpStr.Contains("Dates attended or expected graduation"))
                        {
                            start = getPosition(tmpStr, "Dates attended or expected graduation", 1);
                            while (tmpStr[start] != '<')
                            {
                                tmpEdu += tmpStr[start];
                                start++;
                            }
                            profilModel.Location += tmpEdu.Split(new string[] { "Dates attended or expected graduation" }, StringSplitOptions.None)[1] + "\n";
                            tmpEdu = "";
                        }

                        h3Count++;
                        eduStart = skipHTMLTags(tmpData, getPosition(tmpData, "<h3>", h3Count));
                    }
                    profilModel.Location = profilModel.Location.Replace("&ndash;", " - ");
                    profilModel.Location = profilModel.Location.Replace("&auml;", "ä");
                    profilModel.Location = profilModel.Location.Replace("&uuml;", "ü");
                    profilModel.Location = profilModel.Location.Replace("&ouml;", "ö");
                    profilModel.Location = profilModel.Location.Replace("&nbsp;", " ");
                    profilModel.Location = profilModel.Location.Replace("&#39;", "'");
                    profilModel.Location = profilModel.Location.Replace("&amp;", "&");
                }
                else
                {
                    profilModel.Location += "k.A";
                }

                if (editorData.Contains("<h2>Skills &amp; endorsements</h2>"))
                {                                                      //Extract skills information
                    tmpData = tmpData.Substring(tmpData.IndexOf("<h2>Skills &amp; endorsements</h2>"));
                    int start = dataArray.IndexOf("Skills &amp; endorsements");
                    int nexth2 = getPosition(tmpData, "<h2>", 2);
                    profilModel.Skill += "\n";
                    for (int i = start + 1; tmpData.IndexOf(dataArray[i]) < nexth2; i++)
                    {
                        string tmpStr = dataArray[i];
                        tmpStr = tmpStr.Replace("&ndash;", " - ");
                        tmpStr = tmpStr.Replace("&auml;", "ä");
                        tmpStr = tmpStr.Replace("&uuml;", "ü");
                        tmpStr = tmpStr.Replace("&ouml;", "ö");
                        tmpStr = tmpStr.Replace("&nbsp;", " ");
                        tmpStr = tmpStr.Replace("&#39;", "'");
                        tmpStr = tmpStr.Replace("&amp;", "&");
                        if (tmpStr.IndexOf("endorse", StringComparison.OrdinalIgnoreCase) == -1 && tmpStr.IndexOf("show less", StringComparison.OrdinalIgnoreCase) == -1 && (int)tmpStr[0] > 32 && tmpStr.IndexOf("LinkedIn Skill Assessment badge", StringComparison.OrdinalIgnoreCase) == -1)
                        {
                            //tmpStr = tmpStr.Replace("\n", "");
                            if (tmpStr.Contains("Industry Knowledge") || tmpStr.Contains("Tools & Technologies") || tmpStr.Contains("Interpersonal Skills") || tmpStr.Contains("Other Skills"))
                            {
                                profilModel.Skill += "\n" + tmpStr + "\n";
                            }
                            else
                            {
                                profilModel.Skill += "- " + tmpStr + "\n";
                            }
                        }
                    }
                    //System.Diagnostics.Debug.WriteLine(WebUtility.HtmlEncode(profilModel.Skill));
                    //System.Diagnostics.Debug.WriteLine(profilModel.Skill);
                }
                else
                {
                    profilModel.Skill += "k.A";
                }
            }
            /*var occurence;
                    if (editorData.includes("Background Image"))
                    {
                        occurence = 3;
                    }
                    else
                    {
                        occurence = 2;
                    }
                    var start = getPosition(editorData, "img", occurence) + 9;
                    var end = start;
                    while (editorData[end) != '"')
                    {
                        imgsrc += editorData[end);
                        end++;
                    }

                    var nameStart = getPosition(editorData, "<h2>", 3) + 4;
                    var nameEnd = nameStart;
                    while (editorData[nameEnd) != '<')
                    {
                        name += editorData[nameEnd);
                        profile.name += editorData[nameEnd);
                        nameEnd++;
                    }

                    tmpData = editorData.substring(nameStart, editorData.length + 1);
                    var jobStart = getPosition(tmpData, "<p>", 1) + 3;    //start index of job/position
                    var tmpJob = "";                    //buffer string for inspecting
                    while (tmpData[jobStart) != '<')
                    {
                        tmpJob += tmpData[jobStart);
                        jobStart++;
                    }
                    if (tmpJob.match(/ degree connection / i) || tmpJob.match(/[(]She[/]Her[)]/ i)                   //if tmpJob contains one of the mentioned 
                        || tmpJob.match(/[(]He[/]Him[)]/ i) || tmpJob.match(/[(]They[/]Them[)]/ i)){
                        jobStart = skipHTMLTags(tmpData, jobStart);                                              //search for next position
                        while (tmpData[jobStart) != '<')
                        {                                                 //look for the next < character and append the prior characters to job 
                            job += tmpData[jobStart);
                            profile.job += tmpData[jobStart);
                            jobStart++;
                        }
                    }else
                    {
                        job += tmpJob;
                    }

                    var tagless = removeTags(editorData);                   //buffer string that cha<<nge every html tags into $
                    const dataArray = stringToArray(tagless, '$');           //turn string into array, only to make it easier to track the address

                    livingAt += dataArray[dataArray.indexOf("Contact info") - 1];                   //the address is always before Contact Info

                    if (editorData.includes("<h2>About</h2>"))
                    {                                                                  //Extract the summary
                        tmpData = editorData.substring(editorData.indexOf("<h2>About</h2>"), editorData.length + 1)
                            var nexth2 = getPosition(tmpData, "<h2>", 2);
                        var startIndex = skipHTMLTags(tmpData, getPosition(tmpData, "<p>", 1));
                        while (startIndex <= nexth2)
                        {
                            if (tmpData[startIndex) == '<')
                            {
                                startIndex = skipHTMLTags(tmpData, startIndex);
                                about += " ";
                            }
                            else
                            {
                                about += tmpData[startIndex);
                                profile.about += tmpData[startIndex);
                                startIndex++;
                            }
                        }
                    }
                    else
                    {
                        about += "k.A";
                    }

                    var h3Count = 1;
                    if (editorData.includes("<h2>Experience</h2>"))
                    {                                                 //Extract experience information
                        tmpData = tmpData.substring(tmpData.indexOf("<h2>Experience</h2>"), tmpData.length + 1);
                        var posStart = skipHTMLTags(tmpData, getPosition(tmpData, "<h3>", h3Count));
                        var nexth2 = getPosition(tmpData, "<h2>", 2);
                        var tmpPos = "";
                        while (posStart < nexth2)
                        {
                            while (tmpData[posStart) != '<')
                            {
                                tmpPos += tmpData[posStart);
                                posStart++;
                            }
                            if (tmpPos.includes("Company Name"))
                            {
                                exp += "<br />" + '&nbsp &nbsp' + (tmpPos.Split(new string[] { "Company Name"))[1] + "<br />";
                                profile.exp += "<br />" + '&nbsp &nbsp' + (tmpPos.Split(new string[] { "Company Name"))[1] + "<br />";
                            }
                            else if (tmpPos.includes("Title"))
                            {
                                exp += '&nbsp &nbsp &nbsp' + "- " + (tmpPos.Split(new string[] { "Title"))[1] + "<br />";
                                profile.exp += '&nbsp &nbsp &nbsp' + "- " + (tmpPos.Split(new string[] { "Title"))[1] + "<br />";
                                var tmpStr = tmpData.substring(posStart, nexth2);
                                posStart = tmpStr.indexOf("Dates Employed");
                                while (tmpStr[posStart) != '<')
                                {
                                    tmpPos += tmpStr[posStart);
                                    posStart++;
                                }
                                exp += '&nbsp &nbsp &nbsp &nbsp' + tmpPos.Split(new string[] { "Dates Employed")[1];
                                profile.exp += '&nbsp &nbsp &nbsp &nbsp' + tmpPos.Split(new string[] { "Dates Employed")[1];
                                posStart = tmpStr.indexOf("Employment Duration");
                                while (tmpStr[posStart) != '<')
                                {
                                    tmpPos += tmpStr[posStart);
                                    posStart++;
                                }
                                exp += "&nbsp=&nbsp" + tmpPos.Split(new string[] { "Employment Duration")[1] + "<br />";
                                profile.exp += "&nbsp=&nbsp" + tmpPos.Split(new string[] { "Employment Duration")[1] + "<br />";
                            }
                            else
                            {
                                exp += "<br />" + '&nbsp &nbsp' + tmpPos + "<br />" + '&nbsp &nbsp';
                                profile.exp += "<br />" + '&nbsp &nbsp' + tmpPos + "<br />" + '&nbsp &nbsp';
                                var tmpStr = tmpData.substring(posStart, nexth2);
                                posStart = skipHTMLTags(tmpStr, getPosition(tmpStr, "<p>", 2));
                                while (tmpStr[posStart) != '<')
                                {
                                    exp += tmpStr[posStart);
                                    profile.exp += tmpStr[posStart);
                                    posStart++;
                                }
                                exp += "<br />";
                                profile.exp += "<br />";
                                posStart = tmpStr.indexOf("Dates Employed");
                                while (tmpStr[posStart) != '<')
                                {
                                    tmpPos += tmpStr[posStart);
                                    posStart++;
                                }
                                exp += '&nbsp &nbsp' + tmpPos.Split(new string[] { "Dates Employed")[1];
                                profile.exp += '&nbsp &nbsp' + tmpPos.Split(new string[] { "Dates Employed")[1];
                                posStart = tmpStr.indexOf("Employment Duration");
                                while (tmpStr[posStart) != '<')
                                {
                                    tmpPos += tmpStr[posStart);
                                    posStart++;
                                }
                                exp += "&nbsp=&nbsp " + tmpPos.Split(new string[] { "Employment Duration")[1] + "<br />";
                                profile.exp += "&nbsp=&nbsp " + tmpPos.Split(new string[] { "Employment Duration")[1] + "<br />";
                            }
                            tmpPos = "";
                            h3Count++;
                            posStart = skipHTMLTags(tmpData, getPosition(tmpData, "<h3>", h3Count));
                        }
                    }
                    else
                    {
                        exp += "k.A";
                    }

                    h3Count = 1;
                    if (editorData.includes("<h2>Education</h2>"))
                    {                                                      //Extract education information
                        tmpData = tmpData.substring(tmpData.indexOf("<h2>Education</h2>"), tmpData.length + 1);
                        var eduStart = skipHTMLTags(tmpData, getPosition(tmpData, "<h3>", h3Count));
                        var nexth2 = getPosition(tmpData, "<h2>", 2);
                        var tmpEdu = "";
                        while (eduStart < nexth2)
                        {
                            education += "<br />" + '&nbsp &nbsp';
                            while (tmpData[eduStart) != '<')
                            {
                                education += tmpData[eduStart);
                                profile.edu += tmpData[eduStart);
                                eduStart++;
                            }
                            education += "<br />";
                            profile.edu += "<br />";
                            var tmpStr = tmpData.substring(eduStart, nexth2);

                            var start;
                            if (tmpStr.includes("Degree Name"))
                            {
                                start = getPosition(tmpStr, "Degree Name", 1);
                                while (tmpStr[start) != '<')
                                {
                                    tmpEdu += tmpStr[start);
                                    start++;
                                }
                                education += '&nbsp &nbsp' + (tmpEdu.Split(new string[] { "Degree Name"))[1] + "&nbsp";
                                profile.edu += '&nbsp &nbsp' + (tmpEdu.Split(new string[] { "Degree Name"))[1] + "&nbsp";
                                tmpEdu = "";
                            }

                            if (tmpStr.includes("Field Of Study"))
                            {
                                start = getPosition(tmpStr, "Field Of Study", 1);
                                while (tmpStr[start) != '<')
                                {
                                    tmpEdu += tmpStr[start);
                                    start++;
                                }
                                education += '&nbsp &nbsp' + (tmpEdu.Split(new string[] { "Field Of Study"))[1] + "<br />";
                                profile.edu += '&nbsp &nbsp' + (tmpEdu.Split(new string[] { "Field Of Study"))[1] + "<br />";
                                tmpEdu = "";
                            }
                            else
                            {
                                education += "<br />";
                                profile.edu += "<br />";
                            }

                            if (tmpStr.includes("Dates attended or expected graduation"))
                            {
                                start = getPosition(tmpStr, "Dates attended or expected graduation", 1);
                                while (tmpStr[start) != '<')
                                {
                                    tmpEdu += tmpStr[start);
                                    start++;
                                }
                                education += '&nbsp &nbsp' + (tmpEdu.Split(new string[] { "Dates attended or expected graduation"))[1] + "<br />";
                                profile.edu += '&nbsp &nbsp' + (tmpEdu.Split(new string[] { "Dates attended or expected graduation"))[1] + "<br />";
                                tmpEdu = "";
                            }

                            h3Count++;
                            eduStart = skipHTMLTags(tmpData, getPosition(tmpData, "<h3>", h3Count));
                        }
                    }
                    else
                    {
                        education += "k.A";
                    }

                    if (editorData.includes("<h2>Skills &amp; endorsements</h2>"))
                    {                                                      //Extract skills information
                        tmpData = tmpData.substring(tmpData.indexOf("<h2>Skills &amp; endorsements</h2>"), tmpData.length + 1);
                        var start = dataArray.indexOf("Skills &amp; endorsements");
                        var nexth2 = getPosition(tmpData, "<h2>", 2);
                        for (var i = start; tmpData.indexOf(dataArray[i]) < nexth2; i++)
                        {
                            var tmpStr = dataArray[i];
                            if (!tmpStr.match(/ endorse / i) && !tmpStr.match(/ show less / i))
                            {
                                skills += '&nbsp &nbsp' + tmpStr + "<br />";
                                profile.skills += '&nbsp &nbsp' + tmpStr + "<br />";
                            }
                        }
                    }
                    else
                    {
                        skills += "k.A";
                    }

                    //Put all information together
                    outputStr += name + "<br />" + "<br />" + job + "<br />" + "<br />" + livingAt + "<br />" + "<br />" + about + "<br />" + "<br />" + exp + "<br />" + "<br />" + education + "<br />" + "<br />" + skills + "<br />" + "<br />";
                    document.getElementById("show").innerHTML = outputStr;          //Show the result on web interface
                    try
                    {
                        document.getElementById("img").innerHTML = "<img id='myImg' width='276' height='276' src=" + imgsrc + ">";
                    }
                    catch (err)
                    {
                        document.getElementById("img").innerHTML = "(no image)";
                    }
                }
                document.getElementById("submit").disabled = true;
                document.getElementById("submit").style.backgroundColor = '#bdbdbd';
                document.getElementById("submit").style.cursor = 'not-allowed';
            });

                function reset()
            {                                                           //Clear everything
                imgsrc = '';
                name = "Name:  ";
                job = "Position/Job  ";
                livingAt = "Stadt/Land/Region:  ";
                about = "Zusammenfassung:  ";
                exp = "Berufserfahrungen:  ";
                education = "Ausbildung:  ";
                skills = "Skills:  ";
                editorData = "";
                outputStr = "";
                editor.setData('');
                document.getElementById("show").innerHTML = "";
                document.getElementById("img").innerHTML = "";
                document.getElementById("submit").disabled = false;
                document.getElementById("submit").style.backgroundColor = '#00A6FF';
                document.getElementById("submit").style.cursor = 'pointer';
                profile.name = "";
                profile.adress = "";
                profile.about = "";
                profile.exp = "";
                profile.edu = "";
                profile.job = "";
                profile.skills = "";
            }




            */
            return profilModel;
        }
        public int getPosition(string s, string match, int occurence)
        {
            int i = 1;
            int index = s.IndexOf(match, 0);


            while (i <= occurence && index != -1)
            {
                if (i == occurence)
                {
                    return index;

                }
                else
                {
                    index = s.IndexOf(match, index + 1);
                    i++;
                }
            }

            return -1;
        }
        public int skipHTMLTags(string source, int start)
        {
            int index = start;
            Boolean isOpen = source[start] == '<';
            while (isOpen)
            {
                index++;
                if (source[index] == '>')
                {
                    if (source[index + 1] != '<')
                    {
                        isOpen = false;
                    }
                    index++;
                }
            }
            return index;
        }
        public List<String> stringToArray(string str, char delim)
        {
            List<String> dataArray = new List<String>();
            string tmp = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != delim)
                {
                    tmp += str[i];
                }
                else
                {
                    if (tmp != "")
                    {
                        dataArray.Add(tmp);
                        tmp = "";
                    }
                }
            }
            return dataArray;
        }

        /*function arrayToString(arr)
        {
            for (var i = 0; i < arr.length; i++)
            {
                console.log(arr[i] + "/");
            }
        }*/

        public string removeTags(string str)
        {
            return Regex.Replace(str, "<.*?>", "$"); //Eliminate every html tag
        }
    }
}