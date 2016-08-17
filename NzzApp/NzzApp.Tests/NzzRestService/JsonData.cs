namespace NzzApp.Tests.NzzRestService
{
    public class JsonData
    {
        public static string BoxCorrupt => @"
{
    ""path"": ""/api/articles/1.18674268"",
    ""publicationDateTime"": ""2016-01-10T12:41:16+01:00"",
    ""isBreakingNews"": false,
    ""title"": ""Mexikos Staatsanwaltschaft treibt Ausweisung von «El Chapo» voran"",
    ""subTitle"": ""Auslieferung in die USA"",
    ""leadText"": ""Die mexikanische Generalstaatsanwaltschaft hat sich bereit erklärt, den erneut gefassten Drogenboss Joaquín «El Chapo» Guzmán in die USA auszuliefern. Dort liegt ein Haftbefehl gegen ihn vor."",
    ""body"": [
        {
            ""style"": ""p"",
            ""text"": ""Die mexikanische Generalstaatsanwaltschaft hat sich bereit erklärt, den erneut gefassten Drogenboss Joaquín «El Chapo» Guzmán in die USA auszuliefern. Sie werde die Ausweisung vorantreiben, teilte die Anklageinstanz am Samstag (Ortszeit) mit. Der Chef des mächtigen Sinaloa-Kartells war am Freitag in der westmexikanischen Stadt Los Mochis festgenommen worden, ein halbes Jahr nach seiner Flucht aus einem Hochsicherheitsgefängnis."",
            ""boxes"": []
    },
        {
            ""style"": ""p"",
            ""text"": """",
            ""boxes"": [
                []
            ]
        },
        {
            ""style"": ""p"",
            ""text"": ""Die von der US-Justiz im Juni und August 2015 eingereichten Auslieferungsanträge müssten von den zuständigen Gerichten zugelassen und anschliessend vom mexikanischen Aussenministerium befürwortet werden, heisst es in der Mitteilung der Generalstaatsanwaltschaft. Guzmán könne zwar Einspruch gegen die Auslieferung einlegen. Die Generalstaatsanwaltschaft werde aber in dem Fall die Gültigkeit dieser Einsprüche vor Gericht anfechten."",
            ""boxes"": []
        },
        {
            ""style"": ""p"",
            ""text"": """",
            ""boxes"": [
                []
            ]
        },
        {
            ""style"": ""p"",
            ""text"": ""Die USA hatten die Festnahme Guzmáns begrüsst. In den Vereinigten Staaten liegen mehrere Haftbefehle gegen ihn vor."",
            ""boxes"": []
        }
    ],
    ""authors"": [
        {
            ""name"": ""Peter Gaupp, San José de Costa Rica"",
            ""abbreviation"": """"
        }
    ],
    ""relatedArticles"": [
        {
            ""path"": ""/api/articles/1.18607937"",
            ""publicationDateTime"": ""2015-09-06T10:00:00+02:00"",
            ""isBreakingNews"": false,
            ""title"": ""Ein Massenmörder als Volksheld"",
            ""subTitle"": ""Wer ist «El Chapo» Guzmán?"",
            ""teaser"": ""Joaquin Guzmán hat sein Drogenkartell zu der mächtigsten kriminellen Organisation der Welt gemacht. Kaum ein Gangster hat so viel Macht, Geld und..."",
            ""leadImage"": {
                ""guid"": ""1.18607940"",
                ""path"": ""http://images.nzz.ch/app.php/eos/v2/image/view/%width%/%height%/%format%/h/1.18607940.1441398678.jpg"",
                ""source"": ""Mario Guzman / EPA"",
                ""caption"": ""Guzmán konnte nicht glauben, dass ihn die Sicherheitskräfte im letzten Jahr finden und festnehmen konnten."",
                ""mimeType"": ""image/jpeg""
            },
            ""departments"": [],
            ""hasGallery"": false,
            ""isReportage"": false,
            ""isBriefing"": false
        },
        {
            ""path"": ""/api/articles/1.18250490"",
            ""publicationDateTime"": ""2014-02-25T10:29:00+01:00"",
            ""isBreakingNews"": false,
            ""title"": ""«El Chapo» veränderte Mexikos Drogenwelt"",
            ""subTitle"": ""Mit Geschäftssinn, Geld und Gewalt an die Spitze"",
            ""teaser"": ""Bis zu seiner Festnahme führte Joaquín «El Chapo» Guzmán das Sinaloa-Kartell wie ein modernes global tätiges Unternehmen. Mit Rauschmitteln..."",
            ""leadImage"": {
                ""guid"": ""1.18250489"",
                ""path"": ""http://images.nzz.ch/app.php/eos/v2/image/view/%width%/%height%/%format%/h/1.18250489.1393269578.jpg"",
                ""source"": ""Reuters"",
                ""caption"": ""Joaquín Guzmán alias «El Chapo» war am Samstag in der mexikanischen Hafenstadt Mazatlán verhaftet worden."",
                ""mimeType"": ""image/jpeg""
            },
            ""departments"": [],
            ""hasGallery"": false,
            ""isReportage"": false,
            ""isBriefing"": false
        }
    ],
    ""relatedContent"": [],
    ""leadImage"": {
        ""guid"": ""1.18674478"",
        ""path"": ""http://images.nzz.ch/app.php/eos/v2/image/view/%width%/%height%/%format%/h/1.18674478.1452344948.jpg"",
        ""source"": ""Reuters"",
        ""caption"": ""Der mexikanische Drogenboss Joaquín «El Chapo» Guzmán ist nach einem halben Jahr auf der Flucht wieder gefasst worden."",
        ""mimeType"": ""image/jpeg""
    },
    ""departments"": [
        ""International"",
        ""Aktuelle Themen""
    ],
    ""isReportage"": false,
    ""isBriefing"": false,
    ""webUrl"": ""http://www.nzz.ch/international/aktuelle-themen/mexikanischer-drogenboss-el-chapo-gefasst-1.18674268"",
    ""shortWebUrl"": ""http://www.nzz.ch/international/aktuelle-themen/mexikanischer-drogenboss-el-chapo-gefasst-1.18674268""
}";
        public static string BoxDefault => @"
{
    ""path"": ""/api/articles/1.18674268"",
    ""publicationDateTime"": ""2016-01-10T12:41:16+01:00"",
    ""isBreakingNews"": false,
    ""title"": ""Mexikos Staatsanwaltschaft treibt Ausweisung von «El Chapo» voran"",
    ""subTitle"": ""Auslieferung in die USA"",
    ""leadText"": ""Die mexikanische Generalstaatsanwaltschaft hat sich bereit erklärt, den erneut gefassten Drogenboss Joaquín «El Chapo» Guzmán in die USA auszuliefern. Dort liegt ein Haftbefehl gegen ihn vor."",
    ""body"": [
        {
            ""style"": ""p"",
            ""text"": ""Die mexikanische Generalstaatsanwaltschaft hat sich bereit erklärt, den erneut gefassten Drogenboss Joaquín «El Chapo» Guzmán in die USA auszuliefern. Sie werde die Ausweisung vorantreiben, teilte die Anklageinstanz am Samstag (Ortszeit) mit. Der Chef des mächtigen Sinaloa-Kartells war am Freitag in der westmexikanischen Stadt Los Mochis festgenommen worden, ein halbes Jahr nach seiner Flucht aus einem Hochsicherheitsgefängnis."",
            ""boxes"": []
        },
        {
            ""style"": ""p"",
            ""text"": ""Die von der US-Justiz im Juni und August 2015 eingereichten Auslieferungsanträge müssten von den zuständigen Gerichten zugelassen und anschliessend vom mexikanischen Aussenministerium befürwortet werden, heisst es in der Mitteilung der Generalstaatsanwaltschaft. Guzmán könne zwar Einspruch gegen die Auslieferung einlegen. Die Generalstaatsanwaltschaft werde aber in dem Fall die Gültigkeit dieser Einsprüche vor Gericht anfechten."",
            ""boxes"": []
        },
        {
            ""style"": ""p"",
            ""text"": ""Die USA hatten die Festnahme Guzmáns begrüsst. In den Vereinigten Staaten liegen mehrere Haftbefehle gegen ihn vor."",
            ""boxes"": [
                {
                    ""type"": ""video"",
                    ""authorId"": ""0"",
                    ""customerId"": ""0"",
                    ""videoId"": ""105306""
                },
                {
                    ""type"": ""video"",
                    ""authorId"": ""0"",
                    ""customerId"": ""0"",
                    ""videoId"": ""105306""
                }
            ]
        },
        {
            ""style"": ""p"",
            ""text"": """",
            ""boxes"": [
                {
                    ""type"": ""video"",
                    ""authorId"": ""0"",
                    ""customerId"": ""0"",
                    ""videoId"": ""105306""
                }
            ]
        }
    ],
    ""authors"": [
        {
            ""name"": ""Peter Gaupp, San José de Costa Rica"",
            ""abbreviation"": """"
        }
    ],
    ""relatedArticles"": [
        {
            ""path"": ""/api/articles/1.18607937"",
            ""publicationDateTime"": ""2015-09-06T10:00:00+02:00"",
            ""isBreakingNews"": false,
            ""title"": ""Ein Massenmörder als Volksheld"",
            ""subTitle"": ""Wer ist «El Chapo» Guzmán?"",
            ""teaser"": ""Joaquin Guzmán hat sein Drogenkartell zu der mächtigsten kriminellen Organisation der Welt gemacht. Kaum ein Gangster hat so viel Macht, Geld und..."",
            ""leadImage"": {
                ""guid"": ""1.18607940"",
                ""path"": ""http://images.nzz.ch/app.php/eos/v2/image/view/%width%/%height%/%format%/h/1.18607940.1441398678.jpg"",
                ""source"": ""Mario Guzman / EPA"",
                ""caption"": ""Guzmán konnte nicht glauben, dass ihn die Sicherheitskräfte im letzten Jahr finden und festnehmen konnten."",
                ""mimeType"": ""image/jpeg""
            },
            ""departments"": [],
            ""hasGallery"": false,
            ""isReportage"": false,
            ""isBriefing"": false
        },
        {
            ""path"": ""/api/articles/1.18250490"",
            ""publicationDateTime"": ""2014-02-25T10:29:00+01:00"",
            ""isBreakingNews"": false,
            ""title"": ""«El Chapo» veränderte Mexikos Drogenwelt"",
            ""subTitle"": ""Mit Geschäftssinn, Geld und Gewalt an die Spitze"",
            ""teaser"": ""Bis zu seiner Festnahme führte Joaquín «El Chapo» Guzmán das Sinaloa-Kartell wie ein modernes global tätiges Unternehmen. Mit Rauschmitteln..."",
            ""leadImage"": {
                ""guid"": ""1.18250489"",
                ""path"": ""http://images.nzz.ch/app.php/eos/v2/image/view/%width%/%height%/%format%/h/1.18250489.1393269578.jpg"",
                ""source"": ""Reuters"",
                ""caption"": ""Joaquín Guzmán alias «El Chapo» war am Samstag in der mexikanischen Hafenstadt Mazatlán verhaftet worden."",
                ""mimeType"": ""image/jpeg""
            },
            ""departments"": [],
            ""hasGallery"": false,
            ""isReportage"": false,
            ""isBriefing"": false
        }
    ],
    ""relatedContent"": [],
    ""leadImage"": {
        ""guid"": ""1.18674478"",
        ""path"": ""http://images.nzz.ch/app.php/eos/v2/image/view/%width%/%height%/%format%/h/1.18674478.1452344948.jpg"",
        ""source"": ""Reuters"",
        ""caption"": ""Der mexikanische Drogenboss Joaquín «El Chapo» Guzmán ist nach einem halben Jahr auf der Flucht wieder gefasst worden."",
        ""mimeType"": ""image/jpeg""
    },
    ""departments"": [
        ""International"",
        ""Aktuelle Themen""
    ],
    ""isReportage"": false,
    ""isBriefing"": false,
    ""webUrl"": ""http://www.nzz.ch/international/aktuelle-themen/mexikanischer-drogenboss-el-chapo-gefasst-1.18674268"",
    ""shortWebUrl"": ""http://www.nzz.ch/international/aktuelle-themen/mexikanischer-drogenboss-el-chapo-gefasst-1.18674268""
}";
    }
}
