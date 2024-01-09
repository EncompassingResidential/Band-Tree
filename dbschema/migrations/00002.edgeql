CREATE MIGRATION m1u3i77cmmncxvkspkh35uslrqjz2maxto36e4k2kph5upuicsns2q
    ONTO m1rmnx3v6xrsgpfognceruuaj4rxniwukgoe22rigmgpybwr2tza5q
{
  CREATE TYPE default::DeleteMeTable {
      CREATE REQUIRED PROPERTY title: std::str;
      CREATE REQUIRED PROPERTY wikipediapageid: std::int32;
      CREATE REQUIRED PROPERTY wikipediapagetimestamp: std::datetime;
  };
};
