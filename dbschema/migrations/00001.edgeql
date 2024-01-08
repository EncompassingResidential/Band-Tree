CREATE MIGRATION m1rmnx3v6xrsgpfognceruuaj4rxniwukgoe22rigmgpybwr2tza5q
    ONTO initial
{
  CREATE TYPE default::Genre {
      CREATE REQUIRED PROPERTY name: std::str {
          CREATE CONSTRAINT std::exclusive;
          CREATE CONSTRAINT std::min_len_value(3);
      };
      CREATE REQUIRED PROPERTY timestamp: std::datetime;
      CREATE REQUIRED PROPERTY wikipediapageid: std::int32;
  };
  CREATE TYPE default::MusicCompanyLabel {
      CREATE PROPERTY countryoforigin: std::str;
      CREATE PROPERTY foundingdate: std::datetime;
      CREATE PROPERTY location: tuple<city: std::str, state: std::str, country: std::str>;
      CREATE REQUIRED PROPERTY name: std::str {
          CREATE CONSTRAINT std::exclusive;
          CREATE CONSTRAINT std::min_len_value(3);
      };
      CREATE REQUIRED PROPERTY timestamp: std::datetime;
      CREATE REQUIRED PROPERTY wikipediapageid: std::int32;
  };
  CREATE TYPE default::Artist {
      CREATE REQUIRED MULTI LINK genres: default::Genre;
      CREATE MULTI LINK labels: default::MusicCompanyLabel;
      CREATE REQUIRED PROPERTY artistname: std::str {
          CREATE CONSTRAINT std::exclusive;
          CREATE CONSTRAINT std::min_len_value(3);
      };
      CREATE PROPERTY birthdate: std::datetime;
      CREATE PROPERTY familyname: tuple<first: std::str, middle: std::str, last: std::str>;
      CREATE PROPERTY othernames: array<std::str>;
      CREATE REQUIRED PROPERTY timestamp: std::datetime;
      CREATE REQUIRED PROPERTY wikipediapageid: std::int32;
  };
  CREATE TYPE default::BandModel {
      CREATE REQUIRED MULTI LINK currentmembers: default::Artist;
      CREATE MULTI LINK pastmembers: default::Artist;
      CREATE REQUIRED MULTI LINK genres: default::Genre;
      CREATE MULTI LINK labels: default::MusicCompanyLabel;
      CREATE REQUIRED PROPERTY title: std::str {
          CREATE CONSTRAINT std::exclusive;
          CREATE CONSTRAINT std::min_len_value(3);
      };
      CREATE REQUIRED PROPERTY wikipediapageid: std::int32;
      CREATE REQUIRED PROPERTY wikipediapagetimestamp: std::datetime;
  };
};
