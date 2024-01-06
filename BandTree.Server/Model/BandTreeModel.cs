using System.Text.Json.Serialization;
using EdgeDB;

namespace BandTree.Server.Model
{
    [EdgeDBType]
    public class BandModel
    {
        [JsonPropertyName("title")]
        [EdgeDBProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("pageid")]
        [EdgeDBProperty("wikipediapageid")]
        public string WikipediaPageID { get; set; } = string.Empty;


        [JsonPropertyName("currentmembers")]
        [EdgeDBProperty("currentmembers")]
        public List<Artist> CurrentBandMembers { get; set; } = new List<Artist>();

        [JsonPropertyName("pastmembers")]
        [EdgeDBProperty("pastmembers")]
        public List<Artist>? PastBandMembers { get; set; }

        [JsonPropertyName("genres")]
        [EdgeDBProperty("genres")]
        public List<Genre> GenreList { get; set; } = new List<Genre>();

        // There might have to be another called Distributor's see my NAS iLL Will Records reference
        // in https://github.com/EncompassingResidential/Band-Tree/wiki/Band-Client-Features
        [JsonPropertyName("labels")]
        [EdgeDBProperty("labels")]
        public List<MusicCompanyLabel>? CompanyLabelList { get; set; }

        // Using DateTimeOffset because it handles multiple time zones and/or UTC time saving.
        [JsonPropertyName("timestamp")]
        [EdgeDBProperty("wikipediapagetimestamp")]
        public DateTimeOffset PageTimeStamp { get; set; } = DateTimeOffset.MinValue;
    }

    [EdgeDBType]
    public class Artist
    {
        [JsonPropertyName("name")]
        [EdgeDBProperty("artistname")]
        public string ArtistName { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        [EdgeDBProperty("artistname")]
        public List<string>? OtherNames { get; set; }

        [JsonPropertyName("pageid")]
        [EdgeDBProperty("wikipediapageid")]
        public string WikipediaPageID { get; set; } = string.Empty;

        [JsonPropertyName("birthdate")]
        [EdgeDBProperty("birthdate")]
        public DateTime? BirthDate { get; set; }

        [JsonPropertyName("familyname")]
        [EdgeDBProperty("familyname")]
        public Tuple<string, string, string>? FamilyName { get; set; } = new Tuple<string, string, string>(string.Empty, string.Empty, string.Empty);

        [JsonPropertyName("genres")]
        [EdgeDBProperty("genres")]
        public List<Genre> GenreList { get; set; } = new List<Genre>();

        [JsonPropertyName("labels")]
        [EdgeDBProperty("labels")]
        public List<MusicCompanyLabel>? CompanyLabelList { get; set; }

        [JsonPropertyName("timestamp")]
        [EdgeDBProperty("timestamp")]
        public DateTimeOffset PageTimeStamp { get; set; } = DateTimeOffset.MinValue;

    }

    [EdgeDBType]
    public class Genre
    {
        [JsonPropertyName("genrename")]
        [EdgeDBProperty("name")]
        public string GenreName { get; set; } = string.Empty;

        [JsonPropertyName("pageid")]
        [EdgeDBProperty("wikipediapageid")]
        public string WikipediaPageID { get; set; } = string.Empty;

        [JsonPropertyName("timestamp")]
        [EdgeDBProperty("timestamp")]
        public DateTimeOffset PageTimeStamp { get; set; } = DateTimeOffset.MinValue;
    }

    [EdgeDBType]
    public class MusicCompanyLabel
    {
        [JsonPropertyName("name")]
        [EdgeDBProperty("name")]
        public string LabelName { get; set; } = string.Empty;

        [JsonPropertyName("pageid")]
        [EdgeDBProperty("wikipediapageid")]
        public string WikipediaPageID { get; set; } = string.Empty;

        [JsonPropertyName("foundingdate")]
        [EdgeDBProperty("foundingdate")]
        public DateTime? FoundingDate { get; set; }

        [JsonPropertyName("countryorigin")]
        [EdgeDBProperty("countryoforigin")]
        public string? CountryOfOrigin { get; set; }

        [JsonPropertyName("location")]
        [EdgeDBProperty("location")]
        public Tuple<string, string, string>? CurrentLocation { get; set; } = new Tuple<string, string, string>(string.Empty, string.Empty, string.Empty);

        [JsonPropertyName("timestamp")]
        [EdgeDBProperty("timestamp")]
        public DateTimeOffset PageTimeStamp { get; set; } = DateTimeOffset.MinValue;
    }

}
