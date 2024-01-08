# if you pipe this script into edgedb then they can all use the same DB connection.
# EdgeDB ChatGPT helper called this an EdgeQL script.
#
# This Entire script took 3 seconds VS 10 seconds 
#    for BASH running the same 4 commands with edgedb query "SELECT ....;" commands

# you have to make sure you're connecting to the right database before running these.'
# one example from command line:
#  date; cat BandTree.Server/Doc/EdgeDB.INSERT.commands.sql | edgedb ; date

START TRANSACTION;
SELECT "START TRANSACTION for INSERT Commands";
INSERT DeleteMeTable { title := 'Delete me row', wikipediapageid := 666666, wikipediapagetimestamp
 :=  std::to_datetime('2024-01-08T14:01:23+00:00') } ;

SELECT "SELECT DeleteMeTable { title, wikipediapageid, wikipediapagetimestamp, id } ;";
SELECT DeleteMeTable { title, wikipediapageid, wikipediapagetimestamp, id } ;

SELECT '  End of script date and time  ' ++ <str>std::datetime_current();
COMMIT;
